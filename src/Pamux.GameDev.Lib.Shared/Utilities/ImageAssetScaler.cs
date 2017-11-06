// ------------------------------------------------------------------------------------------------
// <copyright file="ImageAssetScaler.cs" company="Microsoft Corporation">
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
    using System.Windows.Forms;

    public class ImageAssetScaler
    {
        public static readonly AssetInfo[] ScaledAssetTypes = new[]
        {
            new AssetInfo("SplashScreen", 620, 300),
            new AssetInfo("Square44x44Logo", 44, 44),
            new AssetInfo("Square310x310Logo", 310, 310),
            new AssetInfo("Wide310x150Logo", 310, 150),
            new AssetInfo("Square150x150Logo", 150, 150),
            new AssetInfo("Square71x71Logo", 71, 71),
            new AssetInfo("StoreLogo", 50, 50),

            new AssetInfo("Square44x44Logo")
        };

        public static void InvokeForAllImageSizes(Action<AssetInfo, int> action)
        {
            foreach (var asset in ScaledAssetTypes)
            {
                asset.InvokeForAllImageSizes(
                    (scale) => { action.Invoke(asset, scale); });
            }
        }

#if DEBUG
        public static void DumpNames()
        {
            InvokeForAllImageSizes(
                (asset, scale) =>
                {
                    Console.WriteLine(asset.GetFileNameAndSizeString(scale, ".png"));
                    Console.WriteLine(asset.GetFileNameAndSizeString(scale, ".jpg"));
                });
        }

#endif

        public static void ScaleAndSaveAll(Bitmap image, string publishingAssetsDirectory, string win10AssetsDirectory)
        {
            InvokeForAllImageSizes(
                (asset, scale) => { asset.Save(image, scale, publishingAssetsDirectory, win10AssetsDirectory); });
        }

        public static bool CreateScaledAssets(string originalImageFilePath, string publishingAssetsDirectory, string win10AssetsDirectory)
        {
            try
            {
                if (!File.Exists(originalImageFilePath))
                {
                    return false;
                }

                if (!Directory.Exists(publishingAssetsDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(win10AssetsDirectory))
                {
                    return false;
                }

                var image = new Bitmap(originalImageFilePath);

                ScaleAndSaveAll(image, publishingAssetsDirectory, win10AssetsDirectory);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static string SaveScreenshotFromClipboard(string destinationDirectory)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                return null;
            }

            if (!Clipboard.ContainsImage())
            {
                return null;
            }

            return SaveScreenshot(Clipboard.GetImage() as Bitmap, destinationDirectory);
        }

        public static string SaveScreenshot(string destinationDirectory)
        {
            var image = new Bitmap(
                Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height,
                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(
                    Screen.PrimaryScreen.Bounds.X,
                    Screen.PrimaryScreen.Bounds.Y,
                    0,
                    0,
                    Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);
            }

            return SaveScreenshot(image, destinationDirectory);
        }

        private static string SaveScreenshot(Bitmap image, string destinationDirectory)
        {
            if (image == null)
            {
                return null;
            }

            // var clipRect = new Rectangle(0, 0, image.Width, image.Height);

            // Left monitor only, removing Unity
            var topCrop = 109;
            var bottomCrop = 64;
            var leftCrop = 1;
            var rightCrop = 1;

            var clipRect = new Rectangle(
                leftCrop, 
                topCrop, 
                image.Width - leftCrop + rightCrop, 
                image.Height - topCrop - bottomCrop);
            var destRect = new Rectangle(0, 0, 1920, 1080);
            image = ClipImage(image, clipRect);

            for (var order = 1; order < 99; ++order)
            {
                var fullPath = Path.GetFullPath(Path.Combine(destinationDirectory, $"ScreenShot{order:00}.png"));
                if (File.Exists(fullPath))
                {
                    continue;
                }

                image.Save(fullPath, ImageFormat.Png);
                return fullPath;
            }

            return null;
        }

        public static Bitmap ClipImage(Bitmap image, Rectangle clipRect)
        {
            var destinationImage = new Bitmap(clipRect.Width, clipRect.Height);

            using (var g = Graphics.FromImage(destinationImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Draw image with new width and height
                g.DrawImage(
                    image,
                    0,
                    0,
                    clipRect,
                    GraphicsUnit.Pixel);
            }

            return destinationImage;
        }
    }
}