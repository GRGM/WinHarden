using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    internal class ProcessesUtils
    {
        private int m_init_CommandLine;
        private int m_end_CommandLine;
        private int m_init_Description;
        private int m_end_Description;
        private int m_init_ExecutablePath;
        private int m_end_ExecutablePath;
        private int m_init_name;
        private int m_end_name;


        private ProcessesUtils(int init_CommandLine, int end_CommandLine, int init_Description, int end_Description, int init_ExecutablePath, int end_ExecutablePath, int init_name, int end_name)
        {
            m_init_CommandLine = init_CommandLine;
            m_end_CommandLine = end_CommandLine;
            m_init_Description = init_Description;
            m_end_Description = end_Description;
            m_init_ExecutablePath = init_ExecutablePath;
            m_end_ExecutablePath = end_ExecutablePath;
            m_init_name = init_name;
            m_end_name = end_name;
        }

        internal static ProcessesUtils GetProcessesUtils(string header)
        {
            int init_CommandLine = header.IndexOf("CommandLine");
            int end_CommandLine = header.IndexOf("CSName");

            int init_Description = header.IndexOf("Description");
            int end_Description = header.IndexOf("ExecutablePath");

            int init_ExecutablePath = header.IndexOf("ExecutablePath");
            int end_ExecutablePath = header.IndexOf("ExecutionState");

            int init_name = header.IndexOf(" Name")+1;
            int end_name = header.IndexOf("OSName");
            ProcessesUtils processesUtils = new ProcessesUtils(init_CommandLine, end_CommandLine, init_Description, end_Description, init_ExecutablePath, end_ExecutablePath, init_name, end_name);
            return processesUtils;
        }

        internal string GetCommandLine(string line)
        {
            return line.Substring(m_init_CommandLine, m_end_CommandLine - m_init_CommandLine);
        }
        internal string GetDescription(string line)
        {
            return line.Substring(m_init_Description, m_end_Description - m_init_Description);
        }
        internal string GetExecutablePath(string line)
        {
            return line.Substring(m_init_ExecutablePath, m_end_ExecutablePath - m_init_ExecutablePath);
        }
        internal string GetName(string line)
        {
            return line.Substring(m_init_name, m_end_name - m_init_name);
        }




    }
}
