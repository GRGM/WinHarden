using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace InformationUtils
{
    public  class WindowsInformationUtils
    {
        private ToolStripLabel m_extractionLabel;
        private ToolStripProgressBar m_progressBar;


        public WindowsInformationUtils(ToolStripLabel extractionLabel, ToolStripProgressBar progressBar)
        {
            m_extractionLabel = extractionLabel;
            m_progressBar = progressBar;

        }

        public void UpdateProgressBar(string message)
        {
            m_extractionLabel.Text = message;
            m_progressBar.PerformStep();
            m_progressBar.ProgressBar.Refresh();
        }

        public void InitProgressBar(int maximum)
        {
            m_progressBar.Visible = true;
            m_progressBar.Minimum = 1;
            //Change to number of functions to extract data
            m_progressBar.Maximum = maximum;
            m_progressBar.Value = 1;
            m_progressBar.Step = 1;
        }

        public static string CheckFolder(string folder)
        {
            if(folder == null)
            {
                return null;
            }
            if(folder.Trim()=="")
            {
                return null;
            }
            int length = folder.Length;
            //If last characnter is 
            if (folder[length - 1] == '\\')
            {
                folder = folder.Substring(0, length - 1);
            }
            if (!Directory.Exists(folder))
            {
                return null;
            }
            return folder;
        }
    }
}
