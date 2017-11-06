using Pamux.GameDev.Lib.Extensions;
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

            CopyArchivedAssetFile("asset", $"{realAssetName}");
            CopyArchivedAssetFile("asset.meta", $"{realAssetName}.meta");
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
}
