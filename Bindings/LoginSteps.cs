using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using Shouldly;
using TechTalk.SpecFlow;
using TestWebsite.Hooks;

namespace TestWebsite.Bindings
{

    [Binding]
    [Scope(Tag = "Module:Login")]
    public class LoginSteps
    {
        #region Fields

        private static string _username;
        private static string _password;
        private static string _url;
        private static readonly IWebDriver _driver = TestDriver.ChromeDriver;

        #endregion

        #region Private Methods

        private void LoginUser()
        {
            _driver.Navigate().GoToUrl(_url);

            _driver.FindElement(By.XPath("//input[@id='emailAddress']")).SendKeys(_username);
            _driver.FindElement(By.XPath("//input[@tw-validate-field='loginForm' and contains(@data-ng-model,'pass')]")).SendKeys(_password);
            _driver.FindElement(By.XPath("//button[contains(.,'Sign In')]")).Click();
        }

        private void SetValidCredentials(string persona)
        {
            switch (persona.Trim())
            {
                case "Connie":
                    _username = "automation.super.connie@brc1302c.com";
                    _password = "azuser1234";
                    _url = "http://auto-iap/ClientAccount/Login";
                    break;
                case "Gina":
                    _username = "automation.gina@brc1302c.com";
                    _password = "azuser1234";
                    _url = "http://auto-iap/ClientAccount/Login";
                    break;
                case "Robert":
                case "Andie":
                    _username = "automation.robert@grc.com";
                    _password = "azuser12345670";
                    _url = "http://auto-iap/SystemAccount/Login";
                    break;
                case "Cathy":
                    _username = "automation.cathy@brc1302c.com";
                    _password = "password1";
                    _url = "http://auto-iap/ClientAccount/Login";
                    break;
                default:
                    throw new Exception("Invalid persona.");
            }

        }

        private void CheckPageLanding(string persona)
        {
            switch (persona.Trim())
            {
                case "Connie":
                case "Gina":
                    TestDriver.ChromeDriver
                        .FindElement(By.XPath("//div[contains(@class,'gmp-container-fluid') and contains(.,'Profile')]"))
                        .Displayed
                        .ShouldBe(true);
                    break;
                case "Robert":
                    TestDriver.ChromeDriver
                        .FindElement(By.XPath("//div[contains(@class,'navbar-inner') and contains(.,'Profile')]"))
                        .Displayed
                        .ShouldBe(true);
                    break;
                case "Cathy":
                    TestDriver.ChromeDriver
                        .FindElement(By.XPath("//div[@class='navbar-inner' and contains(.,'Willis')]"))
                        .Displayed
                        .ShouldBe(true);
                    break;
                default:
                    throw new Exception("Invalid persona.");
            }
        }

        private static void NavigateToPage(string headerLink, string subMenu)
        {   
            Thread.Sleep(2000);

            var header = _driver
                .FindElement(By.XPath("//li[@class='dropdown topmenubar' and contains(.,'" + headerLink + "')]"));

            Utilities.MoveToElementAndClick(header, _driver);

            var submenu = _driver.FindElement(By.Name(subMenu));
            Utilities.MoveToElementAndClick(submenu, _driver);
        }

        private static void ParseCurrentProject(Dictionary<string, string> checks)
        {
            if (checks.ContainsKey("Project"))
            {
                if (checks["Project"] == "Current")
                {
                    switch (ScenarioContext.Current["persona"].ToString())
                    {
                        case "Robert":
                            checks["Project"] = "";
                            break;
                        case "Gina":
                            break;
                        case "Connie":
                            break;
                        case "Cathy":
                            break;
                        default:
                            throw new Exception("Invalid persona");


                    }
                }
            }
        }

        private static void ParseCurrentClient(Dictionary<string, string> checks)
        {
            if (checks.ContainsKey("Client"))
            {
                if (checks["Client"] == "Current")
                {
                    switch (ScenarioContext.Current["persona"].ToString())
                    {
                        case "Robert":
                            checks["Client"] = "";
                            break;
                        case "Gina":
                            break;
                        case "Connie":
                            break;
                        case "Cathy":
                            break;
                        default:
                            throw new Exception("Invalid persona");


                    }
                }
            }
        }

        private static bool DidAuditLogExists(Table table)
        {


            var checks = Utilities.CreateDictionaryFromTable(table);



            //convert if blocks to method calls
            ParseCurrentClient(checks);
            ParseCurrentProject(checks);

            if (checks.ContainsKey("Username"))
            {
                if (checks["Username"] == "Current")
                {
                    checks["Username"] = _username;
                }
            }




            if (checks.ContainsKey("User Type"))
            {
                if (checks["User Type"] == "Current")
                {
                    switch (ScenarioContext.Current["persona"].ToString())
                    {
                        case "Robert":
                            checks["User Type"] = "Internal User";
                            break;
                        case "Gina":
                            break;
                        case "Connie":
                            break;
                        case "Cathy":
                            break;
                        default:
                            throw new Exception("Invalid persona");


                    }
                }
            }

            if (checks.ContainsKey("Activity"))
            {
                if (checks["Activity"] == "Current")
                {
                    switch (ScenarioContext.Current["persona"].ToString())
                    {
                        case "Robert":
                            checks["Activity"] = "Internal user logged in.";
                            break;
                        case "Gina":
                            break;
                        case "Connie":
                            break;
                        case "Cathy":
                            break;
                        default:
                            throw new Exception("Invalid persona");
                    }
                }
            }

            //fetch first all row in the page
            var rows = _driver.FindElements(By.XPath("//tbody//tr"));
            var output = false;

            //loop per row
            foreach (var row in rows)
            {
                //loop within row from checks
                foreach (var check in checks)
                {
                    //the output will stay as true unless the any text
                    //in the checks is not found
                    if (row.Text.Contains(checks[check.Key]))
                    {
                        output = true;
                    }

                }

                //any occurence of output is true, not need to check
                //succeeding rows
                if (output)
                {
                    break;
                }

            }

            return output;
        }

        #endregion

        #region Given

        [Given(@"I a (.*) user")]
        public void GivenIAmUser(string persona)
        {
            ScenarioContext.Current["persona"] = persona;
        }

        #endregion

        #region When

        [When(@"I login to the system using valid credentials")]
        public void WhenILoginToTheSystemUsingValidCredentials()
        {   
            SetValidCredentials((string)ScenarioContext.Current["persona"]);
            LoginUser();
        }

        [When(@"I login to the system using invalid credentials")]
        public void WhenILoginToTheSystemUsingInvalidCredentials()
        {

        }

        #endregion

        #region Then

        [Then(@"I would be successfully logged in to the system")]
        public void ThenIWouldBeSuccessfullyLoggedInToTheSystem()
        {
            CheckPageLanding((string) ScenarioContext.Current["persona"]);
        }

        [Then(@"I am directed to the System Administration landing page")]
        public void ThenIAmDirectedToTheSystemAdministrationLandingPage()
        {

        }

        [Then(@"the Audit Log is recorded")]
        public void ThenTheAuditLogIsRecorded(Table table)
        {
            SetValidCredentials("Andie");
            LoginUser();
            NavigateToPage("Administration","Audit Log");

            //main check
            DidAuditLogExists(table).ShouldBe(true);



        }

        [Then(@"I would not be successfully logged in to the system")]
        public void ThenIWouldNotBeSuccessfullyLoggedInToTheSystem()
        {

        }

        #endregion

    }

    

}
