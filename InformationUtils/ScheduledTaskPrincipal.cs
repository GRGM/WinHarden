using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    internal class ScheduledTaskPrincipal
    {
        private string m_taskname;
        private string m_logonType;
        private string m_runLevel;
        private string m_userId;
        private string m_groupId;

        internal const string HighestAvailable = "HighestAvailable";

        internal ScheduledTaskPrincipal(string taskname, string logonType, string runLevel, string userId, string groupId)
        {
            m_taskname = taskname;
            m_logonType = logonType;
            m_runLevel = runLevel;
            m_userId = userId;
            m_groupId = groupId;
        }
        
        internal static List<ScheduledTaskPrincipal> GetScheduledTaskPrincipals(string taskPrincipalsFileName)
        {
            FileStream taskPrincipalsFileStream = new FileStream(taskPrincipalsFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader taskPrincipalsFile = new StreamReader(taskPrincipalsFileStream, Encoding.Default);


            List<ScheduledTaskPrincipal> scheduledTaskPrincipals=new List<ScheduledTaskPrincipal>();
            //We read the header
            taskPrincipalsFile.ReadLine();
            string[] separator = new string[1] { @"," };
            while (!taskPrincipalsFile.EndOfStream)
            {
                string line = taskPrincipalsFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }
                string[] fields = line.Split(separator, StringSplitOptions.None);
                ScheduledTaskPrincipal scheduledTaskPrincipal = new ScheduledTaskPrincipal(fields[0], fields[1], fields[2], fields[3], fields[4]);
                scheduledTaskPrincipals.Add(scheduledTaskPrincipal);
            }
            taskPrincipalsFile.Close();
            return scheduledTaskPrincipals;
        }

        internal string Taskname
        {
            get
            {
                return m_taskname;            
            }

        }
        internal string RunLevel
        {
            get
            {
                return m_runLevel;
            }
        }
    }
}
