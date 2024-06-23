using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinHardenApp.Configuration
{
    internal class TreeViewConfiguration
    {
        private TreeView m_filesTreeView;

        internal TreeViewConfiguration(TreeView filesTreeView)
        {
            m_filesTreeView = filesTreeView;
        }

        internal static TreeViewConfiguration GetTreeViewConfiguration(StreamReader inputFile)
        {
            TreeView filesTreeView = new TreeView();
            TreeNode workstationNode = null;
            string line = inputFile.ReadLine().Trim().ToUpper();
            string[] separator = new string[1] { @"=" };

            while (!inputFile.EndOfStream)
            {
                if (line == "")
                {
                    break;
                }
                if (line.StartsWith("WORKSTATION="))
                {
                    string hostName = line.Replace("WORKSTATION=", "");
                    workstationNode = filesTreeView.Nodes.Add(hostName, hostName);
                    line = inputFile.ReadLine().Trim().ToUpper();
                    continue;
                }
                if (line.Contains("="))
                {
                    string[] fields = line.Split(separator, StringSplitOptions.None);
                    string folderType = fields[0];
                    string folderPath = fields[1];

                    TreeNode folderNode = filesTreeView.Nodes.Add(folderType, folderType);
                    folderNode.ToolTipText = folderPath;
                    workstationNode.Nodes.Add(folderNode);
                    line = inputFile.ReadLine().Trim().ToUpper();
                    continue;
                }
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be WORKSTATION=Hostname or FolderType=FolderPath. Program has found: " + line);
            }
            TreeViewConfiguration treeViewConfiguration = new TreeViewConfiguration(filesTreeView);
            return treeViewConfiguration;
        }

        internal string GetTreeFileViewString()
        {
            string treeFileViewString = "";
            //Worksattions
            foreach (TreeNode workstationNode in m_filesTreeView.Nodes)
            {
                treeFileViewString += "WORKSTATION=" + workstationNode.Text + Environment.NewLine;
                foreach (TreeNode folder in workstationNode.Nodes)
                {
                    treeFileViewString += folder.Text + "=" + folder.ToolTipText + Environment.NewLine;
                }
            }
            return treeFileViewString;
        }

        internal TreeView FilesTreeView
        {
            get { return m_filesTreeView; }
            set { m_filesTreeView = value; }
        }
    }
}
