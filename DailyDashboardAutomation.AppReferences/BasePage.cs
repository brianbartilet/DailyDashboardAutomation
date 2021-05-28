using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Collections;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using AppReferences.Utilities;
using System.Collections.Generic;

namespace AppReferences
{
    public enum Browsers
    {
        Chrome,
        Headless
    }

    public abstract class BasePage
    {
        public IWebDriver Driver;
        private DriverService DriverService;
        private Browsers _selectedBrowserOption;
        private readonly int _commandTimeOut = 5;
        private int _chromeErrorCount = 1;
        private readonly Guid _objectId = Guid.NewGuid();

        /// <summary>
        /// BrowserDriver constructor with a browser
        /// </summary>
        /// <param name="browser">Browser</param>
        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        #region Browser

        /// <summary>
        /// SelectedBrowserOption property
        /// </summary>
        public Browsers SelectedBrowserOption
        {
            get { return _selectedBrowserOption; }

            set { _selectedBrowserOption = value; }
        }

        /// <summary>
        /// Returns the currently used webdriver
        /// </summary>
        /// <returns>IWebDriver</returns>
        public IWebDriver GetDriver()
        {
            return Driver;
        }


        /// <summary>
        /// Sets the driver browser
        /// </summary>
        /// <param name="browser">Browser</param>
        public void SetDriver()
        {
            Browsers browser;

            if (!string.IsNullOrEmpty(ReadConfigFile.GetSettingAsString("Browser")))
            {
                browser = SwitchBrowser(ReadConfigFile.GetSettingAsString("Browser"));
            }
            else
            {
                browser = Browsers.Chrome;
            }
            _selectedBrowserOption = browser;

            switch (browser)
            {
                case Browsers.Chrome:
                case Browsers.Headless:

                    Driver = GetChromeDriver();
                    break;
                default:
                    Driver = GetChromeDriver();
                    break;
            }

            int timeout;
            Int32.TryParse(ReadConfigFile.GetSettingAsString("PageLoadTimeout"), out timeout);

        }

        /// <summary>
        /// Allows the capability to switch browsers driver
        /// </summary>
        /// <param name="browserName">string</param>
        /// <returns>Browser</returns>
        public Browsers SwitchBrowser(string browserName)
        {
            Browsers retVal = Browsers.Chrome;

            switch (browserName.ToLower())
            {
                case "chrome":
                    retVal = Browsers.Chrome;
                    break;
                case "headless":
                    retVal = Browsers.Headless;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Get the chrome driver with default chrome options to use
        /// </summary>
        /// <returns></returns>
        private IWebDriver GetChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            if (!string.IsNullOrEmpty(ReadConfigFile.GetSettingAsString("DownloadFolderOption")))
            {
                File.IfDirDoesNotExistCreateIt(
                    ReadConfigFile.GetSettingAsString("DownloadFolderOption"));
                chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                chromeOptions.AddUserProfilePreference("download.default_directory", ReadConfigFile.GetSettingAsString("DownloadFolderOption"));
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            }

            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--no-default-browser-check");
            chromeOptions.AddArgument("--disable-accelerated-video");
            chromeOptions.AddArgument("--disable-accelerated-video-decode");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-cache");

            return GetChromeDriver(chromeOptions);
        }

        /// <summary>
        /// Get the chrome driver with default chrome options to use
        /// </summary>
        /// <returns></returns>
        private IWebDriver GetChromeDriverHeadless()
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            if (!string.IsNullOrEmpty(ReadConfigFile.GetSettingAsString("DownloadFolderOption")))
            {
                File.IfDirDoesNotExistCreateIt(
                    ReadConfigFile.GetSettingAsString("DownloadFolderOption"));
                chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                chromeOptions.AddUserProfilePreference("download.default_directory", ReadConfigFile.GetSettingAsString("DownloadFolderOption"));
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            }

            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--no-default-browser-check");
            chromeOptions.AddArgument("--disable-accelerated-video");
            chromeOptions.AddArgument("--disable-accelerated-video-decode");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-cache");
            chromeOptions.AddArgument("--headless");

            return GetChromeDriver(chromeOptions);
        }

        /// <summary>
        /// Gets the chrome driver with the chrome options requested
        /// </summary>
        /// <param name="chromeOptions"></param>
        /// <returns></returns>
        private IWebDriver GetChromeDriver(ChromeOptions chromeOptions)
        {
            IWebDriver retValWebDriver = null;

            var service = ChromeDriverService.CreateDefaultService();
            try
            {
                retValWebDriver = new ChromeDriver(service, chromeOptions, TimeSpan.FromMinutes(_commandTimeOut));
            }
            catch (Exception ex)
            {
                _chromeErrorCount++;
                service.Dispose();

                if (_chromeErrorCount > 3)
                {
                    throw new Exception("Webdriver could not initialize after 3 attempt, terminal error: " + ex.Message);
                }
                else
                {
                    return GetChromeDriver(chromeOptions);
                }
            }
            DriverService = service;

            return retValWebDriver;
        }

