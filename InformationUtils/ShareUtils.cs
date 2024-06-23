using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management;
using System.Runtime.Remoting.Contexts;

namespace InformationUtils
{
    internal class ShareUtils
    {
         private string m_extractionBaseFolder;


        internal ShareUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }

        internal void GetACLNetShares(string filename)
        {
            string outputFilename = m_extractionBaseFolder +@"\"+ filename;
            if (File.Exists(outputFilename))
            {
                File.Delete(outputFilename);
            }
            string inputFileName = m_extractionBaseFolder + @"\netshare.csv";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            //We read first blank line
            inputFile.ReadLine();
            //We read header
            inputFile.ReadLine();
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
                UInt32 type = UInt32.Parse(fields[8]);
                //https://powershellmagazine.com/2014/07/24/pstip-select-non-administrative-shared-folders-using-win32_share-wmi-class/
                //We focus in non administrative shares
                //if (type >= 0 && type <= 3)
                //{
                ExtractWindowsInformationUtils.ProcessCommand(@"net share " + share + " >> " +@"""" + outputFilename + @"""");
                ExtractWindowsInformationUtils.ProcessCommand(@"echo: >> " +@"""" + outputFilename + @"""");
                //}
            }
            inputFile.Close();
        }

        internal void GetACLWMIShares(string filename)
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
                    outputFile.WriteLine(share["Name"] as string);
                    InvokeMethodOptions options = new InvokeMethodOptions();
                    ManagementBaseObject outParamsMthd = share.InvokeMethod("GetSecurityDescriptor", null, options);
                    ManagementBaseObject descriptor = outParamsMthd["Descriptor"] as ManagementBaseObject;
                    ACLUtils.ShowSecurityShare(descriptor, outputFile);
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
