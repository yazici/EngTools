using Pamux.GameDev.Tools.Extensions;
using Pamux.GameDev.Tools.Models;
using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Tools.AssetUtils
{
    public class UnityPackageProxy
    {
        private string fullPath;
        private string unpackedContentDirectory;
        private IArchive archive;

        public UnityPackageProxy(string fullPath)
        {
            this.fullPath = fullPath;
        }

        internal string EnsureUnpacked()
        {
            if (string.IsNullOrWhiteSpace(unpackedContentDirectory) || !Directory.Exists(unpackedContentDirectory))
            {
                if (!UnpackContent(Settings.EngContent))
                {
                    return null;
                }

                unpackedContentDirectory = Settings.EngContent;
            }


            return unpackedContentDirectory;
        }

        private bool SevenZip(string inputFilePath, string outputDirectory)
        {
            if (!File.Exists(Settings.SevenZipCli))
            {
                throw new FileNotFoundException("Needs 7z.exe to unpack a unityPackage content");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException($"{inputFilePath} doesn't exist");
            }
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = Settings.SevenZipCli,
                Arguments = $"x \"{inputFilePath}\" -o\"{outputDirectory}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var p = Process.Start(startInfo);
            while (!p.StandardOutput.EndOfStream)
            {
                Console.WriteLine(p.StandardOutput.ReadLine());
            }

            return p.ExitCode == 0;
        }
        internal class UnpackingContext
        {
            public string hash;

            public string[] pathNameFileContent;

            private string hashedDirectory;
            public string HashedDirectory
            {
                get
                {
                    return hashedDirectory;
                }
                set
                {
                    if (hashedDirectory == value)
                    {
                        return;
                    }
                    hashedDirectory = value;

                    hash = Path.GetFileName(hashedDirectory);

                    pathNameFileContent = File.ReadAllLines($"{hashedDirectory}\\pathname");

                    realAssetPath = $"{outputDirectory}\\{pathNameFileContent[0]}";
                    realAssetDirectory = Path.GetDirectoryName(realAssetPath);
                    realAssetName = Path.GetFileName(realAssetPath);

                    realAssetDirectory.EnsureDirectory();
                }
            }
            public string realAssetPath;
            public string realAssetDirectory;
            public string realAssetName;

            public string firstStepOutputDirectory;
            public string secondStepOutputDirectory;

            private string outputDirectory;
            public string OutputDirectory
            {
                get
                {
                    return outputDirectory;
                }

                internal set
                {
                    if (outputDirectory == value)
                    {
                        return;
                    }
                    outputDirectory = value;
                    firstStepOutputDirectory = $"{outputDirectory}\\1";
                    secondStepOutputDirectory = $"{outputDirectory}\\2";
                }
            }

            public bool CopyAssetToItsPath()
            {
                foreach (var directory in hashedDirectory.EnumerateDirectories())
                {
                    throw new Exception($"There should not be any directories under {hashedDirectory}");
                }

                foreach (var filePath in hashedDirectory.EnumerateFiles("*"))
                {
                    var fileName = Path.GetFileName(filePath);

                    if (fileName == "asset")
                    {
                        continue;
                    }
                    if (fileName == "metaData")
                    {
                        continue;
                    }
                    if (fileName == "pathname")
                    {
                        continue;
                    }
                    if (fileName == "preview.png")
                    {
                        continue;
                    }


                    throw new Exception($"Unknown filename@ {filePath}");
                }

                CopyArchivedAssetFile("asset", $"{realAssetName}");
                CopyArchivedAssetFile("metaData", $"{realAssetName}.meta");
                CopyArchivedAssetFile("preview.png", $"{realAssetName}.preview.png");
                return true;
            }

            private void CopyArchivedAssetFile(string archivedFileName, string realFileName)
            {
                var archivedFilePath = $"{hashedDirectory}\\{archivedFileName}";
                if (File.Exists(archivedFilePath))
                {
                    File.Copy(archivedFilePath, $"{realAssetDirectory}\\{realFileName}");
                }

            }
        }

        private bool UnpackContent(string outputDirectory)
        {
            outputDirectory.RemoveDirectoryRecursively();

            var context = new UnpackingContext();
            context.OutputDirectory = outputDirectory;

            if (!SevenZip(fullPath, context.firstStepOutputDirectory))
            {
                return false;
            }

            foreach (var file in context.firstStepOutputDirectory.EnumerateFiles("*"))
            {
                if (!SevenZip(file, context.secondStepOutputDirectory))
                {
                    return false;
                }
            }

            context.firstStepOutputDirectory.RemoveDirectoryRecursively();

            foreach (var hashedDirectory in context.secondStepOutputDirectory.EnumerateDirectories())
            {
                context.HashedDirectory = hashedDirectory;
                if (!context.CopyAssetToItsPath())
                {
                    return false;
                }
            }

            context.secondStepOutputDirectory.RemoveDirectoryRecursively();

            return true;
        }

        
        internal void ExtractMetaData(IList<string> assets)
        {
            archive = ArchiveFactory.Open(fullPath);
            if (archive == null)
            {
                return;
            }

            foreach (var volume in archive.Entries)
            {
                ProcessArchiveVolume(volume, assets);
            }
        }


        private void ProcessArchiveVolume(IArchiveEntry volume, IList<string> assets)
        {
            Func<IArchiveEntry, string> getFirstLine = (entry) =>
            {
                var path = string.Empty;
                using (var sr = new StreamReader(entry.OpenEntryStream()))
                {
                    path = sr.ReadLine();
                }
                return path;
            };

            var volumeStreamFilePath = Path.GetTempFileName();
            try
            {
                using (var tempStream = File.Open(volumeStreamFilePath, FileMode.Open))
                {
                    volume.WriteTo(tempStream);
                    tempStream.Flush();

                    using (var tempArchive = ArchiveFactory.Open(tempStream))
                    {
                        var pathEntries = from entry in tempArchive.Entries.ToArray()
                                          where Path.GetFileName(entry.Key).Contains("pathname")
                                            && !entry.IsDirectory
                                          select entry;

                        var toExtract = pathEntries.ToDictionary(
                            pathEntry => Path.GetDirectoryName(pathEntry.Key),
                            pathEntry => getFirstLine(pathEntry));

                        var assetEntries = from entry in tempArchive.Entries.ToArray()
                                     where Path.GetFileName(entry.Key).Contains("asset")
                                        && !entry.IsDirectory
                                     select new
                                     {
                                         entry = entry,
                                         path = toExtract[Path.GetDirectoryName(entry.Key)]
                                     };


                        foreach (var asset in assetEntries)
                        {
                            assets.Add(asset.path);
                        }
                    }

                }
            }
            finally
            {
                if (volumeStreamFilePath != null && File.Exists(volumeStreamFilePath))
                {
                    File.Delete(volumeStreamFilePath);
                }
            }
        }
    }
}
