using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    internal class ServiceUtils
    {
        private string m_extractionBaseFolder;

        internal ServiceUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }

        internal void GetACLService()
        {
            string outputFilename = m_extractionBaseFolder + @"\ACLService.txt";
            using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
            {
                RegistryKey services = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services");
                string[] servicesList = services.GetSubKeyNames();
                foreach (string subKey in servicesList)
                {
                    RegistryKey service = services.OpenSubKey(subKey);


                    outputFile.WriteLine("\nService start type:");
                    outputFile.WriteLine(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + subKey);
                    object key = service.GetValue("Start");
                    if (key == null)
                    {
                        outputFile.WriteLine("Blank start type");
                    }
                    else
                    {
                        string start = (string)service.GetValue("Start").ToString();
                        if (start == "")
                        {
                            outputFile.WriteLine("Blank start type");
                        }
                        else
                        {
                            outputFile.WriteLine(start);
                        }
                    }
                    outputFile.WriteLine("Ended service start type:");
                    outputFile.WriteLine();

                    outputFile.WriteLine("Registry ACL:");
                    outputFile.WriteLine(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + subKey);

                    RegistrySecurity registrySecurity = service.GetAccessControl();
                    ACLUtils.ShowSecurityRegistry(registrySecurity, outputFile);


                    outputFile.WriteLine("Ended registry ACL:");
                    outputFile.WriteLine();

                    outputFile.WriteLine("Image path ACL:");
                    outputFile.WriteLine(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + subKey);
                    string imagePath = GetImagePath(service);
                    if (imagePath == null || imagePath == "")
                    {
                        outputFile.WriteLine("ImagePath not found");
                        outputFile.WriteLine("Ended image path ACL:");
                        continue;
                    }
                    else
                    {
                        outputFile.WriteLine("Image Path: " + imagePath);
                    }
                    string filteredPath = ACLUtils.GetExecutablePath(imagePath);
                    ACLUtils.ShowSecurityFile(filteredPath, outputFile);
                    outputFile.WriteLine("Ended image path ACL:");
                    outputFile.WriteLine();

                }
            }
        }


        internal void GetACLWDFService()
        {
            //https://learn.microsoft.com/en-us/windows-hardware/drivers/wdf/introduction-to-registry-keys-for-drivers
            string outputFilename = m_extractionBaseFolder + @"\ACLWDFService.txt";
            using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
            {
                RegistryKey services = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WUDF\Services");
                string[] servicesList = services.GetSubKeyNames();
                foreach (string subKey in servicesList)
                {
                    RegistryKey service = services.OpenSubKey(subKey);

                    outputFile.WriteLine("Registry ACL:");
                    outputFile.WriteLine(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WUDF\Services\" + subKey);

                    RegistrySecurity registrySecurity = service.GetAccessControl();
                    ACLUtils.ShowSecurityRegistry(registrySecurity, outputFile);


                    outputFile.WriteLine("Ended registry ACL:");
                    outputFile.WriteLine();

                    outputFile.WriteLine("Image path ACL:");
                    outputFile.WriteLine(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WUDF\Services\" + subKey);
                    string imagePath = GetImagePath(service);
                    if (imagePath == null || imagePath == "")
                    {
                        outputFile.WriteLine("ImagePath not found");
                        outputFile.WriteLine("Ended image path ACL:");
                        continue;
                    }
                    else
                    {
                        outputFile.WriteLine("Image Path: " + imagePath);
                    }
                    string filteredPath = ACLUtils.GetExecutablePath(imagePath);
                    ACLUtils.ShowSecurityFile(filteredPath, outputFile);
                    outputFile.WriteLine("Ended image path ACL:");
                    outputFile.WriteLine();

                }
            }
        }

        private string GetImagePath(RegistryKey service)
        {
            string imagePath = (string)service.GetValue("imagePath");
            if (imagePath == null || imagePath == "")
            {
                imagePath = (string)service.GetValue("HostProcessImagePath");
                if (imagePath == null || imagePath == "")
                {
                    return null;
                }
            }
            return imagePath;
        }

        internal void GetACLUnquotedService()
        {
            //https://www.hackingarticles.in/windows-privilege-escalation-unquoted-service-path/

            string inputFileName = m_extractionBaseFolder + @"\wmic_service.csv";
            string outputFilename = m_extractionBaseFolder + @"\service_unquoted_accesses.txt";

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
                if (line.StartsWith(@"Node"))
                {
                    continue;
                }
                //It is a quoted service

                string[] separator = new string[1] { @"," };
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string pathName = fields[3];
                if (pathName.Trim() == "")
                {
                    continue;
                }
                if (pathName.StartsWith(@""""))
                {
                    continue;
                }
                //We remove arguments
                string executablePath = ACLUtils.GetExecutablePath(pathName);
                List<string> partialFolderPaths = ACLUtils.GetPartialPaths(executablePath);

                //If there are no blanks in the path, we would be reviewing the permissions of the folder that contains the real service.
                if (partialFolderPaths.Count == 1)
                {
                    continue;
                }
                foreach (string partialPath in partialFolderPaths)
                {
                    outputFile.WriteLine("Partial path of possible unquoted service path:");
                    outputFile.WriteLine(partialPath + " corresponding to: " + line);
                    ACLUtils.ShowSecurityDirectory(partialPath, outputFile);
                }
            }
            inputFile.Close();
            outputFile.Close();
        }




        internal void GetServiceRights()
        {
            //https://book.hacktricks.xyz/windows/windows-local-privilege-escalation#modify-service-binary-path
            //https://www.winhelponline.com/blog/view-edit-service-permissions-windows/
            string inputFileName = m_extractionBaseFolder + @"\wmic_service.csv";
            string outputFilename = m_extractionBaseFolder + @"\service_executablePath_accesses.txt";

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
                if (line.StartsWith(@"Node"))
                {
                    continue;
                }
                string[] separator = new string[1] { @"," };
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string displayNamee = fields[1];
                string name = fields[2];
                string pathName = fields[3];
                string started = fields[4];
                string startMode = fields[5];
                string startName = fields[6];
                string state = fields[7];
                string status = fields[8];
                if (pathName.Trim() == "")
                {
                    continue;
                }
                outputFile.WriteLine("Service:");
                outputFile.WriteLine(displayNamee + "," + name + "," + status + "," + pathName + "," + started + "," + startMode + "," + startName + "," + state + "," + status);

                string taskPath = ACLUtils.GetExecutablePath(pathName);

                ACLUtils.ShowSecurityFile(taskPath, outputFile);
                outputFile.WriteLine();
                outputFile.WriteLine();
            }
            inputFile.Close();
            outputFile.Close();
        }
        /// <summary>
        /// This funciotn obtains the permissions on service and drivers. For example, it obtains which users may start or stop services.
        /// </summary>
        internal void GetServiceSecurityRulesControl()
        {
            string outputFilename = m_extractionBaseFolder + @"\service_or_device_security_rules.txt";

            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            ServiceController[] scServices = ServiceController.GetServices();
            ServiceController[] scDevices = ServiceController.GetDevices();

            foreach (ServiceController sc in scServices)
            {
                ACLUtils.ShowServiceSecurity(sc, outputFile);
            }
            foreach (ServiceController sc in scDevices)
            {
                ACLUtils.ShowServiceSecurity(sc, outputFile);
            }
            outputFile.Close();
        }


    }
}
