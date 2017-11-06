using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.Lib.Utilities;
using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Lib.Proxies
{
    public class UnityPackageProxy : IUnityPackageProxy
    {
        private UnityPackageMetaData unityPackageMetaData;
        private string unpackedContentDirectory;
        public string UnpackedContentDirectory => unpackedContentDirectory;
        private IArchive archive;

        public UnityPackageProxy(UnityPackageMetaData unityPackageMetaData)
        {
            this.unityPackageMetaData = unityPackageMetaData;
        }

        public void EnsureUnpacked()
        {
            if (string.IsNullOrWhiteSpace(unpackedContentDirectory) || !Directory.Exists(unpackedContentDirectory))
            {
                if (!UnpackContent(unityPackageMetaData.TempUnpackRoot))
                {
                    return;
                }

                unpackedContentDirectory = Settings.EngTempUnpackRoot;
            }
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
        

        private bool UnpackContent(string outputDirectory)
        {
            outputDirectory.RemoveDirectoryRecursively();

            var context = new UnpackingContext
            {
                OutputDirectory = outputDirectory
            };

            if (!SevenZip(unityPackageMetaData.FullPath, context.firstStepOutputDirectory))
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

        
        public void ExtractMetaData(IList<string> assets)
        {
            archive = ArchiveFactory.Open(unityPackageMetaData.FullPath);
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
