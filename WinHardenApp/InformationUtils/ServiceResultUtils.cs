using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using InformationUtils;

namespace WinHardenApp.AnalyzeInformationUtils
{
    internal static class ServiceResultUtils
    {
        internal static string AnalyzeServiceType(StreamReader inputFile, StreamWriter outputFile)
        {
            string key = null;
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();

                //  Service start type:
                //https://docs.microsoft.com/en-us/dotnet/api/system.serviceprocess.servicestartmode?view=dotnet-plat-ext-6.0#:~:text=The%20ServiceStartMode%20is%20used%20by,by%20a%20user%20or%20application.
                if(line.StartsWith("HKEY"))
                {
                    int lastPosition=line.LastIndexOf(@"\");
                    key = line.Substring(lastPosition+1);
                    continue;
                }

                if (line == "Blank start type" || line == "4")
                {
                    //We do not continue analysing here because is not a running service. External loop will read to next service start type section.
                    return null;
                }
                if (line.StartsWith("Ended service start type:"))
                {
                    return key;
                }
            }
            return null;
        }


        internal static void AnalyzeUnsecureKey(StreamReader inputFile, StreamWriter outputFile, string serviceName,string serviceInformation)
        {
            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line=="")
                {
                    continue;
                }
                if (line.StartsWith("Ended registry ACL:"))
                {
                    return;
                }
                if (line.StartsWith("Registry ACL:"))
                {
                    objectToReview = "Service Name: " + serviceName + Environment.NewLine;
                    objectToReview += serviceInformation + Environment.NewLine;
                    objectToReview += inputFile.ReadLine();
                    continue;
                }
                if (!line.StartsWith("User:"))
                {
                    continue;
                }
                Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
            }
        }


        internal static void AnalyzeUnsecureKey(StreamReader inputFile, StreamWriter outputFile, string serviceName)
        {

            string objectToReview = "Service Name: " + serviceName + Environment.NewLine;
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Ended registry ACL:"))
                {
                    return;
                }
                if (!line.StartsWith("User:"))
                {
                    continue;
                }
                Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
            }
        }

        internal static void AnalyzeUnsecurePath(StreamReader inputFile, StreamWriter outputFile, string serviceName, string serviceInformation)
        {
            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("ImagePath not found"))
                {
                    return;
                }
                if (line.StartsWith("Ended image path ACL:"))
                {
                    return;
                }
                if (line.StartsWith("Image path ACL:"))
                {
                    inputFile.ReadLine();
                    objectToReview = "Service Name: " + serviceName + Environment.NewLine;
                    objectToReview += serviceInformation + Environment.NewLine;
                    objectToReview += inputFile.ReadLine();
                    continue;
                }
                if (!line.StartsWith("User:"))
                {
                    continue;
                }
                Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
            }
        }

        internal static void AnalyzeUnsecurePath(StreamReader inputFile, StreamWriter outputFile, string serviceName)
        {
            string objectToReview = "Service Name: " + serviceName + Environment.NewLine;
            objectToReview += inputFile.ReadLine();

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("ImagePath not found"))
                {
                    return;
                }
                if (line.StartsWith("Ended image path ACL:"))
                {
                    return;
                }
                if (!line.StartsWith("User:"))
                {
                    continue;
                }
                Permission.ProcessPermission(line, inputFile, outputFile, objectToReview);
            }
        }

        internal static string GetServiceInformation(string inputWMICservices, string serviceName)
        {
            List<string> writableServicePaths = new List<string>();
            FileStream inputFileStream = new FileStream(inputWMICservices, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            string serviceInformation = "It is not a configured service.";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if(line.Trim()=="")
                {
                    continue;
                }
                string[] separator = new string[1] { @"," };
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string service = fields[2].Trim().ToUpper();

                if (serviceName.ToUpper()==service)
                {
                    serviceInformation = line;
                    break;
                }
            }
            inputFile.Close();
            return serviceInformation;
        }





        internal static List<string> GetWritableServicePaths(string inputWritableServicePathsFileName)
        {
            List<string> writableServicePaths = new List<string>();
            FileStream inputFileStream = new FileStream(inputWritableServicePathsFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine();
                if(line.StartsWith("Service Name:"))
                {
                    string service = line.Replace("Service Name:","").Trim();
                    if(!writableServicePaths.Contains(service))
                    {
                        writableServicePaths.Add(service);
                    }
                }
                   
            }
            inputFile.Close();
            return writableServicePaths;
        }

    }
}
