using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using InformationUtils.Properties;

namespace InformationUtils
{
    public static class SecurityPolicyUtils
    {
        public static void SaveDefaultSecurityPolicyFile(string fileName)
        {
            StreamWriter configurationtFile = new StreamWriter(fileName, false, Encoding.Default);
            string line;
            using (StreamReader ms = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resources.Basic_Template_Secedit))))
            {
                while ((line = ms.ReadLine()) != null)
                {
                    configurationtFile.WriteLine(line);
                }
            }
            configurationtFile.Close();
        }


        public static void CompareSecurityPolicyFiles(string inputFileName, string securityPolicyFileName, string outputFilename)
        {
            StreamReader securityFile = GetStreamReaderSecurityPolicy(securityPolicyFileName);
            string[] inputLines = File.ReadAllLines(inputFileName);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!securityFile.EndOfStream)
            {
                string line = securityFile.ReadLine();
                ComparePolicyToken(line, inputLines, outputFile);
            }


            outputFile.Close();
            securityFile.Close();

        }

        private static void ComparePolicyToken(string securityPolicyLine, string[] inputLines, StreamWriter outputFile)
        {
            //We ignore lines wihtout = as names of the areas
            int pos = securityPolicyLine.IndexOf("=");
            if (pos < 0)
            {
                return;
            }
            string searchToken = securityPolicyLine.Substring(0, pos).Trim();
            foreach (string inputLine in inputLines)
            {
                if (inputLine.StartsWith(searchToken))
                {
                    if (securityPolicyLine.Trim().ToUpper() != inputLine.Trim().ToUpper())
                    {
                        outputFile.WriteLine("EXPECTED: " + securityPolicyLine);
                        outputFile.WriteLine("FOUND: " + inputLine);
                        outputFile.WriteLine();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            outputFile.WriteLine("EXPECTED: " + securityPolicyLine);
            outputFile.WriteLine("Not found in extracted security policy.");
            outputFile.WriteLine();
        }




        private static StreamReader GetStreamReaderSecurityPolicy(string securityPolicyFile)
        {
            StreamReader inputFile = null;
            if (securityPolicyFile is null || securityPolicyFile.Trim() == "")
            {
                inputFile = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resources.Basic_Template_Secedit)));

            }
            else
            {
                FileStream inputFileStream = new FileStream(securityPolicyFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                inputFile = new StreamReader(inputFileStream, Encoding.Default);
            }
            return inputFile;
        }
    }
}
