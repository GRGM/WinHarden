using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using WinHardenApp.Configuration;
using InformationUtils;
using System.DirectoryServices.AccountManagement;

namespace WinHardenApp.AnalyzeInformationUtils
{
    internal class AnalyseWindowsInformationUtils
    {
        private string m_configurationFilesFolder;
        private string m_analysisOutputFolder;
        private WindowsInformationUtils m_windowsInformationUtils;


        internal AnalyseWindowsInformationUtils(string configurationFilesFolder, string analysisOutputFolder, WindowsInformationUtils windowsInformationUtils)
        {
            m_configurationFilesFolder = configurationFilesFolder;
            m_analysisOutputFolder = analysisOutputFolder;
            m_windowsInformationUtils = windowsInformationUtils;
        }

        /// <summary>
        /// We change regional configuration to same configuration of the extracted host. So, some anaylsis are generic
        /// to prevent reginal setting dependencies.
        /// </summary>
        internal void ChangeRegionalConfiguration()
        {

            string regionalConfigurationFile = m_configurationFilesFolder + @"\regionalconfiguration.txt";
            string culture = File.ReadAllText(regionalConfigurationFile);
            CultureInfo.CurrentCulture = new CultureInfo(culture, false);
        }


        internal void AnalyseWindowsInformation()
        {
            //Reviewed url with suggestion about registry policies. These links were used to generate a default registry policy
            //https://www.stigviewer.com/stig/windows_10/2021-08-18/MAC-3_Sensitive/

            m_windowsInformationUtils.UpdateProgressBar("Copying hostname file");
            File.Copy(m_configurationFilesFolder + @"\hostname.txt", m_analysisOutputFolder + @"\hostname.txt",true);

            m_windowsInformationUtils.UpdateProgressBar("Changing regional information");
            ChangeRegionalConfiguration();

            //We use this object to reduce false positives about files and folders belonging to C:\Users\account and modified by the own account.
            ProfileAccountUtils profileAccountUtils= ProfileAccountUtils.GetProfileAccountUtils(m_configurationFilesFolder + @"\set.txt");

            m_windowsInformationUtils.UpdateProgressBar("Analysing initial information");
            AnalyseInitialInformation();

            //Analysing ACL permissions on Shares requires administrator privileges.
            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL shares information");
            AnalyseACLShares();

            ////This function analyses the security of the services. It obtains the access control of the refistry of the keys, access control of the paths containing the services and their status.
            ////So, auditor will be able to reivew if binary planting in services is possible: through modification in the registry or modification in the path of the binary.
            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL services information");
            AnalyseACLService();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL WDF services information");
            AnalyzeACLWDFService();

            //This function is similtar to result above, but service information was obtained in a different way. It obtains less information than function above.
            m_windowsInformationUtils.UpdateProgressBar("Analysing Service rights information");
            AnalyseServiceRights();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL unquoted services");
            AnalyseACLUnquotedService();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL unquoted search paths of scheduled tasks");
            AnalyseACLUnquotedSearchPath();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL previous programas");
            AnalyseACLPreviousPrograms(profileAccountUtils);

            m_windowsInformationUtils.UpdateProgressBar("Analysing Service security rules");
            AnalyseServiceSecurityRulesControl();

            ////This function obtains the security of the environmental variable path. This analysis may be useful to identify vulnerable binaries to DLL hijacking.
            ///In any case, Microsoft does not consider this matter as a vulnerability. Path below is included by default and allows deploying DLL to explit DLL hijacking in affected sotware.
            ///C:\Users\user\AppData\Local\Microsoft\WindowsApps
            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of environmental path variablle folders");
            AnalyseACLPath();

            ////This function obtains the security of the running processes. This analysis may be useful to identify vulnerable binaries to binary planting.
            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of running processes information");
            AnalyseACLProcesses();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of scheduled tasks information");
            AnalyseACLScheduledTasks();

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of start up tasks information");
            AnalyseACLStartUpTasks(profileAccountUtils);

            ////This funcion obtains the security of the folders containing Windows drivers, which are executed under system/kernel privileges.

            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of drivers information");
            AnalyseACLDrivers();

            ////This function obtains the security of the drives.
            m_windowsInformationUtils.UpdateProgressBar("Analysing ACL of drives information");
            AnalyseACLDrives();

            ////This function is useful to retrieve all the arguments in the running processes, due to sometimes user and passwords are included as parameters.
            m_windowsInformationUtils.UpdateProgressBar("Analysing arguments of running processes information");
            AnalyseArgumentProcesses();

            m_windowsInformationUtils.UpdateProgressBar("Analysing local users information");
            AnalyseLocalUserInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting users from local administrators group");
            AnalyseUsersFromLocalAdministrators();

            m_windowsInformationUtils.UpdateProgressBar("Analysing relevant registry keys");
            AnalyseRelevantRegistryKeys();

            m_windowsInformationUtils.UpdateProgressBar("Analysing hotfixes");
            AnalyseHotfixes();

            m_windowsInformationUtils.UpdateProgressBar("Analysing License status");
            AnalyseLicenseStatus();

        }


