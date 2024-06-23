using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;


namespace InformationUtils
{
    public class ScheduledTaskUtils
    {

        private string m_extractionBaseFolder;
        private string m_extractionTasksFolder;
        private const string s_registeredScheduledTaskHeader = "Taskname,LogonType,RunLevel,UserId,GroupId";

        internal ScheduledTaskUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
            m_extractionTasksFolder= m_extractionBaseFolder + @"\Tasks";
        }


        public static void ProcessScheduledTasksRunHighest(string inputFileName, string outputFilename, string taskPrincipalsFileName, string header)
        {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            List<ScheduledTaskPrincipal> scheduledTaskPrincipals= ScheduledTaskPrincipal.GetScheduledTaskPrincipals(taskPrincipalsFileName);

            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    Permission permission = Permission.GetPermission(line, inputFile);
                    //We filter tasks that are not running with the highest privileges
                    bool isRunHighest = IsRunHighest(objectToReview, scheduledTaskPrincipals);
                    if (isRunHighest)
                    {
                        outputFile.WriteLine(objectToReview+","+ ScheduledTaskPrincipal.HighestAvailable);
                        outputFile.WriteLine(permission.GetProperties());
                        outputFile.WriteLine();
                    }
                    continue;
                }
                objectToReview = line;
            }
            inputFile.Close();
            outputFile.Close();
        }

        private static bool IsRunHighest(string objectToReview, List<ScheduledTaskPrincipal> scheduledTaskPrincipals)
        {
            string[] separator = new string[1] { @"," };
            string[] fields = objectToReview.Split(separator, StringSplitOptions.None);
            string taskname = fields[0].ToUpper().Trim();
            foreach(ScheduledTaskPrincipal scheduledTaskPrincipal in scheduledTaskPrincipals)
            {
                if(scheduledTaskPrincipal.Taskname.ToUpper()== taskname)
                {
                    if(scheduledTaskPrincipal.RunLevel== ScheduledTaskPrincipal.HighestAvailable)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }           
            return false;
        }

        internal void GetRegsitrationInformationScheduledTasks()
        {

            SaveRegsitrationInformationScheduledTasks();

            string outputFilename = m_extractionBaseFolder + @"\RegistrationInformationScheduledTasks.csv";
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
            outputFile.WriteLine(s_registeredScheduledTaskHeader);

            DirectoryInfo folder=new DirectoryInfo(m_extractionTasksFolder);
            FileInfo[] files = folder.GetFiles();
            foreach (FileInfo file in files)
            {
                ProcessTaskFile(file.FullName, outputFile);
            }

            outputFile.Close();
        }


        private void SaveRegsitrationInformationScheduledTasks()
        {
            Directory.CreateDirectory(m_extractionTasksFolder);
            string inputFileName = m_extractionBaseFolder + @"\scheduledtasks.csv";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            //We prevent duplicated pairs of task and user
            List<string> tasks = new List<string>();

            //We read header. Header is repeated several times. 
            string line = inputFile.ReadLine();
            string[] separator = new string[1] { @"," };
            string[] fields = line.Split(separator, StringSplitOptions.None);
            string hostNameHeader = fields[0];

            while (!inputFile.EndOfStream)
            {
                line = inputFile.ReadLine();
                if (line.StartsWith(hostNameHeader))
                {
                    continue;
                }
                separator = new string[1] { @",""" };
                fields = line.Split(separator, StringSplitOptions.None);
                string taksName = fields[1].Replace(@"""", "");

                //We prevent processing repeated taskName
                if (!tasks.Contains(taksName))
                {

                    tasks.Add(taksName);
                    //ProcessComandWithProgress("schtasks /query /V /fo CSV > ", m_extractionBaseFolder + @"\scheduledtasks.csv", "Getting schtasks");
                    string command = "schtasks /query /xml /tn " + @"""" + taksName + @"""" + " > ";
                    string outputTaskFile = m_extractionTasksFolder + @"\" + taksName.Replace(@"\", "_");
                    ExtractWindowsInformationUtils.ProcessCommand(command + @"""" + outputTaskFile + @"""");
                }
            }
            inputFile.Close();
        }


        private void ProcessTaskFile(string file, StreamWriter outputFile)
        {
            String lines =File.ReadAllText(file);
            //We replace first line <?xml version="1.0" encoding="UTF-16"?>
            string source = "<?xml version=" + @"""" + "1.0" + @"""" + " encoding=" + @"""" + "UTF-16" + @"""" + "?>";
            string replace = "<?xml version=" + @"""" + "1.0" + @"""" + "?>";
            lines = lines.Replace(source,replace);
            File.WriteAllText(file, lines);
            
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(file);

            XmlNode root = xmlDoc.DocumentElement;


            string taskName = "";
            XmlNodeList registrationNodes = root.ChildNodes[0].ChildNodes;
            foreach (XmlNode node in registrationNodes)
            {
                if (node.Name == "URI")
                {
                    taskName = node.InnerText;
                    break;
                }
            }
            if (taskName == "")
            {
                throw new Exception("Unexpected XML format document for scheduled task.");
            }
            XmlNodeList principalNodes = root.ChildNodes[1].ChildNodes[0].ChildNodes;

            string logonType = "";
            string runLevel = "";
            string userId = "";
            string groupId = "";

            foreach (XmlNode node in principalNodes)
            {
                if (node.Name == "LogonType")
                {
                    logonType = node.InnerText;
                    continue;
                }
                if (node.Name == "RunLevel")
                {
                    runLevel=node.InnerText;
                    continue;
                }
                if (node.Name == "UserId")
                {
                    userId=node.InnerText;
                    continue;
                }
                if (node.Name == "GroupId")
                {
                    groupId=node.InnerText;
                    continue;
                }
                throw new Exception("Unexpected field in XML format document for scheduled task.");
            }
            string line = taskName + "," + logonType + "," + runLevel + "," + userId + ","+ groupId;
            outputFile.WriteLine(line);
        }



        internal void GetACLScheduledTasks()
        {
            string outputFilename = m_extractionBaseFolder + @"\ACLScheduledTasks.txt";
            string inputFileName = m_extractionBaseFolder + @"\scheduledtasks.csv";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
            //We prevent duplicated pairs of task and user
            List<string> tasks = new List<string>();

            //We read header. Header is repeated several times. 
            string line = inputFile.ReadLine();
            string[] separator = new string[1] { @"," };
            string[] fields = line.Split(separator, StringSplitOptions.None);
            string hostNameHeader = fields[0];

            while (!inputFile.EndOfStream)
            {
                line = inputFile.ReadLine();
                if (line.StartsWith(hostNameHeader))
                {
                    continue;
                }
                separator = new string[1] { @",""" };
                fields = line.Split(separator, StringSplitOptions.None);
                string taksName = fields[1].Replace(@"""", "");
                string nextRun = fields[2].Replace(@"""", ""); 
                string status = fields[3].Replace(@"""", ""); 
                string taskRun = fields[8].Replace(@"""", ""); 
                string scheduledTaskState = fields[11].Replace(@"""", ""); 
                string runAsUser = fields[14].Replace(@"""", ""); 
                string scheduleType = fields[18].Replace(@"""", ""); 
                string taskData = taksName + "," + nextRun + "," + status + "," + taskRun + "," + scheduledTaskState + "," + runAsUser + "," + scheduleType;
 
                //We prevent processing repeated tasks
                if (!tasks.Contains(taskData))
                {
                    string taskPath = ACLUtils.GetExecutablePath(taskRun);
                    //Sometimes, there are tasks that they are not related to files. We omit them
                    if (taskPath != null)
                    {
                        outputFile.WriteLine("Scheduled task:");
                        outputFile.WriteLine(taskData);
                        ACLUtils.ShowSecurityFile(taskPath, outputFile);

                        outputFile.WriteLine();
                        outputFile.WriteLine();
                    }
                    tasks.Add(taskData);
                }

            }
            inputFile.Close();
            outputFile.Close();
        }

        internal void GetUnquotedSearchPath()
        {
            string outputFilename = m_extractionBaseFolder + @"\unquoted_ScheduledTasks.txt";
            string inputFileName = m_extractionBaseFolder + @"\scheduledtasks.csv";

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            //We prevent duplicated pairs of task and user
            List<string> tasks = new List<string>();

            //We read header. Header is repeated several times.
            string line = inputFile.ReadLine();
            string[] separator = new string[1] { @"," };
            string[] fields = line.Split(separator, StringSplitOptions.None);
            string hostNameHeader = fields[0];

            while (!inputFile.EndOfStream)
            {
                line = inputFile.ReadLine();
                if (line.StartsWith(hostNameHeader))
                {
                    continue;
                }
                separator = new string[1] { @",""" };
                fields = line.Split(separator, StringSplitOptions.None);
                string taksName = fields[1].Replace(@"""", "");
                string nextRun = fields[2].Replace(@"""", ""); ;
                string status = fields[3].Replace(@"""", ""); ;
                string taskRun = fields[8].Replace(@"""", ""); ;
                string scheduledTaskState = fields[11].Replace(@"""", ""); ;
                string runAsUser = fields[14].Replace(@"""", ""); ;
                string scheduleType = fields[18].Replace(@"""", ""); ;
                string taskData = taksName + "," + nextRun + "," + status + "," + taskRun + "," + scheduledTaskState + "," + runAsUser + "," + scheduleType;


      
                //We prevent processing repeated tasks
                if (!tasks.Contains(taskData))
                {
                    //This issue should be fixed acording to CVE below. But we try to find scheduled tasks with path containing environmental variable,
                    //due to it is possible that quotes are not managed correctly
                    //https://bishopfox.com/blog/windows-task-scheduler-19044-advisory


                    //The Microsoft Windows Task Scheduler component does not properly quote executable paths when calling external programs whose paths contain
                    //an environment variable, if the environment variable’s value contains spaces and the remainder of the program’s path does not.
                    //This can result in an unquoted search path vulnerability even if the path appears to be quoted correctly in the scheduled task configuration.

                    if (IsPossibleUnquotedSearchPath(taskRun))
                    {
                        //We remove arguments
                        string executablePath = ACLUtils.GetExecutablePath(taskRun);
                        List<string> partialFolderPaths = ACLUtils.GetPartialPaths(executablePath);

                        foreach (string partialPath in partialFolderPaths)
                        {
                            outputFile.WriteLine("Possible unquoted search path in scheduled task:");
                            outputFile.WriteLine(partialPath + " corresponding to: " + line);
                            ACLUtils.ShowSecurityDirectory(partialPath, outputFile);
                        }
                    }
                    tasks.Add(taskData);
                }

            }
            inputFile.Close();
            outputFile.Close();
        }

        private bool IsPossibleUnquotedSearchPath(string taskRun)
        {
            if(!taskRun.Contains('%'))
            {
                return false;
            }
           
            int length = taskRun.Length;
            for(int i=0;i<length;i++) 
            { 
                if (taskRun[i] == '%')
                {
                    //It is possible that task run has different environmental variables.
                    //We return true if one of them contains spaces.
                    int environmentalVariableFirst = i + 1;
                    int environmentalVariableLast = NextSymbol(taskRun, i+1);
                    if(environmentalVariableLast==-1)
                    {
                        return false;
                    }
                    i = environmentalVariableLast;
                    environmentalVariableLast--;
                    string environmentalVariable = taskRun.Substring(environmentalVariableFirst, environmentalVariableLast - environmentalVariableFirst + 1);
                    string environmentalVariableValue = Environment.GetEnvironmentVariable(environmentalVariable);
                    if (environmentalVariableValue.Contains(" "))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        private int NextSymbol(string taskRun,int index) 
        {
            int length = taskRun.Length;
            for (int i = index; i < length; i++)
            {
                if (taskRun[i] == '%')
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

