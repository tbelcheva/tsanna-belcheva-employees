using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTestsEmployeesComparison
{
    class AutomatedTests
    {
        
        IWebDriver driver;

        [SetUp]
        public void StartBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "http://localhost/employees/"; // the web app is hosted on IIS
        }

        [Test]
        public void TestSuccessfulFileLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"D:\data\employees_data.txt");

            string fileName = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].name");
            string fileType = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].type");

            Assert.IsTrue(fileInput.Displayed);
            Assert.NotNull(fileName);
            Assert.IsTrue(fileType == "text/plain");
        }

        [Test]
        public void TestOneMatchingRecord()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"D:\data\employees_data.txt");

            string fileName = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].name");
            string fileType = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].type");
            int rowsCount = Convert.ToInt32(js.ExecuteScript("return document.querySelector('.employees').rows.length"));

            Assert.IsTrue(fileInput.Displayed);
            Assert.NotNull(fileName);
            Assert.IsTrue(fileType == "text/plain");

            Assert.AreEqual(2, rowsCount); // 1 data row + header row
        }

        [Test]
        public void TestDuplicateRecordsAppearance()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"D:\data\employees_data2.txt");

            string fileName = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].name");
            string fileType = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].type");
            int rowsCount = Convert.ToInt32(js.ExecuteScript("return document.querySelector('.employees').rows.length"));

            Assert.IsTrue(fileInput.Displayed);
            Assert.NotNull(fileName);
            Assert.IsTrue(fileType == "text/plain");

            Assert.AreEqual(3, rowsCount); // 2 data rows with duplicate days + header row
        }

        [Test]
        public void TestNoMatchingRecords()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"D:\data\employees_data3.txt");

            string fileName = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].name");
            int rowsCount = Convert.ToInt32(js.ExecuteScript("return document.querySelector('.employees').rows.length"));

            Assert.IsNotNull(fileName);
            Assert.IsTrue(fileInput.Displayed);
            Assert.AreEqual(1, rowsCount); // only header row
        }

        [Test]
        public void TestInvalidDataInTextFile()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"D:\data\employees_data6.txt");

            string fileName = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].name");
            string fileType = (string)js.ExecuteScript("return document.getElementById('fileInput').files[0].type");
            int rowsCount = Convert.ToInt32(js.ExecuteScript("return document.querySelector('.employees').rows.length"));

            Assert.IsTrue(fileInput.Displayed);
            Assert.NotNull(fileName);
            Assert.IsTrue(fileType == "text/plain");

            Assert.AreEqual(1, rowsCount); // only header row
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }

    }
   
}
