using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using AppReferences.Investagrams.Pages.Objects;
using OpenQA.Selenium;

namespace AppReferences.Trello.Tests
{
    [Binding]
    public class InvestagramsSteps
    {
        private readonly IWebDriver _driver;

        #region Get Screener Data

        [Given(@"I on the screener option")]
        public void GivenIOnTheScreenerOption()
        {
            var lp = new Page_Login(_driver);
            lp.LoginAsUser(null, null);
        }

        [When(@"I load a saved screener")]
        public void WhenILoadASavedScreener()
        {

        }

        [Then(@"I can view all results")]
        public void ThenICanViewAllResults()
        {
        
        }

        #endregion

    }
}
