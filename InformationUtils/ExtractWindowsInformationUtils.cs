using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
//using static System.Net.WebRequestMethods;


namespace InformationUtils
{
    public class ExtractWindowsInformationUtils
    {
        private string m_extractionBaseFolder;
        WindowsInformationUtils m_windowsInformationUtils;

        //This variable includes the number of UpdateProgressBar calls to show a progress bar.
        private const int s_extractMaximumProgressBar = 63;



        public static void ExtractPermissionsAndAuditConfiguration(string outputFolder, string inputFileName)
        {
            string outputPermissionsFilename = outputFolder + @"\permissions_from_input_file.txt";
            string outputAuditFilename = outputFolder + @"\audit_from_input_file.txt";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            StreamWriter outputPermissionsFile = new StreamWriter(outputPermissionsFilename, false, Encoding.Default);
            StreamWriter outputAuditFile = new StreamWriter(outputAuditFilename, false, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.Trim() == "")
                {
                    continue;
                }
                if(!Directory.Exists(line))
                {
                    outputPermissionsFile.WriteLine("Folder does not exist: " + line);
                    outputAuditFile.WriteLine("Folder does not exist: " + line);
                    continue;
                }

                ACLUtils.WritePermissionRecursiveFolder(outputPermissionsFile, line);
                AuditUtils.WriteAuditConfigurationRecursiveFolder(outputAuditFile, line);

            }
            inputFile.Close();
            outputPermissionsFile.Close();
            outputAuditFile.Close();

        }

        public ExtractWindowsInformationUtils(string extractionBaseFolder, WindowsInformationUtils windowsInformationUtils)
        {
            m_extractionBaseFolder = extractionBaseFolder;
            m_windowsInformationUtils = windowsInformationUtils;
        }

        public void ExtractWindowsInformation()
        {
            ExtractWindowsInformation(null);
        }

        public void ExtractWindowsInformation(string registryPolicyFile)
        {
            LogUtils.LogFile = m_extractionBaseFolder + @"\log.txt";
            m_windowsInformationUtils.InitProgressBar(s_extractMaximumProgressBar);

            m_windowsInformationUtils.UpdateProgressBar("Extracting current regional configuration");
            GetCurrentRegionalConfiguration();

            m_windowsInformationUtils.UpdateProgressBar("Extracting initial information");
            GetInitialInformation();

            m_windowsInformationUtils.UpdateProgressBar("Extracting logons from event viewer");
            GetLogons();

            //Getting ACL permissions on Shares requires administrator privileges.
            m_windowsInformationUtils.UpdateProgressBar("Getting ACL shares information");
            GetACLShares();

            m_windowsInformationUtils.UpdateProgressBar("Getting services information");
            GetServicesInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting previously executed programs");
            GetPreviouslyExecutedPrograms();

            //This function obtains the security of the environmental variable path. This analysis may be useful to identify vulnerable binaries to DLL hijacking.
            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of path folders information");
            GetACLPath();

            //This function obtains the security of the running processes. This analysis may be useful to identify vulnerable binaries to binary planting.
            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of running processes information");
            GetACLProcesses();

            m_windowsInformationUtils.UpdateProgressBar("Getting scheduled tasks information");
            GetScheduledTasksInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of start up tasks information");
            GetACLStartUpTasks();

            //This funcion obtains the security of the folders containing Windows drivers, which are executed under system/kernel privileges.
            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of drivers information");
            GetACLDrivers();

            //This function obtains the security of the drives.
            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of drives information");
            GetACLDrives();

            m_windowsInformationUtils.UpdateProgressBar("Getting ACL of previously executed programs");
            GetACLPreviouslyExecutedPrograms();

            //This function is useful to retrieve all the arguments in the running processes, due to sometimes user and passwords are included as parameters.
            m_windowsInformationUtils.UpdateProgressBar("Getting arguments of running processes information");
            GetArgumentProcesses();

            //These funcions are useful to identify on going connections in the server. If it is a production server, remaining connections should be done from other production servers or jumphost
            m_windowsInformationUtils.UpdateProgressBar("Getting sessions");
            GetSessions();

            m_windowsInformationUtils.UpdateProgressBar("Getting terminal server sessions");
            GetTerminalServerConnections();

            //This function downloads the google.com web page. So, we verify in a practical web if it is a production server connected to Internet.
            m_windowsInformationUtils.UpdateProgressBar("Testing internet connection");
            DownloadGoogleCom();

            m_windowsInformationUtils.UpdateProgressBar("Getting active directory information");
            GetActiveDirectoryInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting local users information");
            GetLocalUserInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting local group information");
            GetLocalGroupInformation();

            m_windowsInformationUtils.UpdateProgressBar("Getting information from relevant local groups, as local administrators and remote desktop users");
            GetMembersFromRelevantLocalGroups();

            //This function requires administrator privileges
            m_windowsInformationUtils.UpdateProgressBar("Getting local user privileges");
            GetLocalUsersPrivileges();

            //This function requires administrator privileges
            m_windowsInformationUtils.UpdateProgressBar("Getting privileges of each user");
            GetLocalUserDetailedPrivileges();

            m_windowsInformationUtils.UpdateProgressBar("Getting relevant registry keys");
            GetRelevantRegistryKeys(registryPolicyFile);

            //Getting ACL permissions on audit policies requires administrator privileges.
            m_windowsInformationUtils.UpdateProgressBar("Getting audit policies");
            GetAuditPolicies();

            m_windowsInformationUtils.UpdateProgressBar("Extract Windows License status");
            GetWindowsLicenseStatus();
        }




