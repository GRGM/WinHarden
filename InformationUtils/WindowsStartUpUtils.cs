using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System.IO;
using Microsoft.Win32;

namespace InformationUtils
{
    public class WindowsStartUpUtils
    {
        private string m_extractionBaseFolder;
        private string m_analysisOutputFolder;

        private static List<string> s_autorunHKLMLocations = new List<string>() {
            @"Software\Microsoft\Windows\CurrentVersion\Run",
            @"Software\Microsoft\Windows\CurrentVersion\RunOnce",
            @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run",
            @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce",
            @"Software\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Run",
            @"Software\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Runonce",
            @"Software\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\RunEx",

            //Service Autoruns
            @"Software\Microsoft\Windows\CurrentVersion\RunService",
            @"Software\Microsoft\Windows\CurrentVersion\RunOnceService",
            @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunService",
            @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnceService",
            @"System\CurrentControlSet\Services",
          
            //Special Autorun
            @"Software\Microsoft\Windows\CurrentVersion\RunOnceEx",
            @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnceEx",
            //RunServices
            @"Software\Microsoft\Windows\CurrentVersion\RunServices",

            //RunServicesOnce 
            @"Software\Microsoft\Windows\CurrentVersion\RunServicesOnce",

            //From group policies
            @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run"
            };

        //private static List<string> s_autorunHKCULocations = new List<string>()
        //    {
        //        @"Software\Microsoft\Windows\CurrentVersion\Run",
        //        @"Software\Microsoft\Windows NT\CurrentVersion\Windows\Run",
        //        @"Software\Microsoft\Windows\CurrentVersion\RunOnce",
        //        @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run",
        //        @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce",

        //    //Service Autoruns


        //        @"Software\Microsoft\Windows\CurrentVersion\RunService",
        //        @"Software\Microsoft\Windows\CurrentVersion\RunOnceService",
        //        @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunService",
        //        @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnceService",
            
        //    //Special Autorun

        //        @"Software\Microsoft\Windows\CurrentVersion\RunOnceEx",
        //        @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnceEx",

        //    //RunServices
        //        @"Software\Microsoft\Windows\CurrentVersion\RunServices",            

        //    //RunServicesOnce 
        //        @"Software\Microsoft\Windows\CurrentVersion\RunServicesOnce",
                
        //        //From group policies
        //        @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run"
        //    };


        public WindowsStartUpUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }

        public WindowsStartUpUtils(string extractionBaseFolder, string analysisOutputFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
            m_analysisOutputFolder = analysisOutputFolder;
        }


