// ------------------------------------------------------------------------------------------------
// <copyright file="AssetInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Pamux.GameDev.Lib.Utilities
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public class AssetInfo
    {
        private readonly int[] scales = new[]
        {
            100,
            125,
            150,
            200,
            400,
            0, // This is the end of scale types
            16, // These are target based sizes
            24,
            32,
            48,
            256,
            20,
            30,
            36,
            40,
            42,
            50,
            54,
            60,
            63,
            64,
            72,
            70,
            75,
            80,
            90,
            100,
            120,
            200,
            96
        };

        private readonly string prefix;

        private readonly int w;

        private readonly int h;

        private readonly bool isTargetBased;

        public AssetInfo(string prefix)
        {
            this.prefix = prefix;
            isTargetBased = true;
        }

        public AssetInfo(string prefix, int w, int h)
        {
            this.prefix = prefix;
            this.h = h;
            this.w = w;
            isTargetBased = false;
        }

        public string GetFileNameWithoutExtension(int scaleOrSize)
        {
            return isTargetBased
                       ? $@"{prefix}.targetsize-{scaleOrSize}"
                       : $@"{prefix}.scale-{scaleOrSize}";
        }

        private int ScaledW(int scale) => (int)Math.Ceiling((double)scale * w / 100);

        private int ScaledH(int scale) => (int)Math.Ceiling((double)scale * h / 100);

        public Size GetAssetSize(int scaleOrSize)
        {
            return isTargetBased
                       ? new Size(scaleOrSize, scaleOrSize)
                       : new Size(ScaledW(scaleOrSize), ScaledH(scaleOrSize));
        }

        public string GetAssetSizeString(int scaleOrSize)
        {
            var size = GetAssetSize(scaleOrSize);
            return $@"{size.Width}x{size.Height}";
        }

#if DEBUG
        public string GetFileNameAndSizeString(int scaleOrSize, string extension)
        {
            return $"{GetFileNameWithoutExtension(scaleOrSize)}.{extension}: {GetAssetSizeString(scaleOrSize)}";
        }
#endif

        public void InvokeForAllImageSizes(Action<int> action)
        {
            /*
            Splash Screen: SplashScreen.scale-100.png
            App list icon: Square44x44Logo.scale-100.png
            Large tile: Square310x310Logo.scale-100.png
            Wide tile: Wide310x150Logo.scale - 100.png
            Medium tile: Square150x150Logo.scale - 100.png
            Small tile: Square71x71Logo.scale-100.png
            */

            /*16x16* Square44x44Logo.targetsize - 16.png
            24x24* Square44x44Logo.targetsize - 24.png
            32x32* Square44x44Logo.targetsize - 32.png
            48x48* Square44x44Logo.targetsize - 48.png
            256x256* Square44x44Logo.targetsize - 256.png
            20x20 Square44x44Logo.targetsize - 20.png
            30x30 Square44x44Logo.targetsize - 30.png
            36x36 Square44x44Logo.targetsize - 36.png
            40x40 Square44x44Logo.targetsize - 40.png
            60x60 Square44x44Logo.targetsize - 60.png
            64x64 Square44x44Logo.targetsize - 64.png
            72x72 Square44x44Logo.targetsize - 72.png
            80x80 Square44x44Logo.targetsize - 80.png
            96x96 Square44x44Logo.targetsize - 96.png
            */
            var inTargetBasedValues = false;
            foreach (var scale in scales)
            {
                if (scale == 0)
                {
                    inTargetBasedValues = true;
                    continue;
                }

                if (isTargetBased != inTargetBasedValues)
                {
                    continue;
                }

                action.Invoke(scale);
            }
        }

        public void Save(Bitmap image, int scale, string publishingAssetsDirectory, string win10AssetsDirectory)
        {
            var size = GetAssetSize(scale);

            var destinationImage = new Bitmap(size.Width, size.Height);

            using (var g = Graphics.FromImage(destinationImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Draw image with new width and height
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }

            var fileNameWithoutExtension = GetFileNameWithoutExtension(scale);

            var fullPathWithoutExtension = Path.GetFullPath(Path.Combine(publishingAssetsDirectory, $@"jpg\{fileNameWithoutExtension}.jpg"));
            destinationImage.Save(fullPathWithoutExtension, ImageFormat.Jpeg);
            fullPathWithoutExtension = Path.GetFullPath(Path.Combine(publishingAssetsDirectory, $@"png\{fileNameWithoutExtension}.png"));
            destinationImage.Save(fullPathWithoutExtension, ImageFormat.Png);

            fullPathWithoutExtension = Path.GetFullPath(Path.Combine(win10AssetsDirectory, $@"{fileNameWithoutExtension}.jpg"));
            destinationImage.Save(fullPathWithoutExtension, ImageFormat.Jpeg);
        }
    }
}