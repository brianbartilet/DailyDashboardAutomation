using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;

using TestWebsite.Hooks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestWebsite.Bindings
{
    [Binding]
    public class AbuGames
    {
        private enum OrderlistColumn
        {
            Name = 1,
            PieceCount,
            CardName,
            Expansion,
            Quality,
            Price
        }

        [Given(@"I am on the order page")]
        public void GivenIAmOnTheOrderPage()
        {   
            TestDriver.ChromeDriver.Navigate().GoToUrl("https://www.abugames.com/shop.cgi");
        }

        [Given(@"my order cart is empty")]
        public void GivenMyOrderCartIsEmpty()
        {
            
        }


        [When(@"I export an order in a spreadsheet in the shopping cart")]
        public void WhenIExportAnOrderInASpreadsheetInTheShoppingCart()
        {
            var xlexcel = new Excel.Application();

            var xlWorkBook = xlexcel.Workbooks.Open("C:\\TestWebsite\\OrderListTarget.xlsx");
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets["OrderList"];

            var xlRange = xlWorkSheet.UsedRange;

            const int rowStart = 2;

            var count = xlRange.Rows.Count;
            var driver = TestDriver.ChromeDriver;

            Thread.Sleep(5000);
            //TOGGLE FOREIGN OPTION
            //driver
            //    .FindElement(By.XPath("//input[@name='displayspeciallinked2']"))
            //    .Click();
            Thread.Sleep(1000);

            for (var i = rowStart; i <= count; i++)
            {   
                Thread.Sleep(3000);

                var nameOwner = (string)(xlWorkSheet.Cells[i, OrderlistColumn.Name] as Excel.Range).Value;

                if (nameOwner != null)
                {
                    var qualityTarget = (string)(xlWorkSheet.Cells[i, OrderlistColumn.Quality] as Excel.Range).Value.ToString();
                    var priceTarget = (string)(xlWorkSheet.Cells[i, OrderlistColumn.Price] as Excel.Range).Value.ToString();
                    var pieceTarget = (string)(xlWorkSheet.Cells[i, OrderlistColumn.PieceCount] as Excel.Range).Value.ToString();
                    var cardNameRaw = (string)(xlWorkSheet.Cells[i, OrderlistColumn.CardName] as Excel.Range).Value.ToString();
                    var expansionName = (string)(xlWorkSheet.Cells[i, OrderlistColumn.Expansion] as Excel.Range).Value.ToString();
                    string languageName = null;

                    if (pieceTarget == "0") continue;

                    var cardName = Utilities.XPathLiteral(cardNameRaw).Trim('\'', '\"');
                    expansionName = Utilities.XPathLiteral(expansionName).Trim('\'', '\"');

                    var messageError = nameOwner + "," + cardName.Replace(",", " ") + "," + expansionName.Replace(",", " ") + "," + qualityTarget + "," + pieceTarget + "," + priceTarget;


                    //CHECK FOR FOREIGN CARDS
                    try
                    {
                        if (cardNameRaw.Contains(" - FOREIGN"))
                        {
                            cardName = cardNameRaw.Split(new string[] { " - " }, StringSplitOptions.None).First().Trim();
                            languageName = cardNameRaw.Split(new string[] { " - " }, StringSplitOptions.None).Last().Trim();
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        driver
                            .FindElement(By.XPath("//form[@name='searchbar2']//td[contains(.,'Card:')]//input[@type='text']"))
                            .SendKeys(cardName);
                        Thread.Sleep(1000);

                        driver
                            .FindElement(By.XPath("//form[@name='searchbar2']//td[contains(.,'Edition:')]//select[contains(.,'Any Edition')]"))
                            .Click();
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        for (i = 0; i < 60; i++)
                        {   
                            Thread.Sleep(5000);
                            driver.Navigate().Refresh();
                        }
                    }

                    if (expansionName.Contains("\'"))
                    {
                        var tempString = expansionName.Split('\'');

                        expansionName = tempString[0];
                    }

                    try
                    {   
                        //CHECK FOR FOIL
                        if (cardName.Contains("- FOIL") || expansionName.Contains("Foil"))
                        {
                            if (expansionName.Contains("Commander"))
                            {
                                Utilities.MoveToElementAndClick(driver
                                    .FindElement(
                                        By.XPath(
                                            "//form[@name='searchbar2']//td[contains(.,'Edition:')]//select[contains(.,'Any Edition')]//option[contains(.,'" +
                                            expansionName + "')]")), TestDriver.ChromeDriver);
                            }
                            else
                            {
                                Utilities.MoveToElementAndClick(driver
                                    .FindElement(
                                        By.XPath(
                                            "//form[@name='searchbar2']//td[contains(.,'Edition:')]//select[contains(.,'Any Edition')]//option[contains(.,'" +
                                            expansionName + " Foil')]")), TestDriver.ChromeDriver);
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Utilities.MoveToElementAndClick(driver
                                .FindElement(
                                    By.XPath(
                                        "//form[@name='searchbar2']//td[contains(.,'Edition:')]//select[contains(.,'Any Edition')]//option[contains(.,'" +
                                        expansionName + "')]")), TestDriver.ChromeDriver);
                            Thread.Sleep(1000);
                        }

                    }
                    catch(Exception)
                    {

                    }


                    driver
                        .FindElement(By.XPath("//form[@name='searchbar2']//td[contains(.,'Card:')]//input[@type='image']"))
                        .Click();
                    Thread.Sleep(2000);

                    IWebElement resultItem;
                    try
                    {
                        if (cardName.Contains("\'"))
                        {
                            var tempString = cardName.Split('\'');

                            cardName = tempString[0];
                        }

                        if (languageName == null)
                        {
                            resultItem = driver
                                .FindElement(By.XPath("//div[@class='cardinfo' and contains(.,'" + cardName + "')]"));
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            resultItem = driver
                                .FindElement(By.XPath("//div[@class='cardinfo' and contains(.,'" + cardName + "') and contains(.,'"+ languageName +"')]"));
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception)
                    {
                        Utilities.WriteToManualAdd(messageError + ",NO RESULTS");

                        continue;
                    }

                    Thread.Sleep(1000);

                    try
                    {
                        if (resultItem.Text.Contains(qualityTarget))
                        {
                            IWebElement quantityInput = null;
                            if (resultItem.Text.Contains(priceTarget))
                            {
                                if (languageName == null)
                                {
                                    quantityInput =
                                        resultItem.FindElement(
                                            By.XPath(".//tr[contains(.,'" + qualityTarget + "') and contains(.,'" +
                                                     priceTarget +
                                                     "')]//input"));
                                }
                                else
                                {
                                    quantityInput =
                                        resultItem.FindElement(
                                            By.XPath(".//tr[contains(.,'" + qualityTarget + "') and contains(.,'" + 
                                                     languageName + "') and contains(.,'" +
                                                     priceTarget +
                                                     "')]//input"));
                                }


                                Thread.Sleep(1000);
                                Utilities.MoveToElementAndClick(quantityInput, driver);

                                //tr[contains(.,'NM-M') and contains(.,'15.99')]//input

                            }
                            else
                            {
                                Utilities.WriteToManualAdd(messageError + ",PRICE IS NOW INVALID. PLEASE UPDATE PRICE");
                            }
                            if (!quantityInput.Enabled)
                            {
                                Utilities.WriteToManualAdd(messageError + ",OUT OF STOCK");
                            }
                            else
                            {
                                quantityInput.SendKeys(pieceTarget);
                                var addCart = driver
                                    .FindElement(
                                        By.XPath(
                                            "//div[contains(@style, 'right')]//input[@type='image' and contains(@src, '/images/addtocart.jpg')]"));
                                Thread.Sleep(1000);
                                Utilities.MoveToElementAndClick(addCart, driver);


                            }
                        }
                        else
                        {
                            Utilities.WriteToManualAdd(messageError + ",INVALID QUALITY");
                        }
                    }
                    catch (Exception)
                    {
                        
                    }

                       
                }


            }

         
            Utilities.releaseObject(xlWorkSheet);
            Utilities.releaseObject(xlWorkBook);
            Utilities.releaseObject(xlexcel);

        }

        public Exception StaleElementReferenceException { get; set; }

        [Then(@"the order would be processed")]
        public void ThenTheOrderWouldBeProcessed()
        {
            while (true)
            {
                Thread.Sleep(1000);
                SystemSounds.Exclamation.Play();
            }
        }


        [Test]
        public void Runner()
        {
            #region Init
            SystemSounds.Exclamation.Play();
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");


            TestDriver.ChromeDriver = new ChromeDriver(@"C:\\TestWebsite\\", options);
            TestDriver.ChromeDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));

            #endregion

            var url = "https://qa-iap.towerswatson.com/SystemAccount/Login";
            var username = "robert.emaden@grc.com";
            var password = "Robert123";

            TestDriver.ChromeDriver.Navigate().GoToUrl(url);


            TestDriver.ChromeDriver.FindElement(By.XPath("//input[@id='emailAddress']")).SendKeys(username);
            TestDriver.ChromeDriver.FindElement(By.XPath("//input[@tw-validate-field='loginForm' and contains(@data-ng-model,'pass')]")).SendKeys(password);
            TestDriver.ChromeDriver.FindElement(By.XPath("//button[contains(.,'Sign In')]")).Click();
        }

    }
}
