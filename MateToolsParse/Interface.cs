using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MateToolsParse
{
    class Interface : WikiInfoParse
    {
        private string _question;
        private string _slashes = "#######################################";
        private string _fileFormat = ".txt";

        public string UserInputQuestion()
        {
            Console.WriteLine("Enter your question:");
            _question = Console.ReadLine();
            return _question;           
        }

        public void SavingForDB()
        {
            File.WriteAllText("DB_File.txt", _slashes + "\n" + Noun +"("
                + InfinitiveVerbForm +", ?)" + "\n" + _question + "\n" + StrippedStringSource +"\n" + _slashes);
        }
        
        public void ApplicationRun()
        {
            MateTableOpen(UserInputQuestion());
            GettingTableInfo();
            SourceTextParse();
            SavingForDB();
        }
    }
}
