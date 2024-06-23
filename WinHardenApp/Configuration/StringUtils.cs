using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinHardenApp.Configuration
{
    internal static class StringUtils
    {
        internal static List<int> GetListInt(string listInt)
        {
            List<int> intList = new List<int>();
            string[] separator = new string[1] { @"," };
            string[] tokens = listInt.Split(separator, StringSplitOptions.None);
            foreach (string token in tokens)
            {
                bool isParsed = int.TryParse(token, out int intValue);
                if (isParsed)
                {
                    intList.Add(intValue);
                }
                else
                {
                    return null;
                }
            }
            return intList;
        }

        internal static string GetStringFromListInt(List<int> intList)
        {
            string stringList = "";
            if (intList is null)
            {
                return "";
            }
            if (intList.Count==0)
            {
                return "";
            }
            foreach(int tokenInt in intList)
            {
                stringList += ","+tokenInt.ToString();
            }
            stringList=stringList.Remove(0, 1);
            return stringList;
        }

        internal static bool GetBoolNextLine(StreamReader inputFile,string property)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith(property+"=".ToUpper()))
            {
                string boolStr = line.Replace(property+"=", "");
                bool isParsed = bool.TryParse(boolStr, out bool boolValue);
                if (!isParsed)
                {
                    inputFile.Close();
                    throw new Exception("Not expected configuration line. It should be "+ property + "=TRUE|FALSE. Program has found: " + line);
                }
                return boolValue;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be "+ property + "=TRUE|FALSE. Program has found: " + line);
            }
        }
    }
}