        /// <summary>
        /// Closes the driver
        /// </summary>
        public void CloseDriver()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                int explicit_wait;
                Int32.TryParse(ReadConfigFile.GetSettingAsString("ExplicitWaitValue"), out explicit_wait);

                while (stopwatch.Elapsed.TotalSeconds <= explicit_wait)
                {
                    Driver.Quit();

                    if (AreWindowHandlesClosed())
                    {
                        break;
                    }
                    int interval_time;
                    Int32.TryParse(ReadConfigFile.GetSettingAsString("ExplicitWaitValue"), out interval_time);
                    Thread.Sleep(interval_time);
                }

                stopwatch.Stop();

            }
            catch (Exception ex)
            {
                DriverService?.Dispose();
            }
            finally
            {
                if (DriverService != null)
                {
                    if (DriverService.ProcessId != 0)
                    {
                        Process.GetProcessById(DriverService.ProcessId).Kill();
                    }
                }
                DriverService = null;
            }
        }

        /// <summary>
        /// Checks if Window handles still exist
        /// </summary>
        /// <returns></returns>
        public bool AreWindowHandlesClosed()
        {
            try
            {
                if (Driver.WindowHandles.Count > 0)
                {
                    return false;
                }
                ;

            }
            catch
            {
                //an error occurred so window handles are null
                return true;
            }
            return true;
        }

        /// <summary>
        /// This is used to take a screenshot.
        /// </summary>
        public void TakeScreenshot()
        {
            ScreenShot s = new ScreenShot(Driver);
            s.TakeScreenshot();
        }

        #endregion

        #region Elements


        public virtual void WaitForElementToDisappear(By elementLocator, int timeOutInSeconds = 0)
        {
            var timeSpan = TimeSpan.FromSeconds(timeOutInSeconds == 0 ? ReadConfigFile.GetSettingAsInt("TimeOutInSeconds") : timeOutInSeconds);
            WebDriverWait wait = new WebDriverWait(GetDriver(), timeSpan);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement element = d.FindElement(elementLocator);
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        public virtual void WaitForElementToDisappear(By elementLocator, int seconds, string purpose)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            WebDriverWait wait = new WebDriverWait(GetDriver(), timeSpan);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement element = d.FindElement(elementLocator);
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        public virtual void WaitForElementToDisappear(IWebElement inputElement, int timeOutInSeconds = 0)
        {
            var timeSpan = TimeSpan.FromSeconds(timeOutInSeconds == 0 ? ReadConfigFile.GetSettingAsInt("TimeOutInSeconds") : timeOutInSeconds);
            WebDriverWait wait = new WebDriverWait(GetDriver(), timeSpan);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            wait.Until<bool>((d) =>
            {
                try
                {
                    return !inputElement.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        public virtual IWebElement WaitForElementToAppear(By elementLocator)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            IWebElement element = null;
            try
            {
                element = wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + elementLocator);
            }
            return element;
        }

        public virtual IWebElement WaitForElementToAppear(string elementId)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            IWebElement element = null;
            try
            {
                element = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(elementId)));
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + elementId);
            }
            return element;
        }

        public virtual void WaitForElementToAppear(IWebElement inputElement)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            try
            {
                wait.Until<bool>((d) => inputElement.Displayed);
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + inputElement);
            }

        }

        public virtual void WaitForElementToAppear(IWebElement parentElement, By childElementLocator)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            IWebElement childElement = null;
            try
            {
                wait.Until<IWebElement>((d) =>
                {
                    Thread.Sleep(1000);
                    childElement = parentElement.FindElement(childElementLocator);
                    if (childElement.Displayed & childElement.Enabled)
                    {
                        return childElement;
                    }
                    return childElement;
                });
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + childElementLocator);
            }
        }

        public virtual void WaitForElementToNotExist(By elementLocator)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.Until<bool>((driver) =>
            {
                try
                {
                    driver.FindElement(elementLocator);
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }
        
        public virtual void WaitForElementToBeClickable(By elementLocator)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));
            IWebElement element = null;
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + elementLocator);
            }
        }

        public virtual void WaitForElementToBeClickable(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(ReadConfigFile.GetSettingAsInt("TimeOutInSeconds")));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchFrameException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(StaleElementReferenceException));

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (Exception)
            {
                throw new Exception("Page is taking too long to load. Unable to locate element - " + element.Text);
            }
        }

        #endregion

        #region Page

        public abstract bool DidPageLoad(out string result);

        public abstract void WaitForPageToLoad(bool waitForAngular = true);

        #endregion

        #region Tables

        public virtual List<string[]> GetTableData(string[] column_names)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}