        internal void GetACLUsersStartUpFolder()
        {
            // C:\Users\XXXXX\AppData\Roaming\Microsoft\Windows\Start Menu\Programs
            string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            int lastIndex = userProfile.LastIndexOf(@"\");
            string userFolder = userProfile.Substring(0, lastIndex);
            DirectoryInfo userDirectory = new DirectoryInfo(userFolder);

            string outputFilename = m_extractionBaseFolder + @"\ACLUsersStartUpFolder.txt";
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            foreach (DirectoryInfo directory in userDirectory.GetDirectories())
            {
                string usersStartUpFolder = directory.FullName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs";
                if (Directory.Exists(usersStartUpFolder))
                {
                    outputFile.WriteLine("Folder path ACL");
                    outputFile.WriteLine(usersStartUpFolder);
                    ACLUtils.ShowSecurityDirectory(usersStartUpFolder, outputFile);
                }
            }
            outputFile.Close();
        }





        internal void GetACLAllUsersStartUp()
        {
            string programData = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
            string allUsersStartUp = programData + @"\Microsoft\Windows\Start Menu\Programs\StartUp";

            //We obtain the ACL in All Users Start Up
            string outputFilename = m_extractionBaseFolder + @"\ACLallUsersStartUp.txt";
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
            outputFile.WriteLine("Folder path ACL");
            outputFile.WriteLine(allUsersStartUp);
            ACLUtils.ShowSecurityDirectory(allUsersStartUp, outputFile);
            outputFile.Close();
        }


        internal void GetACLAllUsersRegistryKey()
        {
            //Start up folder registry keys
            string outputFilename = m_extractionBaseFolder + @"\StartUpRegistryKeys.txt";
            RegistryUtils.WriteHKLMKeyValuesToFile(outputFilename, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders\Common Startup", false, RegistryFormatType.NameAndKey);
            outputFilename = m_extractionBaseFolder + @"\StartUpRegistry64Keys.txt";
            RegistryUtils.WriteHKLMKeyValuesToFile(outputFilename, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders\Common Startup", false, RegistryFormatType.NameAndKey);
        }


        internal void GetACLLocalMachineStartUpKey()
        {
            //We obtain the registry key for AutoRun in HKLM
            string outputFilename = m_extractionBaseFolder + @"\ACLLocalMachineStartUpKey.txt";
            StreamWriter HKLMOutputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            foreach(string key in s_autorunHKLMLocations)
            {
                RegistryKey autorunKey = Registry.LocalMachine.OpenSubKey(key);
                if(autorunKey is null)
                {
                    continue;
                }
                HKLMOutputFile.WriteLine("\nRegistry ACL:");
                HKLMOutputFile.WriteLine(@"HKEY_LOCAL_MACHINE\"+ key);

                RegistrySecurity registrySecurity = autorunKey.GetAccessControl();
                ACLUtils.ShowSecurityRegistry(registrySecurity, HKLMOutputFile);
            }

            HKLMOutputFile.Close();
        }


        internal void GetACLCommandStartUpTasks()
        {
            string outputFilename = m_extractionBaseFolder + @"\ACLCommandStartUpTasks.txt";
            string inputFileName = m_extractionBaseFolder + @"\startUpTasks.csv";
            StreamReader inputFile = new StreamReader(inputFileName, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().ToUpper();
                string hostname = Environment.MachineName.ToUpper();

                if (!line.StartsWith(hostname))
                {
                    continue;
                }
                string[] separator = new string[1] { @"," };
                string[] fields = line.Split(separator, StringSplitOptions.None);

                string caption = fields[1];
                string command = fields[2];
                string user = fields[6];

                outputFile.WriteLine("Autorun Program");
                outputFile.WriteLine(caption + "," + command + "," + user);

                string taskPath = ACLUtils.GetExecutablePath(command);

                ACLUtils.ShowSecurityFile(taskPath, outputFile);

                outputFile.WriteLine();
                outputFile.WriteLine();
            }
            inputFile.Close();
            outputFile.Close();
        }


        public void AnalyseACLCommandStartUpTasks()
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLCommandStartUpTasks.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLCommandStartUpTasks_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Autorun Program"))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    continue;
                }

                if (line.StartsWith("User:"))
                {
                    string user = line.Replace("User: ", "").Trim().ToUpper();
                    string[] separator = new string[1] { @"," };
                    string[] fields = objectToReview.Split(separator, StringSplitOptions.None);
                    string usersExecuting = fields[2].Trim().ToUpper();
                    //If user with privileges is the same that it is running the process, we consider that it is not a way to escalate privileges.
                    if (user == usersExecuting)
                    {
                        continue;
                    }
                    Permission.ProcessStartUpPermission(line, inputFile, outputFile, objectToReview);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        public void AnalyseACLLocalMachineStartUpKey()
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLLocalMachineStartUpKey.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLLocalMachineStartUpKey_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Registry ACL:"))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        public void AnalyseACLUsersStartUpFolder(ProfileAccountUtils profileAccountUtils)
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLUsersStartUpFolder.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLUsersStartUpFolder_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Folder path ACL"))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    //We consider that it is not a misconfiguration that an user may modify its own Program Folder.
                    if (!profileAccountUtils.IsUserProfileFolderSelfAccessed(objectToReview, line))
                    {
                        Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
                    }
                }
            }
            inputFile.Close();
            outputFile.Close();
        }
        public void AnalyseACLAllUsersStartUp()
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLallUsersStartUp.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLallUsersStartUp_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Folder path ACL"))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }
    }
}
