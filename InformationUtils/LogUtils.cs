using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationUtils
{
    internal static class LogUtils
    {
        private static string s_logFile = null;

        internal static void AddLogEntry(Exception exception)
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

        internal static string LogFile
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
