using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationUtils
{
    internal class SearchString
    {
        private List<string> m_strings;
        private List<string> m_folders;
        private string m_resultFolder;
        private int m_numberBefore;
        private int m_numberAfter;
        bool m_replaceNewLine;

        internal SearchString(List<string> strings, List<string> folders, string resultFolder, int numberBefore, int numberAfter, bool replaceNewLine)
        {
            m_strings = strings;
            m_folders = folders;
            m_resultFolder = resultFolder;
            m_numberBefore = numberBefore;
            m_numberAfter = numberAfter;
            m_replaceNewLine = replaceNewLine;
        }


        internal void SearchStringFolder()
        {
            //We search this identifier in each file in the hard disk
            foreach (string folder in m_folders)
            {
                SearchStringFolder(folder);
            }
        }


        private void SearchStringFolder(string folder)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(folder);
            }
            catch
            {
                //the folder is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }

            //We carry out look in-depth
            foreach (string directory in directories)
            {
                SearchStringFolder(directory);
            }
            string[] files = Directory.GetFiles(folder);
            foreach (string file in files)
            {
                SearchStringFile(file);
            }
        }
        private void SearchStringFile(string file)
        {
            //We do several search in each file
            string fileString;
            string fileStringUnicode;
            try
            {
                fileString = File.ReadAllText(file, System.Text.Encoding.ASCII);
                fileString = fileString.Replace("\0", "").ToString().ToUpper();
                fileStringUnicode = File.ReadAllText(file, System.Text.Encoding.Unicode);
                fileStringUnicode = fileStringUnicode.Replace("\0", "").ToString().ToUpper();
            }
            catch
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }
            SearchStrings(fileString, fileStringUnicode, file);
        }




        private void SearchStrings(string fileString, string fileStringUnicode, string file)
        {
            foreach (string stringToSearch in m_strings)
            {
                string stringFileResult = GetStringFileResult(m_resultFolder, stringToSearch);
                //If we find in ASCII, we return;
                if (SearchConcreteString(stringToSearch, fileString, stringFileResult, file))
                {
                    continue;
                }
                SearchConcreteString(stringToSearch, fileStringUnicode, stringFileResult, file);
            }
        }


        private bool SearchConcreteString(string stringToSearch, string fileString, string stringFileResult, string file)
        {
            bool returnValue = false;

            if (m_numberBefore == 0 && m_numberAfter == 0)
            {
                returnValue = SearchFirstOccurence(stringToSearch, fileString, stringFileResult, file);


            }
            else
            {
                returnValue = SearchAllOccurences(stringToSearch, fileString, stringFileResult, file);
            }
            return returnValue;
        }

        private bool SearchFirstOccurence(string stringToSearch, string fileString, string stringFileResult, string file)
        {
            bool returnValue = false;
            string stringUpper = stringToSearch.ToUpper();
            //We remind that fileString (or fileStringUnicode) were changed to upper case in a previous function
            if (fileString.Contains(stringUpper))
            {
                //We only write a line to identify the file where string has been found
                StreamWriter writer = new StreamWriter(stringFileResult, true, System.Text.Encoding.Default);
                writer.WriteLine(file + " : " + stringUpper);
                writer.Close();
                return true;
            }
            return returnValue;
        }


        private bool SearchAllOccurences(string stringToSearch, string fileString, string stringFileResult, string file)
        {
            string stringUpper = stringToSearch.ToUpper();
            bool returnValue = false;
            string[] searchStringArray = new string[1];
            searchStringArray[0] = stringUpper;
            string[] results = null;
            try
            {
                results = fileString.Split(searchStringArray, StringSplitOptions.None);
            }
            catch
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                //It is possible that file is too big and it generates outofmemory exception. In this case, we consider that 
                //we miss a relevant password in such type of files, because passwords are usually recorded in small files...
                //We force a call to garbage collector to prevent possible next outofmemory exceptions
                GC.Collect();
                return true;
            }

            int occurences = results.Length - 1;
            for (int i = 0; i < occurences; i++)
            {
                returnValue = true;
                string before = GetBeforeString(results, i);
                string middle = stringToSearch;
                string after = GetAfterString(results, i);
                string surroundingString = before + middle + after;
                if (m_replaceNewLine)
                {
                    //We replace new line by space to have a structured file
                    surroundingString = surroundingString.Replace(Environment.NewLine, " ");
                    surroundingString = surroundingString.Replace("\r", " ");
                    surroundingString = surroundingString.Replace("\n", " ");
                }
                else
                {
                    //In this case, we would have an structure file where each record could have several lines. But for a visual analysis is better separating records with new lines
                    surroundingString = Environment.NewLine + surroundingString + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                }
                StreamWriter writer = new StreamWriter(stringFileResult, true, System.Text.Encoding.Default);
                writer.WriteLine(file + " : " + stringUpper + ":" + surroundingString);
                writer.Close();
            }
            return returnValue;
        }


        private string GetBeforeString(string[] results, int beforeIndex)
        {
            string beforeString = results[beforeIndex];
            if (String.IsNullOrEmpty(beforeString))
            {
                return "";
            }
            if (beforeString.Length <= m_numberBefore)
            {
                return beforeString;
            }
            else
            {
                string returnBefore = beforeString.Substring(beforeString.Length - m_numberBefore, m_numberBefore);
                return returnBefore;
            }
        }
        private string GetAfterString(string[] results, int index)
        {
            int afterIndex = index + 1;
            string afterString = results[afterIndex];
            if (String.IsNullOrEmpty(afterString))
            {
                return "";
            }
            if (afterString.Length <= m_numberAfter)
            {
                return afterString;
            }
            else
            {
                string returnAfter = afterString.Substring(0, m_numberAfter);
                return returnAfter;
            }
        }



        private string GetStringFileResult(string resultFolder, string stringToSearch)
        {
            string newuser = stringToSearch.Replace(@"\", "_");
            newuser = newuser.Replace(@"/", "_");
            newuser = newuser.Replace(@":", "_");
            newuser = newuser.Replace(@"=", "_");
            newuser = newuser.Replace("\"", "_");
            newuser = newuser.Replace("<", "_");
            newuser = newuser.Replace(">", "_");
            string userFileResult = resultFolder + @"\" + newuser + ".txt";
            return userFileResult;
        }
    }
}
