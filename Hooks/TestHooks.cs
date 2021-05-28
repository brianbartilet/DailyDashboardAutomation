using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using NUnit;
using OpenQA.Selenium.Interactions;
using Shouldly;
using Excel = Microsoft.Office.Interop.Excel;


namespace TestWebsite.Hooks
{
    public static class TestDriver
    {
        public static ChromeDriver ChromeDriver;

    }


    [Binding]
    public class TestHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            SystemSounds.Exclamation.Play();
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");


            TestDriver.ChromeDriver = new ChromeDriver(options);
            TestDriver.ChromeDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));

            string newFileName = "C:\\TestWebsite\\ManualAdd.csv";

            Utilities.RunCommand("taskkill /f /im Excel.exe");

            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }

        }

        [AfterScenario]
        public void AfterScenario()
        {
            Thread.Sleep(5000);
            TestDriver.ChromeDriver.Quit();
        }
    }

    public static class Utilities
    {
        public static void MoveToElementAndClick(IWebElement element, IWebDriver driver)
        {
            var action = new Actions(driver);

            action.MoveToElement(element);
            action.Perform();

            element.Click();
        }

        public static void MoveToElement(IWebElement element, IWebDriver driver)
        {
            var action = new Actions(driver);

            action.MoveToElement(element);
            action.Perform();

        }

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Produce an XPath literal equal to the value if possible; if not, produce
        /// an XPath expression that will match the value.
        /// 
        /// Note that this function will produce very long XPath expressions if a value
        /// contains a long run of double quotes.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <returns>If the value contains only single or double quotes, an XPath
        /// literal equal to the value.  If it contains both, an XPath expression,
        /// using concat(), that evaluates to the value.</returns>
        public static string XPathLiteral(string value)
        {
            // if the value contains only single or double quotes, construct
            // an XPath literal
            if (!value.Contains("\""))
            {
                return "\"" + value + "\"";
            }
            if (!value.Contains("'"))
            {
                return "'" + value + "'";
            }

            // if the value contains both single and double quotes, construct an
            // expression that concatenates all non-double-quote substrings with
            // the quotes, e.g.:
            //
            //    concat("foo", '"', "bar")
            StringBuilder sb = new StringBuilder();
            sb.Append("concat(");
            string[] substrings = value.Split('\"');
            for (int i = 0; i < substrings.Length; i++)
            {
                bool needComma = (i > 0);
                if (substrings[i] != "")
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("\"");
                    sb.Append(substrings[i]);
                    sb.Append("\"");
                    needComma = true;
                }
                if (i < substrings.Length - 1)
                {
                    if (needComma)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("'\"'");
                }

            }
            sb.Append(")");
            return sb.ToString();
        }

        public static void WriteToManualAdd(string message)
        {   
            string newFileName = "C:\\TestWebsite\\ManualAdd.csv";
            string output = message + Environment.NewLine;

            if (!File.Exists(newFileName))
            {
                var header = "Name,Card Name,Expansion,Condition,Quantity,Price,Message" + Environment.NewLine;

                File.WriteAllText(newFileName, header);
            }

            File.AppendAllText(newFileName, output);
            SystemSounds.Exclamation.Play();

        }

        public static void RunCommand(string cmd)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();

        }

    }
}
