// ------------------------------------------------------------------------------------------------
// <copyright file="WindowsStore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Pamux.GameDev.Tools.Tabs
{
    using System;
    using System.Timers;
    using System.Windows.Forms;

    using Pamux.GameDev.Tools.AssetUtils;

    using Timer = System.Windows.Forms.Timer;

    /// <summary>
    /// WindowsStore publishing and maintenance utilities
    /// </summary>
    public partial class WindowsStore : UserControl
    {
        private readonly Timer saveScreenshotTimer = new Timer();

        private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
        {
            saveScreenshotTimer.Stop();
            SaveScreenshot();
            saveScreenshotTimer.Enabled = true;
        }


        public WindowsStore()
        {
            InitializeComponent();

            saveScreenshotTimer.Enabled = false;
            saveScreenshotTimer.Interval = 10000;
            saveScreenshotTimer.Tick += TimerEventProcessor;
        }

        private void ButtonCreateScaledImagesClick(object sender, EventArgs e)
        {
            /*ImageAssetScaler.CreateScaledAssets(
                $@"C:\src\Zodiac\Tools\Data\Crab_Nebula.jpg",
                $@"C:\src\Zodiac\Tools\src\Pamux.GameDev.Tools\TestAssets\scaled");*/

            // https://docs.microsoft.com/en-us/windows/uwp/publish/app-screenshots-and-images

            /*
             * Desktop: 1366 x 768 pixels or larger.Supports 4K images(3840 x 2160).
            Mobile: Images must be one of the following: 1080 x 1920, 1920 x 1080, 768 x 1280, 1280 x 768, 720 x 1280, 1280 x 720, 800 x 480, or 480 x 800 pixels.
                Xbox: 3480 x 2160 pixels or smaller.Supports 4K images(3840 x 2160).
            Holographic: 1268 x 720 pixels or larger.Supports 4K images(3840 x 2160).

            For the best display, keep the following guidelines in mind when creating your screenshots:

Keep critical visuals and text in the top 3/4 of the image. Text overlays may appear on the bottom 1/4. 
Don’t add additional logos, icons, or marketing messages to your screenshots.
Don’t use extremely light or dark colors or highly-contrasting stripes that may interfere with readability of text overlays.

You can also provide a short caption that describes each screenshot in 200 characters or less.
            */
            ImageAssetScaler.CreateScaledAssets(
                $@"C:\src\Zodiac\Tools\Data\Crab_Nebula.jpg",
                $@"C:\src\Zodiac\PublishingAssets",
                $@"C:\src\Zodiac\Win10\Zodiac Alliance\Assets");
        }

        private void ButtonSaveScreenshotFromClipboardClick(object sender, EventArgs e)
        {
            // var fullPath = ImageAssetScaler.SaveScreenshotFromClipboard($@"C:\src\Zodiac\PublishingAssets");
            SaveScreenshot();
        }

        private void SaveScreenshot()
        {
            var fullPath = ImageAssetScaler.SaveScreenshot($@"C:\src\Zodiac\PublishingAssets");

            if (fullPath != null)
            {
                textMessage.Text = fullPath;
            }
            else
            {
                textMessage.Text = string.Empty;
            }
        }

        private void ButtonPeriodicScreenSaveClick(object sender, EventArgs e)
        {
            saveScreenshotTimer.Enabled = !saveScreenshotTimer.Enabled;

            buttonPeriodicScreenSave.Text = saveScreenshotTimer.Enabled 
                ? "Saving... Clickk again to stop" 
                : "Save ScreenShot Periodically";
        }


    }
}