        private void AnalyseInitialInformation()
        {
            //https://github.com/security-cheatsheet/cmd-command-cheat-sheet

            AnalyseInitialInformationUtils analyseWindowsInformationUtils = new AnalyseInitialInformationUtils(m_configurationFilesFolder, m_analysisOutputFolder);

            analyseWindowsInformationUtils.AnalyseWindowsVersion();

            //We generate this function to extract the TCP listening ports to support netwtork segmentation tests.
            analyseWindowsInformationUtils.AnalyseListeningPorts();

            analyseWindowsInformationUtils.AnalyseAntivirus();
        }

        private void AnalyseHotfixes()
        {
            string inputFileName = m_configurationFilesFolder + @"\security_updates.csv";
            string outputFilename = m_analysisOutputFolder + @"\security_updates_RESULT.txt";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);


            List<int> bulletins = new List<int>();
            string[] separator = new string[1] { @"," };

            //We obtain the list of Hotfixes and we ordered by number
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.Trim() == "")
                {
                    continue;
                }
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string bulletin = fields[5].ToUpper();
                if(!bulletin.Contains("KB"))
                {
                    continue;
                }
                bulletin=bulletin.Replace("KB", "").Trim();
                int bulletinId = int.Parse(bulletin);
                bulletins.Add(bulletinId);
            }
            if (bulletins.Count > 0)
            {
                bulletins.Sort();
                bulletins.Reverse();
                outputFile.WriteLine("Hotfixes ordered by age, more recent first:");
                foreach (int x in bulletins)
                {
                    outputFile.WriteLine(x);
                }
                outputFile.WriteLine();
                string firstBulletin = bulletins[0].ToString();
                outputFile.WriteLine(@"Check the details of the unknown patches at https://support.microsoft.com/help/KBID for example https://support.microsoft.com/help/"+ firstBulletin+" in case of KB"+ firstBulletin);
            }
            inputFile.Close(); 
            outputFile.Close();
        }

        private void AnalyseLicenseStatus()
        {
            string inputFileName = m_configurationFilesFolder + @"\LicenseStatus.txt";
            string outputFilename = m_analysisOutputFolder + @"\LicenseStatus_RESULT.txt";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string Description = "";
            string LicenseStatus = "";
            string Name = "";
            string PartialProductKey = "";

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.Trim() == "")
                {
                    continue;
                }
                if(line.StartsWith("Description ="))
                {
                    Description = line;
                    continue;
                }
                if (line.StartsWith("LicenseStatus ="))
                {
                    LicenseStatus = line;
                    continue;
                }
                if (line.StartsWith("Name ="))
                {
                    Name = line;
                    continue;
                }
                if (line.StartsWith("PartialProductKey ="))
                {
                    PartialProductKey = line;
                    if(LicenseStatus.StartsWith("LicenseStatus = 1"))
                    {
                        outputFile.WriteLine(Description);
                        outputFile.WriteLine(LicenseStatus);
                        outputFile.WriteLine(Name);
                        outputFile.WriteLine(PartialProductKey);
                        outputFile.WriteLine();
                    } 
                }
            }
            inputFile.Close();
            outputFile.Close();
        }



        private void AnalyseACLShares()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLWMIShare.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLWMIShare_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Net share:");
        }



        private void AnalyseACLService()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLService.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLService_RESULT.txt";
            string inputWMICservices = m_configurationFilesFolder + @"\wmic_service.CSV";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.StartsWith("Service start type:"))
                {

                    string serviceName=ServiceResultUtils.AnalyzeServiceType(inputFile, outputFile);
                    if (!(serviceName is null))
                    {
                        string serviceInformation = ServiceResultUtils.GetServiceInformation(inputWMICservices, serviceName);
                        ServiceResultUtils.AnalyzeUnsecureKey(inputFile, outputFile, serviceName, serviceInformation);
                        ServiceResultUtils.AnalyzeUnsecurePath(inputFile, outputFile, serviceName, serviceInformation);
                    }
                }
             }
            inputFile.Close();
            outputFile.Close();
        }



        private void AnalyzeACLWDFService()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLWDFService.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLWDFService_RESULT.txt";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                string serviceName = "";
                if (line.StartsWith("Registry ACL:"))
                {
                    serviceName = inputFile.ReadLine();
                    ServiceResultUtils.AnalyzeUnsecureKey(inputFile, outputFile,serviceName) ;
                }
                if (line.StartsWith("Image path ACL:"))
                {
                    serviceName = inputFile.ReadLine();
                    ServiceResultUtils.AnalyzeUnsecurePath(inputFile, outputFile, serviceName);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        private void AnalyseACLUnquotedService()
        {
            string inputFileName = m_configurationFilesFolder + @"\service_unquoted_accesses.txt";
            string outputFilename = m_analysisOutputFolder + @"\service_unquoted_accesses_RESULT.txt";
            //Remember that it is possible that this function detects unquoted service path in "stopped" services. In this situation, it may happen that users have permissions on service.
            //You may detect through service_or_device_security_rules.txt. It obtains the users able to start the service.
            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Partial path of possible unquoted service path:");
        }

        private void AnalyseACLUnquotedSearchPath()
        {
            string inputFileName = m_configurationFilesFolder + @"\unquoted_ScheduledTasks.txt";
            string outputFilename = m_analysisOutputFolder + @"\unquoted_ScheduledTasks_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Possible unquoted search path in scheduled task:");
        }

        


        private void AnalyseACLPreviousPrograms(ProfileAccountUtils profileAccountUtils)
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLPreviousPrograms.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLPreviousPrograms_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Previous program execution:", profileAccountUtils);
        }

        private void AnalyseServiceRights()
        {

            string inputFileName = m_configurationFilesFolder + @"\service_executablePath_accesses.txt";
            string outputFilename = m_analysisOutputFolder + @"\service_executablePath_accesses_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Service:");

        }


        private void AnalyseServiceSecurityRulesControl()
        {
            string inputFileName = m_configurationFilesFolder + @"\service_or_device_security_rules.txt";
            string outputFilename = m_analysisOutputFolder + @"\service_or_device_security_rules_RESULT.txt";

            string inputWritableServicePathsFileName = m_analysisOutputFolder + @"\ACLService_RESULT.txt";
            List<string> writableServicePaths = ServiceResultUtils.GetWritableServicePaths(inputWritableServicePathsFileName);

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
                if (line.StartsWith("Security of service or driver:"))
                {
                    objectToReview = line;
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    Permission.ProcessServicePermission(line, inputFile, outputFile, objectToReview, writableServicePaths);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        private void AnalyseACLPath()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLPath.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLPath_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Folder path ACL:");


        }

        private List<string> GetUsersExecutingProcess(string process)
        {
            //A process may be executed by different users. A program may be running by different users at same time.
            List<string> userNames = new List<string>();
            string inputFileName = m_configurationFilesFolder + @"\tasklist.csv";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                //So we are not affected by commmas other than to separate the fields.
                line = line.Replace(@""",""", @"""_||_""");

                string[] separator = new string[1] { @"_||_" };
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string processLine = fields[0].Replace(@"""","");
                string userName = fields[6].Replace(@"""", "");


                if (processLine.ToUpper()==process.ToUpper())
                {
                    //A same process may be run by different users
                    if(!userNames.Contains(userName))
                    {
                        userNames.Add(userName);
                    }
                }
            }
            inputFile.Close();
            return userNames;
        }

        private void AnalyseACLProcesses()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLProcesses.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLProcesses_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            List<string> usersExecuting = null;
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Process ACL"))
                {
                    line= inputFile.ReadLine().Trim();
                    string process = line.Replace("NAME: ", "");
                    int position = process.IndexOf(",");
                    process = process.Substring(0, position);
                    usersExecuting = GetUsersExecutingProcess(process);
                    objectToReview = line + "; Executed by users: ";
                    foreach(string user in usersExecuting)
                    {
                        objectToReview += user + " ";
                    }
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    string user = line.Replace("User: ", "").Trim().ToUpper();
                    //We do not consider a risky situation when a process that privileges for a concrete user and it is executed by such user.
                    if(usersExecuting.Count==1 && user== usersExecuting[0].ToUpper())
                    {
                        continue;
                    }
                    Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        private void AnalyseACLScheduledTasks()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLScheduledTasks.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLScheduledTasks_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Scheduled task:");

            inputFileName = m_analysisOutputFolder + @"\ACLScheduledTasks_RESULT.txt";
            string taskPrincipalsFileName = m_configurationFilesFolder + @"\RegistrationInformationScheduledTasks.csv";
            outputFilename = m_analysisOutputFolder + @"\ACLScheduledTasks_RunHighest_RESULT.txt";

            ScheduledTaskUtils.ProcessScheduledTasksRunHighest(inputFileName, outputFilename, taskPrincipalsFileName, "Scheduled task:");
        }


        private void AnalyseACLStartUpTasks(ProfileAccountUtils profileAccountUtils)
        {

            WindowsStartUpUtils windowsStartUpUtils = new WindowsStartUpUtils(m_configurationFilesFolder, m_analysisOutputFolder);

            windowsStartUpUtils.AnalyseACLCommandStartUpTasks();

            windowsStartUpUtils.AnalyseACLLocalMachineStartUpKey();
            
            ////We search unprotected startup folder for all users in local machine 
            windowsStartUpUtils.AnalyseACLUsersStartUpFolder(profileAccountUtils);

            ////We obtain the ACL on all users folder and HKLM startup key.
            windowsStartUpUtils.AnalyseACLAllUsersStartUp();

        }

        private void AnalyseACLDrivers()
        {
            DriverUtils driverUtils = new DriverUtils(m_configurationFilesFolder, m_analysisOutputFolder);
            driverUtils.AnalyseACLDriverFolders();
            driverUtils.AnalyseACLDriverFiles();
        }


        private void AnalyseACLDrives()
        {
            string inputFileName = m_configurationFilesFolder + @"\ACLDrives.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLDrives_RESULT.txt";

            Permission.ProcessPathsPermission(inputFileName, outputFilename, "Drive ACL:");

        }


        private void AnalyseArgumentProcesses()
        {
            string inputFileName = m_configurationFilesFolder + @"\ArgumentProcesses.txt";
            string outputFilename = m_analysisOutputFolder + @"\ArgumentProcesses_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            //https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/windows-commands
            List<string> passwordArguments = new List<string> { " pass", " user", " -u", "net use", "ssh ", "ftp ", "telnet ", "putty ", "cmdkey" , @" /u" , "ktpass" , "passwd", "password", @" /up", @" /cup" , "key"};
            //Command where /p usually includes a password. We generate different list to prevent false positives if we only obtain arguments with /p
            List<string> commandPasswordArguments = new List<string> { "driverquery", "gpresult", "schtasks", "driverquery", "systeminfo", "tasklist", "taskkill", "wmic" };

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                foreach(string passwordArg in passwordArguments)
                {
                    if(line.ToUpper().Contains(passwordArg.ToUpper()))
                    {
                        outputFile.WriteLine("Found "+ passwordArg+": "+ line);
                        break;
                    }
                }
                foreach (string passwordArg in commandPasswordArguments)
                {
                    if (line.ToUpper().Contains(@" /p".ToUpper()) &&  line.ToUpper().Contains(passwordArg.ToUpper()))
                    {
                        outputFile.WriteLine("Found " + passwordArg + ": " + line);
                        break;
                    }
                }
            }
            inputFile.Close();
            outputFile.Close();
        }


        private void AnalyseLocalUserInformation()
        {
            string inputFileName = m_configurationFilesFolder + @"\localUsers.csv";
            string outputFilename = m_analysisOutputFolder + @"\localUsers_RESULT.txt";

            LocalUserUtils.AnalyseLocalUserInformation(inputFileName, outputFilename);
        }



        private void AnalyseUsersFromLocalAdministrators()
        {
            LocalUserUtils localUserUtils = new LocalUserUtils(m_configurationFilesFolder, m_analysisOutputFolder);

            //We generate and close domainAdministratorGroup_RESULT.csv with domain account information. We remind that we obtain this information because it is slow process for the extraction part.

            //This function returns true or false if it has been able to connect to LDAP server. This function does not apply for home PCs


            if (!localUserUtils.AnalyseDomainAccountsLocalAdministrators(WinHardenConfiguration.Configuration.ServerDomainConfiguration.ConnectedServer, WinHardenConfiguration.Configuration.ServerDomainConfiguration.ObtainGroups))
            {
                return;
            }

            string inputFileName = localUserUtils.ExtractionBaseFolder + @"\azureAdministratorGroup.csv";
            string[] inputLines = System.IO.File.ReadAllLines(inputFileName);


            //Now, we merge localAdministratorGroup.csv, domainAdministratorGroup_RESULT.csv and azureAdministratorGroup_RESULT.csv to generate a document with all the inforamtion related to local administrators, including domain, machine and Azure accounts
            localUserUtils.MergeTotalAdministrators();

            //Lastly we analyse the fields of the users to find any possible gaps on them
            string inputTotalFilename = m_analysisOutputFolder + @"\totalAdministratorGroup_RESULT.csv";
            string outputFilename = m_analysisOutputFolder + @"\totalAdministratorGroup_RESULT_GAPS.txt";
            LocalUserUtils.AnalyseLocalUserInformation(inputTotalFilename, outputFilename);           
        }

        private void AnalyseRelevantRegistryKeys()
        {
            string inputFileName = m_configurationFilesFolder + @"\WinLogonRegistryKeys.txt";
            string outputFilename = m_analysisOutputFolder + @"\registryKeys_RESULT.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);



            //We read thre registry key
            string registryKey = inputFile.ReadLine();

            string defaultUserName = "";
            string defaultDomainName = "";
            string defaultPassword = "";
            string autoAdminLogon = "";


            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                if (line.Contains("DefaultUserName"))
                {
                    defaultUserName = line;
                }
                if (line.Contains("DefaultDomainName"))
                {
                    defaultDomainName = line;
                }
                if (line.Contains("DefaultPassword"))
                {
                    defaultPassword = line;
                }
                if (line.Contains("AutoAdminLogon"))
                {
                    autoAdminLogon = line;
                }
            }
            if (defaultPassword != "")
            {
                outputFile.WriteLine(registryKey);
                outputFile.WriteLine("Password related to auto admin logon has been identified:");
                outputFile.WriteLine(autoAdminLogon);
                outputFile.WriteLine(defaultDomainName);
                outputFile.WriteLine(defaultUserName);
                outputFile.WriteLine(defaultPassword);
            }
            inputFile.Close();
            outputFile.Close();



        }
    }
}
