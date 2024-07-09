using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationUtils
{
    public static class LogUtils
    {
        private static string s_logFile = null;


        public static void InitLogFile(string logFile)
        {
            s_logFile = logFile;
            if(File.Exists(logFile))
            {
                File.Delete(logFile);   
            }
        }


        public static void AddLogEntry(Exception exception)
        {
            if(s_logFile==null)
            {
                return;
            }
            string exceptionInformation = "Message: " + exception.Message + Environment.NewLine +
            "Source: " + exception.Source + Environment.NewLine +
            "Stack Trace: " + exception.StackTrace + Environment.NewLine;
            File.AppendAllText(s_logFile, exceptionInformation);
        }
        public static void AddLogEntry(string message)
        {
            if (s_logFile == null)
            {
                return;
            }
            File.AppendAllText(s_logFile, message);
        }

        public static string LogFile
        {
            get
            { 
                return s_logFile;
            }
            set
            {
                s_logFile = value; 
            }
        }



    }
}
