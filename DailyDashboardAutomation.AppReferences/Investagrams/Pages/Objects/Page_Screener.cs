using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace AppReferences.Investagrams.Pages.Objects
{
    class Page_Screener : BasePage
    {
        public Page_Screener(IWebDriver driver) : base(driver)
        {
        }

        #region Page Objects

        [FindsBy(How = How.XPath, Using = "//select[@id='SavedScreenerDropDownList']")]
        public IWebElement DropdownScreener { get; set; }

        [FindsBy(How = How.XPath, Using = "//select[@id='SavedScreenerDropDownList']//options")]
        public IList<IWebElement> DropdownScreenerList { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='btnLoadSavedScreener']")]
        public IWebElement ButtonLoadScreener { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='btnRunScreener']")]
        public IWebElement ButtonRunScreener { get; set; }

        #endregion

        public override bool DidPageLoad(out string result)
        {
            throw new NotImplementedException();
        }

        public override void WaitForPageToLoad(bool waitForAngular = true)
        {
            throw new NotImplementedException();
        }

        public override List<string[]> GetTableData(string [] column_names)
        {
            throw new NotImplementedException();
        }

        public void LoadScreener(string name)
        {
            DropdownScreener.Click();
            var target = DropdownScreenerList.First(x => x.Text == name);
            target.Click();

            ButtonLoadScreener.Click();
            ButtonRunScreener.Click();

        }
    }
}
