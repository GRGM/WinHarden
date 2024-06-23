using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinHardenApp.AnalyzeInformationUtils
{
    internal class AnalyseInitialInformationUtils
    {
        private string m_configurationFilesFolder;
        private string m_analysisOutputFolder;

        internal AnalyseInitialInformationUtils(string configurationFilesFolder, string analysisOutputFolder)
        {
            m_configurationFilesFolder = configurationFilesFolder;
            m_analysisOutputFolder = analysisOutputFolder;
        }

        internal void AnalyseWindowsVersion()
        {
            //ProcessComandWithProgress("net accounts > " + m_extractionBaseFolder + @"\password_policy.txt", "Getting net accounts");
            string inputFileName = m_configurationFilesFolder + @"\ver.txt";
            string outputFilename = m_analysisOutputFolder + @"\ver_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.Trim() == "")
                {
                    continue;
                }
                if (line.StartsWith(@"Microsoft Windows"))
                {
                    outputFile.WriteLine(line);
                    string[] separator = new string[1] { @"." };
                    string[] fields = line.Split(separator, StringSplitOptions.None);

                    string osbuild = fields[2];
                    outputFile.WriteLine("OS build is:" + osbuild);
                    outputFile.WriteLine(@"Visit https://docs.microsoft.com/en-us/windows/release-health/release-information to verify if availability date of the OS build is reasonable. Analyse if not installed last KB articles are related to critical vulnerabilities.");
                    continue;
                }
            }
            inputFile.Close();
            outputFile.Close();
        }

        

        internal void AnalyseListeningPorts()
        {
            string inputFileName = m_configurationFilesFolder + @"\netstat.txt";
            string outputFilename = m_analysisOutputFolder + @"\listening_TCP_Ports_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line.Trim() == "")
                {
                    continue;
                }
                if (line.StartsWith(@"TCP") && line.EndsWith(@"LISTENING"))
                {
                    string[] separator = new string[1] { @" " };
                    string[] fields = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    string localAddress = fields[1];
                    if(localAddress.StartsWith("0.0.0.0:"))
                    {
                        string port = localAddress.Replace("0.0.0.0:", "");
                        outputFile.WriteLine(port);
                    }
                    continue;
                }
            }
            inputFile.Close();
            outputFile.Close();
        }

        internal void AnalyseAntivirus()
        {

            ////Link below explain the product state value
            ////https://ourcodeworld.com/articles/read/878/how-to-identify-detect-and-name-the-antivirus-software-installed-on-the-pc-with-c-on-winforms
            //ProcessComandWithProgress(@"WMIC /Node:localhost /Namespace:\\root\SecurityCenter2 Path AntiVirusProduct Get /Format:List > " + m_extractionBaseFolder + @"\antivirus.txt", "Getting antivirus product");
            
            string inputFileName = m_configurationFilesFolder + @"\antivirus.txt";
            string outputFilename = m_analysisOutputFolder + @"\antivirus_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);


            //https://mspscripts.com/get-installed-antivirus-information-2/

            string[] separator = new string[1] { @"=" };
            string displayName = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if(line.Trim()=="")
                {
                    continue;
                }
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string parameter = fields[0];
                string parameterValue = fields[1];

                if (parameter=="displayName")
                {
                    displayName = parameterValue;
                    continue;
                }
                if (parameter=="productState")
                {
                    //"262144" {$defstatus = "Up to date";$rtstatus = "Disabled"}
                    //"266240" {$defstatus = "Up to date";$rtstatus = "Enabled"}

                    //"262160" {$defstatus = "Out of date";$rtstatus = "Disabled"}
                    //"266256" {$defstatus = "Out of date";$rtstatus = "Enabled"}
                    //"393216" {$defstatus = "Up to date";$rtstatus = "Disabled"}
                    //"393232" {$defstatus = "Out of date";$rtstatus = "Disabled"}
                    //"393488" {$defstatus = "Out of date";$rtstatus = "Disabled"}
                    //"397312" {$defstatus = "Up to date";$rtstatus = "Enabled"}
                    //"397328" {$defstatus = "Out of date";$rtstatus = "Enabled"}
                    //    # Windows Defender
                    //"393472" {$defstatus = "Up to date";$rtstatus = "Disabled"}
                    //"397584" {$defstatus = "Out of date";$rtstatus = "Enabled"}
                    //"397568" {$defstatus = "Up to date";$rtstatus = "Enabled"}


                    if (parameterValue=="397568" || parameterValue =="266240" || parameterValue == "397312")
                    { 
                       outputFile.WriteLine(displayName+ " Antivirus is enabled and up to date:"+ parameterValue);
                    }
                    else
                    {
                        outputFile.WriteLine(displayName + @" Antivirus is not enabled or up to date. See https://ourcodeworld.com/articles/read/878/how-to-identify-detect-and-name-the-antivirus-software-installed-on-the-pc-with-c-on-winform and https://mspscripts.com/get-installed-antivirus-information-2/"
                            +Environment.NewLine+ parameterValue);
                    }
                    //We write next line regarding to timestamp as well.
                    line = inputFile.ReadLine();
                    outputFile.WriteLine(line);
                    continue;
                }
            }
            inputFile.Close();
            outputFile.Close();
        }



        internal void AnalyseDEP()
        {
            ////https://docs.microsoft.com/en-us/troubleshoot/windows-client/performance/determine-hardware-dep-available
            //ProcessComandWithProgress(@"wmic OS Get DataExecutionPrevention_SupportPolicy > " + m_extractionBaseFolder + @"\DEPpolicy.txt", "Getting wmic OS");
            string inputFileName = m_configurationFilesFolder + @"\DEPpolicy.txt";
            string outputFilename = m_analysisOutputFolder + @"\DEPpolicy_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
            inputFile.ReadLine();
            string numberStr = inputFile.ReadLine();
            switch(numberStr)
            {
                case "0":
                    outputFile.WriteLine("DEP is not enabled for any processes");
                    break;
                case "1":
                    outputFile.WriteLine("DEP is enabled for all processes");
                    break;
                case "2":
                    outputFile.WriteLine("Only Windows system components and services have DEP applied");
                    break;
                case "3":
                    outputFile.WriteLine("DEP is enabled for all processes. Administrators can manually create a list of specific applications that do not have DEP applied");
                    break;
            }

            inputFile.Close();
            outputFile.Close();
        }


        private static int GetPropertyInt(string line, string label)
        {
            string data = line.Replace(label, "").Trim();
            data = data.Replace("None","0").Trim();
            data = data.Replace("Never", int.MaxValue.ToString()).Trim();
            return int.Parse(data);
        }
    }

}
