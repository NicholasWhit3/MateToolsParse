using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MateToolsParse
{
    class WikiInfoParse : MateToolsParse
    {
        private string _stringSource;
        private string _strippedStringSource;

        public string StrippedStringSource { get => _strippedStringSource; set => _strippedStringSource = value; }

        string RemoveJunkChars(string inputStr, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(inputStr, string.Empty);
        }

        public string TextStripped(string souceText)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            try
            {
                souceText = RemoveJunkChars(souceText, '(', ')');
                souceText = RemoveJunkChars(souceText, '[', ']');
            }
            catch (Exception strippedExcept)
            {
                Console.WriteLine(strippedExcept.Message);
            }

            souceText = regex.Replace(souceText, " ");
            return souceText;
        }

        public void SourceTextParse()
        {
            var webclient = new WebClient();
            string link = "https://de.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&titles=" + Noun + "&explaintext";
            var pagesource = webclient.DownloadString(link);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pagesource);
            var fnode = doc.GetElementsByTagName("extract")[0];
            _stringSource = fnode.InnerText;
            _strippedStringSource = TextStripped(_stringSource);
            _strippedStringSource = _strippedStringSource.Substring(0, _strippedStringSource.IndexOf('.') + 1);

        }
    }
}
