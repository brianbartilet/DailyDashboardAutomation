using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using AppReferences.PSEIStock.Api;
using OpenQA.Selenium;

namespace AppReferences.PSEITools.Steps
{
    [Binding]
    public class PseiToolsSteps
    {

        #region Get Stock Data

        [When(@"I fetch stock information")]
        public void WhenIFetchStockInformation()
        {
            var stockApi = new StockApi();
            ScenarioContext.Current["Stock"] = stockApi.GetStockPrice("APX");
        }

        [Then(@"the stock information is fetched successfully")]
        public void ThenTheStockInformationIsFecthedSuccessfully()
        {
            var stock = (Stock)ScenarioContext.Current["Stock"];
            Assert.IsNotNull(stock.Name);
        }

        #endregion

    }
}
