using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pamux.GameDev.Lib.Utilities
{
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

                if (string.IsNullOrWhiteSpace(OutputDirectory))
                {
                    throw new ArgumentNullException(nameof(OutputDirectory));
                }
                if (string.IsNullOrWhiteSpace(PamuxMetaDataDirectory))
                {
                    throw new ArgumentNullException(nameof(PamuxMetaDataDirectory));
                }

                hashedDirectory = value;

                hash = Path.GetFileName(hashedDirectory);

                pathNameFileContent = File.ReadAllLines($"{hashedDirectory}\\pathname");

                RealAssetRelativePath = $"{pathNameFileContent[0]}";
                
                RealAssetDirectory.EnsureDirectory();
                PamuxMetaDataAssetDirectory.EnsureDirectory();
            }
        }
        public string RealAssetRelativePath;
        public string RealAssetRelativeDirectory => Path.GetDirectoryName(RealAssetRelativePath);

        public string RealAssetFullPath => $"{outputDirectory}\\{RealAssetRelativePath}";
        public string RealAssetDirectory => $"{outputDirectory}\\{RealAssetRelativeDirectory}";


        public string RealAssetFileName => Path.GetFileName(RealAssetRelativePath);

        public string PamuxMetaDataDirectory;
        public string PamuxMetaDataAssetDirectory => $"{PamuxMetaDataDirectory}\\{RealAssetRelativeDirectory}";

        public string FirstStepOutputDirectory;
        public string SecondStepOutputDirectory;

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
                FirstStepOutputDirectory = $"{outputDirectory}\\1";
                SecondStepOutputDirectory = $"{outputDirectory}\\2";
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
                if (fileName == "asset.meta")
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

            CopyArchivedAssetFile("asset", $"{RealAssetFileName}", false);

            CopyArchivedAssetFile("asset.meta", ".meta", true);
            CopyArchivedAssetFile("metaData", ".metaData", true);
            CopyArchivedAssetFile("preview.png", ".preview.png", true);

            return true;
        }

        private void CopyArchivedAssetFile(string archivedFileName, string extension, bool alsoCopyToPamuxMetaDataDirectory)
        {
            
            var archivedFilePath = $"{hashedDirectory}\\{archivedFileName}";
            if (File.Exists(archivedFilePath))
            {
                File.Copy(archivedFilePath, $"{RealAssetDirectory}\\{RealAssetFileName}{extension}", true);
                if (alsoCopyToPamuxMetaDataDirectory)
                {
                    File.Copy(archivedFilePath, $"{PamuxMetaDataAssetDirectory}\\{RealAssetFileName}{extension}", true);
                }
            }

        }
    }
}
