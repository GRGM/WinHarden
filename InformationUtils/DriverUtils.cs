using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationUtils
{
    public class DriverUtils
    {
        private string m_extractionBaseFolder;
        private string m_analysisOutputFolder;


        public DriverUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }


        public DriverUtils(string extractionBaseFolder, string analysisOutputFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
            m_analysisOutputFolder = analysisOutputFolder;
        }


        internal void GetACLDriverFolders()
        {
            string outputFilename = m_extractionBaseFolder + @"\ACLDriverFolders.txt";
            using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
            {
                string driverPath = @"C:\WINDOWS\System32\drivers";
                outputFile.WriteLine("Folder path ACL");
                outputFile.WriteLine(driverPath);
                ACLUtils.ShowSecurityDirectory(driverPath, outputFile);
                //https://learn.microsoft.com/en-us/windows-hardware/drivers/wdf/introduction-to-registry-keys-for-drivers
                driverPath = @"C:\WINDOWS\System32\drivers\umdf";
                outputFile.WriteLine("Folder path ACL");
                outputFile.WriteLine(driverPath);
                ACLUtils.ShowSecurityDirectory(driverPath, outputFile);
                driverPath = @"C:\WINDOWS\inf";
                outputFile.WriteLine("Folder path ACL");
                outputFile.WriteLine(driverPath);
                ACLUtils.ShowSecurityDirectory(driverPath, outputFile);
                driverPath = @"C:\WINDOWS\System32\DriverStore\FileRepository";
                outputFile.WriteLine("Folder path ACL");
                outputFile.WriteLine(driverPath);
                ACLUtils.ShowSecurityDirectory(driverPath, outputFile);
            }
        }

        internal void GetACLDriverFiles()
        {
            string outputFilename = m_extractionBaseFolder + @"\ACLDriverFiles.txt";
            string inputFileName = m_extractionBaseFolder + @"\drivers.csv";
            StreamReader inputFile = new StreamReader(inputFileName, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            //We read first header line
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {

                string line = inputFile.ReadLine();
                //So we are not affected by commmas other than to separate the fields.
                line = line.Replace(@""",""", @"""_||_""");
                if (line.StartsWith(@"""HostName"""))
                {
                    continue;
                }
                string[] separator = new string[1] { @"_||_" };
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string moduleName = fields[0];
                string displayName = fields[1];
                string description = fields[2];
                string driverType = fields[3];
                string startMode = fields[4];
                string state = fields[5];
                string status = fields[6];
                string path = fields[13];

                outputFile.WriteLine("Driver file:");
                outputFile.WriteLine(moduleName + "," + displayName + "," + description + "," + driverType + "," + startMode + "," + state + "," + status);

                string taskPath = ACLUtils.GetExecutablePath(path);
                outputFile.WriteLine(taskPath);
                ACLUtils.ShowSecurityFile(taskPath, outputFile);

                outputFile.WriteLine();
                outputFile.WriteLine();

            }
            inputFile.Close();
            outputFile.Close();


        }

        public void AnalyseACLDriverFolders()
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLDriverFolders.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLDriverFolders_RESULT.txt";

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


        public void AnalyseACLDriverFiles()
        {
            string inputFileName = m_extractionBaseFolder + @"\ACLDriverFiles.txt";
            string outputFilename = m_analysisOutputFolder + @"\ACLDriverFiles_RESULT.txt";

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
                if (line.StartsWith("Driver file:"))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    objectToReview += inputFile.ReadLine().Trim();
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
