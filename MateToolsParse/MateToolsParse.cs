using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MateToolsParse
{
    class MateToolsParse 
    {
        private IWebDriver driver;
        private IWebElement _tableElements;
        private IList<IWebElement> _tableRow;

        private readonly string _url = "http://de.sempar.ims.uni-stuttgart.de/";
        private string _infinitiveVerbForm;
        private string _noun;
        private string _fileName = "MateToolsOutput.txt";

        private int _verbIndex;
        private int _nounIndex;

        private char _questionMark;

        private readonly By _parseButton = By.XPath("//input[@type='submit']");
        private readonly By _inputArea = By.XPath("//textarea[@name='sentence']");

        public string Noun { get => _noun; set => _noun = value; }
        public string InfinitiveVerbForm { get => _infinitiveVerbForm; set => _infinitiveVerbForm = value; }

        public void MateTableOpen(string inputStr)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(_url);

            var input = driver.FindElement(_inputArea);
            input.SendKeys(inputStr);

            var clickButtonParse = driver.FindElement(_parseButton);
            clickButtonParse.Click();

        }

        public void GettingTableInfo()
        {
            _tableElements = driver.FindElement(By.XPath("/html/body/table[2]/tbody"));
            _tableRow = _tableElements.FindElements(By.TagName("tr"));
           
            StringBuilder logString = new StringBuilder();

            foreach (IWebElement row in _tableRow)
            {
                Console.WriteLine(row.Text);
                logString.Append(row.Text).Append(Environment.NewLine);
                string str = logString.ToString();
                FindingVerbInSentence();
                FindingNounInSentence();
                File.WriteAllText("MateToolsOutput.txt", str);
                File.WriteAllText("Verbs.txt", _infinitiveVerbForm + " " + _noun);
            }

        }

        public void FindingVerbInSentence()
        {
            string[] ppos =
            {
                "VVFIN",
                "VVIMP",
                "VVINF",
                "VVIZU",
                "VVPP",
                "VAFIN",
                "VAIMP",
                "VMPP",
                "VMINF",
                "VMFIN",
                "VAINF",
                "VAPP"
            };
            var lines = File.ReadLines(_fileName);
            int lineCounter = 1;

            foreach(var line in lines)
            {
                for(int i = 0; i < ppos.Length; i++)
                {
                    if(line.Contains(ppos[i]))
                    {
                        _infinitiveVerbForm = line.Split(' ').Skip(lineCounter).FirstOrDefault();
                        _verbIndex = line.IndexOf(_infinitiveVerbForm);
                    }
                }
                lineCounter++;
            }
        }

        public void FindingNounInSentence()
        {
            var lines = File.ReadLines(_fileName);
            int lineCounter = 0;

            foreach (var line in lines)
            {              
                if (line.Contains("NN"))
                {
                    _noun = line.Split(' ').Skip(lineCounter).FirstOrDefault();
                    _nounIndex = line.IndexOf(_noun);
                    break;
                }
                lineCounter++;
            }
        }

        //public void IsAQuestionMark() //    $.
        //{
        //}

    }
}
