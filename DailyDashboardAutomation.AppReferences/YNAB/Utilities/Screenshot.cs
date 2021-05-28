using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AppReferences.Utilities
{
    class ScreenShot
    {
        private readonly IWebDriver _driver;

        public ScreenShot(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// This is used to take a screenshot.
        /// </summary>
        public void TakeScreenshot(string filename = "")
        {
            Screenshot screenShot = ((ITakesScreenshot)_driver).GetScreenshot();

            filename = string.IsNullOrEmpty(filename) ? DateTime.Now.ToString("yyyyMMdd_hhmmss") : filename;

            filename = Path.GetInvalidFileNameChars()
                .Aggregate(filename, (current, c) => current.Replace(c.ToString(), "_")).Replace(" ", "");

            SaveScreenShot(screenShot, filename);

        }

        /// <summary>
        /// This method will save a screenshot to the server and also to the local machine
        /// </summary>
        /// <param name="screenShot"></param>
        /// <param name="filename"></param>
        private void SaveScreenShot(Screenshot screenShot, string filename)
        {
            string folderPath = GetScreenShotFolder();
            File.IfDirDoesNotExistCreateIt(folderPath);
            screenShot.SaveAsFile(folderPath + "\\" + filename + ".png", 0);

        }

        /// <summary>
        /// Gets the proper screenshot folder to use for saving the screenshot
        /// </summary>
        /// <param name="useServerPath"></param>
        /// <returns></returns>
        private string GetScreenShotFolder()
        {
            string screenShotBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScreenShots", 
                DateTime.Today.ToShortDateString()).Replace("/", "_");
            return screenShotBasePath;

        }

    }
}
