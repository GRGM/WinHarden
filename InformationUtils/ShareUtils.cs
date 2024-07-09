using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management;
using System.Runtime.Remoting.Contexts;
using System.Runtime.InteropServices.ComTypes;

namespace InformationUtils
{
    internal class ShareUtils
    {
         private string m_extractionBaseFolder;


        internal ShareUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }


        internal Dictionary<string, string> GetACLNetShares(string filename)
        {
            string outputFilename = m_extractionBaseFolder +@"\"+ filename;
            if (File.Exists(outputFilename))
            {
                File.Delete(outputFilename);
            }
            if (!ExtractWindowsInformationUtils.IsAdministrator())
            {
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
                outputFile.WriteLine("Extraction has not being executed under administration privileges: permissions on shares are not shown in this file.");
                outputFile.Close();
            }

            string inputFileName = m_extractionBaseFolder + @"\netshare.csv";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            //We read first blank line
            inputFile.ReadLine();
            //We read header
            inputFile.ReadLine();

            Dictionary<string, string> sharesPath =  new Dictionary<string, string>();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if (line.Trim() == "")
                {
                    continue;
                }
                string[] separator = new string[1] { @"," };
                string[] fields = line.Split(separator, StringSplitOptions.None);

                string share = fields[5];
                string localPath = fields[6];
                UInt32 type = UInt32.Parse(fields[8]);
                //https://powershellmagazine.com/2014/07/24/pstip-select-non-administrative-shared-folders-using-win32_share-wmi-class/
                ExtractWindowsInformationUtils.ProcessCommand(@"net share " + share + " >> " +@"""" + outputFilename + @"""");
                ExtractWindowsInformationUtils.ProcessCommand(@"echo: >> " +@"""" + outputFilename + @"""");

                sharesPath.Add(share, localPath);
            }
            inputFile.Close();
            return sharesPath;
        }

        internal void GetACLWMIShares(string filename, Dictionary<string, string> sharesPath)
        {
            try
            {
                string outputFilename = m_extractionBaseFolder + @"\" + filename;
                //https://stackoverflow.com/questions/6227892/reading-share-permissions-in-c-sharp
                StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

                ConnectionOptions myConnectionOptions = new ConnectionOptions();

                myConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
                myConnectionOptions.Authentication = AuthenticationLevel.Packet;

                ManagementScope myManagementScope =
                    new ManagementScope(@"\\localhost\root\cimv2", myConnectionOptions);

                myManagementScope.Connect();


                ManagementObjectSearcher myObjectSearcher =
                    new ManagementObjectSearcher(myManagementScope.Path.ToString(), "SELECT * FROM Win32_LogicalShareSecuritySetting");

                foreach (ManagementObject share in myObjectSearcher.Get())
                {
                    outputFile.WriteLine("Net share:");
                    string name = share["Name"] as string;
                    outputFile.WriteLine("Share name: "+ name);
                    string logicalPath = sharesPath[name];
                    outputFile.WriteLine("Local shared path: "+ logicalPath);
                    InvokeMethodOptions options = new InvokeMethodOptions();
                    ManagementBaseObject outParamsMthd = share.InvokeMethod("GetSecurityDescriptor", null, options);
                    ManagementBaseObject descriptor = outParamsMthd["Descriptor"] as ManagementBaseObject;
                    outputFile.WriteLine("Share security: ");
                    ACLUtils.ShowSecurityShare(descriptor, outputFile);
                    outputFile.WriteLine("Local path security:");
                    ACLUtils.ShowSecurityDirectory(logicalPath, outputFile);
                }
                outputFile.Close();
            }
            catch (Exception e)
            {
                string exceptionInformation = "Message: " + e.Message + Environment.NewLine +
                                            "Source: " + e.Source + Environment.NewLine +
                                            "Stack Trace: " + e.StackTrace + Environment.NewLine;
                File.AppendAllText(m_extractionBaseFolder + @"\log.txt", exceptionInformation);
            }
        }
    }
}
