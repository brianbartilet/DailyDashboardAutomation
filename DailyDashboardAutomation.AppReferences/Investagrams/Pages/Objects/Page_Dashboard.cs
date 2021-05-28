using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AppReferences.Investagrams.Pages.Objects
{
    class Page_Dashboard : BasePage
    {
        public Page_Dashboard(IWebDriver driver) : base(driver)
        {

        }

        #region Page Objects

        public enum Menu
        {
            Screener
        }

        [FindsBy(How = How.XPath, Using = "//li[@class='dropdown user-menu']//a[contains(@href,'#')]")]
        public IWebElement ButtonUserMenuOpen { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='dropdown user-menu open']//a[contains(@href, 'Screener')")]
        public IWebElement MenuButtonScreener { get; set; }

        #endregion

        public override bool DidPageLoad(out string result)
        {
            throw new NotImplementedException();
        }

        public override void WaitForPageToLoad(bool waitForAngular = true)
        {
            throw new NotImplementedException();
        }

        public void GoToMenu(Menu menu)
        {
            ButtonUserMenuOpen.Click();

            
            switch (menu)
            {
                case Menu.Screener:
                    MenuButtonScreener.Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
                
                 
        }
    }
}
