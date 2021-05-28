using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AppReferences.Investagrams.Pages.Objects
{
    class Page_Login : BasePage
    {
        public Page_Login(IWebDriver driver) : base(driver)
        {

        }
        
        #region Page Objects

        [FindsBy(How = How.XPath, Using = "//li[@class='dropdown user-menu']//a[contains(.,'LOGIN')]")]
        public IWebElement ButtonLogin { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='LoginUpdatePanel']//name[@id='LoginUserControlPanel$Username']")]
        public IWebElement ModalEmail { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='LoginUpdatePanel']//name[@id='LoginUserControlPanel$Password']")]
        public IWebElement ModalPassword { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='LoginUpdatePanel']//name[@id='LoginUserControlPanel$LoginButton']")]
        public IWebElement ModalButtonLogin { get; set; }

        #endregion

        public override bool DidPageLoad(out string result)
        {
            throw new NotImplementedException();
        }

        public override void WaitForPageToLoad(bool waitForAngular = true)
        {
            
        }

        public void LoginAsUser(string username, string password)
        {
            Driver.Navigate();

            ButtonLogin.Click();
            ModalEmail.SendKeys(username);
            ModalPassword.SendKeys(password);
            ModalButtonLogin.Click();

            WaitForPageToLoad();

        }
    }
}