        public  static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void ProcessComandWithProgress(string command, string outputFile, string message)
        {
            m_windowsInformationUtils.UpdateProgressBar(message);
            ProcessCommand(command + @""""+ outputFile+@"""");
        }


        private void GetCurrentRegionalConfiguration()
        {
            //We extract date time pattern to easy date time conversion when we analyse files with dates
            try
            { 
                CultureInfo culture = CultureInfo.CurrentCulture;
                string regionalConfigurationFile = m_extractionBaseFolder + @"\regionalconfiguration.txt";
                File.WriteAllText(regionalConfigurationFile, culture.Name);
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
}


        private void GetInitialInformation()
        {
            try
            {

                //https://github.com/security-cheatsheet/cmd-command-cheat-sheet


                ProcessComandWithProgress("net accounts > ", m_extractionBaseFolder + @"\password_policy.txt", "Getting net accounts");
                //Set environmental variables
                ProcessComandWithProgress("set >", m_extractionBaseFolder + @"\set.txt", "Getting set");
                //Windows version
                ProcessComandWithProgress("ver >", m_extractionBaseFolder + @"\ver.txt", "Getting ver");


                ProcessComandWithProgress(@"ipconfig /all > ", m_extractionBaseFolder + @"\ipconfig.txt", "Getting ipconfig");
                ProcessComandWithProgress("netstat -an > ", m_extractionBaseFolder + @"\netstat.txt", "Getting netstat");

                //This command requires administrator privileges
                ProcessComandWithProgress("netstat -anb > ", m_extractionBaseFolder + @"\netstat_processes.txt", "Getting netstat -anb");

                ProcessComandWithProgress("hostname > ", m_extractionBaseFolder + @"\hostname.txt", "Getting hostname");

                //This scrtip shows vulnerabilities from systeminfo. 
                //https://github.com/bitsadmin/wesng
                ProcessComandWithProgress("systeminfo > ", m_extractionBaseFolder + @"\systeminfo.txt", "Getting systeminfo");


                ProcessComandWithProgress("arp -a > ", m_extractionBaseFolder + @"\arp.txt", "Getting arp");
                ProcessComandWithProgress("cmdkey /list > ", m_extractionBaseFolder + @"\cmdkey.txt", "Getting cmdkey");

                // This command creates a report that displays what group policies objects are applied to a user and computer.
                ProcessComandWithProgress("gpresult /v > ", m_extractionBaseFolder + @"\gpresult.txt", "Getting gpresult");
                ProcessComandWithProgress("route print > ", m_extractionBaseFolder + @"\route.txt", "Getting route print");
                ProcessComandWithProgress("net use > ", m_extractionBaseFolder + @"\netuse.txt", "Getting net use");
                ProcessComandWithProgress("powershell Get-ExecutionPolicy > ", m_extractionBaseFolder + @"\executionPolicy.txt", "Getting powershell");

                ProcessComandWithProgress("tasklist /v /FO CSV > ", m_extractionBaseFolder + @"\tasklist.csv", "Getting tasklist");
                ProcessComandWithProgress("wmic process list FULL /FORMAT:table > ", m_extractionBaseFolder + @"\wmic_processes.txt", "Getting wmic process");

                ProcessComandWithProgress("wmic startup list /Format:CSV >", m_extractionBaseFolder + @"\startUpTasks.csv", "Getting wmic startup");
                ProcessComandWithProgress("schtasks /query /V /fo CSV > ", m_extractionBaseFolder + @"\scheduledtasks.csv", "Getting schtasks");


                ProcessComandWithProgress("sc.exe queryex > ", m_extractionBaseFolder + @"\sc_queryex.txt", "Getting sc");

                //https://book.hacktricks.xyz/windows/windows-local-privilege-escalation#modify-service-binary-path
                //It is relevant search unquoated services in PathName parameter
                //PathName = "C:\Program Files (x86)\Common Files\xxxxxxx.exe"
                ProcessComandWithProgress("wmic service get DisplayName, Name, PAthNAme,Started,StartMode,StartName,State,Status /Format:CSV > ", m_extractionBaseFolder + @"\wmic_service.CSV", "Getting wmic service");

                ProcessComandWithProgress("driverquery /FO CSV /V > ", m_extractionBaseFolder + @"\drivers.csv", "Getting drivers");

                ProcessComandWithProgress("net user > ", m_extractionBaseFolder + @"\local_users.txt", "Getting local users");

                ProcessComandWithProgress("wmic qfe list /Format:CSV > ", m_extractionBaseFolder + @"\security_updates.csv", "Getting wmic qfe");

                //Comand below does not apply for version of Windows Server due to they do not have Security Center.
                //https://github.com/fusioninventory/fusioninventory-agent/issues/701
                //Link below explain the product state value
                //https://ourcodeworld.com/articles/read/878/how-to-identify-detect-and-name-the-antivirus-software-installed-on-the-pc-with-c-on-winforms
                ProcessComandWithProgress(@"WMIC /Node:localhost /Namespace:\\root\SecurityCenter2 Path AntiVirusProduct Get /Format:List > ", m_extractionBaseFolder + @"\antivirus.txt", "Getting antivirus product");

                ProcessComandWithProgress(@"wmic product get name,version > ", m_extractionBaseFolder + @"\InstalledSoftwareList.txt", "Getting installed software");

                //https://docs.microsoft.com/en-us/troubleshoot/windows-client/performance/determine-hardware-dep-available
                ProcessComandWithProgress(@"wmic OS Get DataExecutionPrevention_SupportPolicy > ", m_extractionBaseFolder + @"\DEPpolicy.txt", "Getting wmic OS");

                //https://dirteam.com/sander/2009/09/23/how-to-tell-whether-it-s-a-server-core-domain-controller/
                ProcessComandWithProgress(@"wmic computersystem get DomainRole > ", m_extractionBaseFolder + @"\DomainRole.txt", "Getting domain role");


                //This command requires administration privileges to be run.
                ProcessComandWithProgress(@"auditpol.exe /get /category:* > ", m_extractionBaseFolder + @"\auditpol.txt", "Getting auditpol");
                //https://docs.microsoft.com/en-us/windows-server/identity/ad-ds/plan/security-best-practices/audit-policy-recommendations
                //Link explaining how auditing in file shares works.
                //https://learn.microsoft.com/en-us/windows/security/threat-protection/auditing/audit-detailed-file-share


                //This command requires administration privileges to be run.
                //https://stackoverflow.com/questions/11607389/how-to-view-user-privileges-using-windows-cmd
                //https://stackoverflow.com/questions/8310485/get-system-minimum-password-length-and-complexity
                ProcessComandWithProgress(@"secedit.exe /export /cfg ", m_extractionBaseFolder + @"\secedit.cfg", "Getting secedit");


                //It is possible that secedit is not generated from function above, due it usually requires administration privileges
                if (File.Exists(m_extractionBaseFolder + @"\secedit.cfg"))
                {
                    string[] lines = File.ReadAllLines(m_extractionBaseFolder + @"\secedit.cfg");
                    string[] replacedLines = SIDUtils.TranslateSIDsInLines(lines);
                    File.WriteAllLines(m_extractionBaseFolder + @"\secedit_translated_SID.cfg", replacedLines);
                }
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetACLPath()
        {
            try
            { 
                string outputFilename = m_extractionBaseFolder + @"\ACLPath.txt";
                using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
                {
                    string path = Environment.GetEnvironmentVariable("path");
                    //Separator for path environment variable is always ;
                    string[] separator = new string[1] { ";" };
                    string[] pathList = path.Split(separator, StringSplitOptions.None);

                    foreach (string pathFolder in pathList)
                    {
                        if(pathFolder.Trim()=="")
                        {
                            continue;
                        }
                        if (Directory.Exists(pathFolder))
                        {
                            outputFile.WriteLine("Folder path ACL:");
                            outputFile.WriteLine(pathFolder);
                            ACLUtils.ShowSecurityDirectory(pathFolder, outputFile);
                        }
                        else
                        {
                            outputFile.WriteLine("Folder path not found:");
                            outputFile.WriteLine(pathFolder);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetACLDrivers()
        {
            try
            {
                DriverUtils driverUtils = new DriverUtils(m_extractionBaseFolder);

                driverUtils.GetACLDriverFolders();
                driverUtils.GetACLDriverFiles();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }


        private void GetServicesInformation()
        {
            try
            {
                ServiceUtils serviceUtils = new ServiceUtils(m_extractionBaseFolder);


                //This function analyses the security of the services. It obtains the access control of the refistry of the keys, access control of the paths containing the services and their status.
                //So, auditor will be able to reivew if binary planting in services is possible: through modification in the registry or modification in the path of the binary.
                m_windowsInformationUtils.UpdateProgressBar("Getting ACL services information");
                serviceUtils.GetACLService();

                //WDF services are managed in a different key
                m_windowsInformationUtils.UpdateProgressBar("Getting ACL WDF services information");
                serviceUtils.GetACLWDFService();

                ////https://book.hacktricks.xyz/windows/windows-local-privilege-escalation#modify-service-binary-path
                ////It is relevant search unquoated services in PathName parameter
                ////PathName = "C:\Program Files (x86)\Common Files\xxxxxxx.exe"
                ///https://medium.com/@SumitVerma101/windows-privilege-escalation-part-1-unquoted-service-path-c7a011a8d8ae
                m_windowsInformationUtils.UpdateProgressBar("Getting ACL unquoted services information");
                serviceUtils.GetACLUnquotedService();

                m_windowsInformationUtils.UpdateProgressBar("Getting Service rights information");
                serviceUtils.GetServiceRights();

                m_windowsInformationUtils.UpdateProgressBar("Getting Service security rules information");
                serviceUtils.GetServiceSecurityRulesControl();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }

        private void GetPreviouslyExecutedPrograms()
        {
            try
            {
                //We obtain the registiry key where previous executed programs are listed
                //https://www.nirsoft.net/utils/executed_programs_list.html

                //HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache
                //HKEY_CURRENT_USER\Software\Microsoft\Windows\ShellNoRoam\MUICache
                //HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted
                //HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store

                string outputFilename = m_extractionBaseFolder + @"\PreviousPrograms.txt";
                RegistryUtils.WriteHKCUKeyValuesToFile(outputFilename, @"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache",false,RegistryFormatType.OnlyName);
                RegistryUtils.WriteHKCUKeyValuesToFile(outputFilename, @"Software\Microsoft\Windows\ShellNoRoam\MUICache", true, RegistryFormatType.OnlyName);
                RegistryUtils.WriteHKCUKeyValuesToFile(outputFilename, @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted", true, RegistryFormatType.OnlyName);
                RegistryUtils.WriteHKCUKeyValuesToFile(outputFilename, @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store", true, RegistryFormatType.OnlyName);


            }
            catch(Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }


        private void GetACLProcesses()
        {
            try
            {
                string inputFileName = m_extractionBaseFolder + @"\wmic_processes.txt";
                string outputFilename = m_extractionBaseFolder + @"\ACLProcesses.txt";
                FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                List<string> processes = new List<string>();

                string header = inputFile.ReadLine();
                ProcessesUtils processesUtils = ProcessesUtils.GetProcessesUtils(header);
                while (!inputFile.EndOfStream)
                {
                    string line = inputFile.ReadLine();
                    string name = processesUtils.GetName(line);
                    string executablePath = processesUtils.GetExecutablePath(line);
                    if (executablePath.Trim() == "")
                    {
                        continue;
                    }

                    //We prevent inserting duplicated paths
                    if (!processes.Contains(executablePath))
                    {
                        outputFile.WriteLine("Process ACL");
                        outputFile.WriteLine("NAME: " + name.Trim() + ", PATH: " + executablePath);
                        ACLUtils.ShowSecurityFile(executablePath, outputFile);
                        outputFile.WriteLine();
                        outputFile.WriteLine();

                        processes.Add(executablePath);
                    }


                }

                inputFile.Close();
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }


        }
        private void GetScheduledTasksInformation()
        {
            try
            {
                ScheduledTaskUtils scheduledTaskUtils = new ScheduledTaskUtils(m_extractionBaseFolder);

                m_windowsInformationUtils.UpdateProgressBar("Getting Scheduled Tasks registration information");
                //This function may run only under administrator privileges due to it reads from C:\Windows\System32\Tasks
                scheduledTaskUtils.GetRegsitrationInformationScheduledTasks();
                
                m_windowsInformationUtils.UpdateProgressBar("Getting ACL services information");
                scheduledTaskUtils.GetACLScheduledTasks();

                //This issue should be fixed acording to CVE below. But we try to find scheduled tasks with path containing environmental variable,
                //due to it is possible that quotes are not managed correctly
                //https://bishopfox.com/blog/windows-task-scheduler-19044-advisory
                m_windowsInformationUtils.UpdateProgressBar("Getting unquoted search path of scheduled tasks.");
                scheduledTaskUtils.GetUnquotedSearchPath();

            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }







        private void GetACLStartUpTasks()
        {
            try
            {
                WindowsStartUpUtils windowsStartUpUtils = new WindowsStartUpUtils(m_extractionBaseFolder);

                windowsStartUpUtils.GetACLCommandStartUpTasks();

                windowsStartUpUtils.GetACLLocalMachineStartUpKey();
                //We search unprotected startup folder for all users in local machine 
                windowsStartUpUtils.GetACLUsersStartUpFolder();

                //We obtain the ACL on all users folder and HKLM startup key.
                windowsStartUpUtils.GetACLAllUsersStartUp();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }

        private void GetACLDrives()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\ACLDrives.txt";
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    //We only review local drives
                    if (d.DriveType == DriveType.Fixed)
                    {
                        outputFile.WriteLine("Drive ACL:");
                        outputFile.WriteLine(d.Name);
                        ACLUtils.ShowSecurityDirectory(d.Name, outputFile);
                    }

                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }
        private void GetACLPreviouslyExecutedPrograms()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\ACLPreviousPrograms.txt";
                string inputFileName = m_extractionBaseFolder + @"\PreviousPrograms.txt";
                FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                while (!inputFile.EndOfStream)
                {
                    string line = inputFile.ReadLine();
                    line=line.Replace(".ApplicationCompany", "");
                    line = line.Replace(".FriendlyAppName", "");
                    
                    //We ignore lines that are not related to programs
                    if (File.Exists(line))  
                    {
                        outputFile.WriteLine("Previous program execution:");
                        outputFile.WriteLine(line);
                        ACLUtils.ShowSecurityFile(line, outputFile);

                        outputFile.WriteLine();
                        outputFile.WriteLine();
                    }
                }
                inputFile.Close();
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }



        private void GetArgumentProcesses()
        {
            try
            {
                string inputFileName = m_extractionBaseFolder + @"\wmic_processes.txt";
                string outputFilename = m_extractionBaseFolder + @"\ArgumentProcesses.txt";
                FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                string header = inputFile.ReadLine();
                ProcessesUtils processesUtils = ProcessesUtils.GetProcessesUtils(header);
                while (!inputFile.EndOfStream)
                {
                    string line = inputFile.ReadLine();
                    string commandLine = processesUtils.GetCommandLine(line);
                    if (commandLine.Trim() == "")
                    {
                        continue;
                    }
                    outputFile.WriteLine(commandLine);
                }
                inputFile.Close();
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetSessions()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\Sessions.txt";
                using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
                {
                    string sessions = WindowsSessionUtils.EnumSessionsToString("localhost");
                    outputFile.WriteLine(sessions);
                }
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }
        internal void GetTerminalServerConnections()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\TerminalServerConnections.txt";
                using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
                {
                    string sessions = TerminalSessionUtils.GetTerminalServicesSessions("localhost");
                    outputFile.WriteLine(sessions);
                }
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }

        internal void GetWindowsLicenseStatus()
        {
            try
            {
                LicenseUtils licenseUtils = new LicenseUtils(m_extractionBaseFolder);

                licenseUtils.GetLicenseStatus();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void DownloadGoogleCom()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\googlecom.txt";
                try
                {
                    ICredentials credentials = CredentialCache.DefaultCredentials;
                    System.Net.IWebProxy proxy = WebRequest.GetSystemWebProxy();
                    proxy.Credentials = credentials;
                    WebClient webClient = new WebClient();
                    webClient.Proxy = proxy;
                    webClient.DownloadFile(@"https://www.google.com", outputFilename);
                }
                catch (Exception e)
                {
                    using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
                    {
                        outputFile.WriteLine(e.Message);
                        outputFile.WriteLine();
                        outputFile.WriteLine(e.Source);
                        outputFile.WriteLine();
                        outputFile.WriteLine(e.StackTrace);
                    }
                }
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }

        private void GetLocalGroupInformation()
        {
            try
            {
                GetLocalGroupInformationByNetCommand("localGroups.txt");
                GetLocalGroupInformationByAPI("localGroups.csv");
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }

        private void GetLocalGroupInformationByNetCommand(string file)
        {
            string outputFilename = m_extractionBaseFolder + @"\" + file;
            if (File.Exists(outputFilename))
            {
                File.Delete(outputFilename);
            }
            //We obtain the list of local groups
            PrincipalContext context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
            PrincipalSearcher searcher = new PrincipalSearcher(new GroupPrincipal(context));
            foreach (Principal group in searcher.FindAll())
            {
                ProcessCommand(@"net localgroup """ + group.Name + @""" >> " + @"""" + outputFilename + @"""");
            }
        }
        private void GetLocalGroupInformationByAPI(string file)
        {
            string outputFilename = m_extractionBaseFolder + @"\" + file;
            StreamWriter localOutputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            
            localOutputFile.WriteLine("Group;Name;Type;IsLocal;IsEnabled");

            //We obtain the list of local groups
            PrincipalContext context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
            PrincipalSearcher searcher = new PrincipalSearcher(new GroupPrincipal(context));
            foreach (Principal group in searcher.FindAll())
            {
                GroupPrincipal groupPrincipal = group as GroupPrincipal;
                foreach (Principal principal in groupPrincipal.Members)
                {
                    string groupName = group.Name;
                    string name = principal.Name;
                    string type = "";
                    string isLocal = "";
                    string isEnabled = "";

                    if (principal.Sid.Value.Trim().ToUpper().StartsWith("S-1-12"))
                    {
                        //SID starting with S-1-12 are Azure objects. We will obtain the users in the Analyzer program connecting to Azure Active Directory
                        name = principal.Sid.Value;
                        type = "Azure";
                        isLocal = "FALSE";
                        isEnabled = "";
                        localOutputFile.WriteLine(group + ";" + name + ";" + type + ";" + isLocal + ";" + isEnabled);
                        continue;
                    }

                    //It is possible that removed users belong to grops. In this case, princiapl is added in the list, with a SID value, but remaining fields to null.
                    //We ignore them
                    if (principal.Name == null)
                    {
                        continue;
                    }
                    //We obtain full details for local accounts. They are usually a short number, so doing this function during extracting should not delay the execution.



                    //Local user
                    if (principal.StructuralObjectClass is null)
                    {
                        UserPrincipal userPrincipal = principal as UserPrincipal;
                        isLocal = "TRUE";
                        type = "USER";
                        if (userPrincipal != null)
                        {
                            isEnabled = userPrincipal.Enabled.ToString();
                        }
                    }
                    // Domain user
                    if (principal.StructuralObjectClass == "user" || principal.StructuralObjectClass == "msDS-GroupManagedServiceAccount")
                    {
                        UserPrincipal userPrincipal = principal as UserPrincipal;
                        isLocal = "FALSE";
                        type = "USER";
                        isEnabled = userPrincipal.Enabled.ToString();
                    }
                    //Group. Domain or local according to ContextType
                    if (principal.StructuralObjectClass == "group")
                    {
                        type = "GROUP";
                    }

                    if (principal.ContextType == ContextType.Machine)
                    {
                        isLocal = "TRUE";
                    }

                    //Here, we obtain the quick information of the object to prevent connecting to domain. So, this function is fast during extracting data.
                    //We manage as principial due to both user and groups classes are pincipal.
                    if (principal.ContextType == ContextType.Domain)
                    {
                        isLocal = "FALSE";
                    }
                    if (principal.ContextType == ContextType.ApplicationDirectory)
                    {
                        localOutputFile.Close();
                        throw new Exception("Unprocessed Application Directory as context type.");
                    }

                    localOutputFile.WriteLine(group + ";" + name + ";" + type + ";" + isLocal + ";" + isEnabled);

                }
            }

            localOutputFile.Close();
        }


        private void GetActiveDirectoryInformation()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\activeDirectory.txt";


                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
                string connectedServer = ActiveDirectoryUtils.GetDefaultLDAPServer();
                if (connectedServer is null)
                {
                    connectedServer = "DefaultLDAPServer=None";
                }
                else
                {
                    connectedServer = "DefaultLDAPServer=" + connectedServer;
                }
                outputFile.WriteLine(connectedServer);
                string userDNSDomain = Environment.GetEnvironmentVariable("USERDNSDOMAIN");
                string userDomain = Environment.GetEnvironmentVariable("USERDOMAIN");
                outputFile.WriteLine("USERDNSDOMAIN=" + userDNSDomain);
                outputFile.WriteLine("USERDOMAIN=" + userDomain);
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetLocalUserInformation()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\localUsers.csv";


                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
                outputFile.WriteLine(LocalUserUtils.Header);
                //We obtain the list of local users

                PrincipalContext context = new PrincipalContext(ContextType.Machine);
                PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context));

                foreach (Principal userPrincipal in searcher.FindAll())
                {
                    UserPrincipal user = userPrincipal as UserPrincipal;
                    string userInformation = LocalUserUtils.GetUserInformation(user, true);
                    outputFile.WriteLine(userInformation);
                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetLocalUserDetailedPrivileges()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\localUserDetailedPrivileges.csv";
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                if (!IsAdministrator())
                {
                    outputFile.WriteLine("ERROR: access not auhtorized. Program requires administrator privileges.");
                    outputFile.Close();
                    return;
                }
                string header =
                    "SamAccountName"
                    + "," + "Description"
                    + "," + "Name"
                    + "," + "Privileges";

                outputFile.WriteLine(header);
                //We obtain the list of local users

                PrincipalContext context = new PrincipalContext(ContextType.Machine);
                PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context));

                LsaWrapper lsaWrapper = new LsaWrapper();

                foreach (Principal userPrincipal in searcher.FindAll())
                {
                    UserPrincipal user= userPrincipal as UserPrincipal;

                    string description = "";
                    if (user.Description != null)
                    {
                        description = user.Description.Replace(',', '|');
                    }
                    string val = user.SamAccountName
                        + "," + description
                        + "," + user.Name;

                    string privileges = "";
                    Rights[] rights = lsaWrapper.EnumerateAccountPrivileges(user.Sid);
                    if (!(rights is null))
                    {
                        foreach (Rights privilege in rights)
                        {
                            privileges += @" \\ " + privilege.ToString();
                        }
                    }
                    outputFile.WriteLine(val + "," + privileges);
                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }



        private void GetMembersFromRelevantLocalGroups()
        {
            try
            {
                GetMembersFromSIDGroup(SIDUtils.s_Builtin_Administrators, "AdministratorGroup.csv");
                //Personal computers usually does not have Remote Desktop Users group... So, files would generated empty.
                GetMembersFromSIDGroup(SIDUtils.s_Builtin_RemoteDesktopUsers, "RemoteDesktopUsersGroup.csv");
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetLocalUsersPrivileges()
        {
            try
            {
                //This functions obtains the same information, with translated SIDs, as secedit.cfg

                //All privileges are explained in link below
                //https://docs.microsoft.com/en-us/windows/win32/secauthz/privilege-constants

                string outputFilename = m_extractionBaseFolder + @"\localUserPrivileges.txt";
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                if (!IsAdministrator())
                {
                    outputFile.WriteLine("ERROR: access not auhtorized. Program requires administrator privileges.");
                    outputFile.Close();
                    return;
                }

                LsaWrapper lsaWrapper = new LsaWrapper();
                foreach (Rights privilege in (Rights[])Enum.GetValues(typeof(Rights)))
                {
                    string[] userRights = lsaWrapper.EnumerateAccountsWithUserRight(privilege, true);
                    if (userRights is null)
                    {
                        outputFile.WriteLine("No users assigned to privilege: " + privilege.ToString());
                        outputFile.WriteLine();
                        continue;
                    }
                    outputFile.WriteLine("Privilege: " + privilege.ToString());
                    foreach (string userRight in userRights)
                    {
                        outputFile.WriteLine(userRight);
                    }
                    outputFile.WriteLine();
                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }


        private void GetLogons()
        {
            try
            {
                //It would include autologon paramaters as well
                string outputFilename = m_extractionBaseFolder + @"\LogonEvents.txt";
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                if (!IsAdministrator())
                {
                    outputFile.WriteLine("ERROR: access not auhtorized to retrieve events from log. Program requires administrator privileges.");
                    outputFile.Close();
                    return;
                }


                EventLog eventLog = new EventLog();
                eventLog.Log = "Security";
    
                foreach (EventLogEntry logEntry in eventLog.Entries)
                {
                    if(logEntry.InstanceId== 4624 || logEntry.InstanceId == 4672)
                    {
                        outputFile.WriteLine("EVENT CREATED AT "+ logEntry.TimeGenerated.ToString() + Environment.NewLine
                            + "ENTRY TYPE: "+ logEntry.EntryType.ToString() + Environment.NewLine
                            + "EVENT ID: " + logEntry.InstanceId.ToString() + Environment.NewLine
                            + "MESSAGE:" +Environment.NewLine
                            + logEntry.Message + Environment.NewLine);
                    }
                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }

        private void GetACLShares()
        {
            try
            {
                ProcessComandWithProgress("net share > ", m_extractionBaseFolder + @"\netshare.txt", "Getting net share");
                ProcessComandWithProgress("wmic share get AllowMaximum,Caption,Description,MaximumAllowed,Name,Path,Status,Type /format:csv > ", m_extractionBaseFolder + @"\netshare.csv", "Getting wmic share");
                ShareUtils shareUtils = new ShareUtils(m_extractionBaseFolder);
                Dictionary<string, string> sharesPath = shareUtils.GetACLNetShares("ACLNetShare.txt");
                shareUtils.GetACLWMIShares("ACLWMIShare.txt", sharesPath);
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }



        private void GetMembersFromSIDGroup(string sidValue,string nameFile)
        {

            //Full infomration regarding local accounts
            string localFilename = m_extractionBaseFolder + @"\local" + nameFile;

            //Short information regarding to domain accounts that included in local groups. Extended details will be obtained during analysis execution. 
            //This function is only used for local groups (Local Administrators and Remote Desktop). It is the reason that ContextType is directly Machine.
            string domainFilename = m_extractionBaseFolder + @"\domainLocal" + nameFile;

            string azureFilename = m_extractionBaseFolder + @"\azure" + nameFile;

            StreamWriter localOutputFile = new StreamWriter(localFilename, false, Encoding.Default);
            StreamWriter domainOutputFilename = new StreamWriter(domainFilename, false, Encoding.Default);
            StreamWriter azureOutputFilename = new StreamWriter(azureFilename, false, Encoding.Default);

            localOutputFile.WriteLine(LocalUserUtils.Header);
            domainOutputFilename.WriteLine(LocalUserUtils.PrincipalHeader);
            azureOutputFilename.WriteLine("SID");

            try
            {
                //This function is only used for local groups (Local Administrators and Remote Desktop). It is the reason that ContextType is directly Machine.
                List<Principal> principals = LocalUserUtils.GetMembersFromGroup(ContextType.Machine, IdentityType.Sid, sidValue, Environment.MachineName);


                foreach (Principal principal in principals)
                {
                    if (principal.Sid.Value.Trim().ToUpper().StartsWith("S-1-12"))
                    {
                        //SID starting with S-1-12 are Azure objects. We will obtain the users in the Analyzer program connecting to Azure Active Directory
                        azureOutputFilename.WriteLine(principal.Sid.Value);
                        continue;
                    }

                    //It is possible that removed users belong to grops. In this case, princiapl is added in the list, with a SID value, but remaining fields to null.
                    //We ignore them
                    if (principal.Name == null)
                    {
                        continue;
                    }
                    //We obtain full details for local accounts. They are usually a short number, so doing this function during extracting should not delay the execution.
                    if (principal.ContextType == ContextType.Machine)
                    {
                        string userInformation = LocalUserUtils.GetUserInformation((UserPrincipal)principal, true);
                        localOutputFile.WriteLine(userInformation);
                        continue;
                    }

                    //Here, we obtain the quick information of the object to prevent connecting to domain. So, this function is fast during extracting data.
                    //We manage as principial due to both user and groups classes are pincipal.
                    if (principal.ContextType == ContextType.Domain)
                    {
                        string userInformation = LocalUserUtils.GetPrincipalInformation(principal);
                        domainOutputFilename.WriteLine(userInformation);
                        continue;
                    }
                    if (principal.ContextType == ContextType.ApplicationDirectory)
                    {
                        localOutputFile.Close();
                        domainOutputFilename.Close();
                        azureOutputFilename.Close();
                        throw new Exception("Unprocessed Application Directory as context type.");
                    }
                }
            }
            catch (Exception e)
            {
                //It is possible that there is domain accounts and workstation is not connected to the network. In this case, we process this controlled exception
                localOutputFile.WriteLine(e.Message);
                domainOutputFilename.WriteLine(e.Message);
                azureOutputFilename.WriteLine(e.Message);
            }
            finally
            {
                localOutputFile.Close();
                domainOutputFilename.Close();
                azureOutputFilename.Close();
            }
        }

        private void GetRelevantRegistryKeys(string registryPolicyFile)
        {
            try
            {
                //It would include autologon paramaters as well
                string outputFilename = m_extractionBaseFolder + @"\WinLogonRegistryKeys.txt";
                RegistryUtils.WriteHKLMKeyValuesToFile(outputFilename, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",false,RegistryFormatType.NameAndKey);

                outputFilename = m_extractionBaseFolder + @"\InstalledPrograms.txt";
                RegistryUtils.WriteFullHKLMValuesToFile(outputFilename, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false, RegistryFormatType.NameAndKey);

                //This file is relevant. Even it includes UAC configuration.
                //https://docs.microsoft.com/en-us/windows/security/identity-protection/user-account-control/user-account-control-group-policy-and-registry-key-settings#registry-key-settings
                //Relevant registry keys
                //https://admx.help/HKLM/Software/Policies/Microsoft/Windows/WinRM/Service
                outputFilename = m_extractionBaseFolder + @"\RegistryPolicy.txt";
                RegistryUtils.WriteRegistryPolicy(registryPolicyFile, outputFilename);
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }

        }

        internal static void ProcessCommand(string command)
        {
            //We configure ProcessStartInfo for launching commands from cmd
            //https://stackoverflow.com/questions/1369236/how-to-run-console-application-from-windows-service/1369252#1369252
            ProcessStartInfo startInfo = new ProcessStartInfo(@"cmd.exe", @"/c " + command);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.ErrorDialog = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process outproc = Process.Start(startInfo);

            while (!outproc.HasExited)
            {
                outproc.WaitForExit(200);
            }
        }




        private void GetAuditPolicies()
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\auditpolicies.csv";
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
                if (!IsAdministrator())
                {
                    outputFile.WriteLine("ERROR: access not auhtorized. Program requires administrator privileges.");
                    outputFile.Close();
                    return;
                }
                if (!Environment.Is64BitProcess)
                {
                    outputFile.WriteLine("Getting audit policies through API requires 64bits process to prevent exception.");
                    outputFile.Close();
                    return;
                }
                outputFile.WriteLine("CategoryName,CategoryGuid,SubCategoryName,SubCategoryGuid,Policy");
                List<string> categories = AuditPolicyUtils.GetCategories();
                foreach (string category in categories)
                {
                    string categoryName = AuditPolicyUtils.GetCategoryName(category);
                    List<string> subCategories = AuditPolicyUtils.GetSubCategories(category);
                    foreach (string subCategory in subCategories)
                    {
                        string subCategoryName = AuditPolicyUtils.GetSubCategoryName(subCategory);
                        string policy = AuditPolicyUtils.GetSystemPolicy(subCategory);
                        outputFile.WriteLine(categoryName + "," + category + "," + subCategoryName + "," + subCategory + "," + policy);
                    }
                }
                outputFile.Close();
            }
            catch (Exception exception)
            {
                LogUtils.AddLogEntry(exception);
            }
        }

    }
}
