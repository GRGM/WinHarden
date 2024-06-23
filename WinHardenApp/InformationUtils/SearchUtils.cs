using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinHardenApp.Forms;
using static System.Windows.Forms.LinkLabel;

namespace WinHardenApp.AnalyzeInformationUtils
{
    internal class SearchUtils
    {
        private TreeView m_searchTreeView;

        internal SearchUtils(TreeView searchTreeView)
        {
            m_searchTreeView = searchTreeView;
        }

        internal void SearchCurrentFileTreeView(string searchText, TabPage selectedTab)
        {
            TreeViewUtils.ClearTreeView(m_searchTreeView);
            SearchTextInFile(selectedTab, searchText,true);
        }

        internal void SearchAllOpenFilesTreeView(string searchText, TabControl filesTabControl)
        {
            TreeViewUtils.ClearTreeView(m_searchTreeView);
            foreach(TabPage tabPage in filesTabControl.TabPages)
            {
                SearchTextInFile(tabPage, searchText,true);
            }
        }

        internal void SearchAllWorkingFoldersFilesTreeView(string searchText, TreeView FilesTreeView)
        {
            TreeViewUtils.ClearTreeView(m_searchTreeView);
            foreach(TreeNode node1 in FilesTreeView.Nodes)
            {
                //Workstation level
                foreach (TreeNode folderNode in node1.Nodes)
                {
                    //Folder level
                    foreach (TreeNode fileNode in folderNode.Nodes)
                    {
                        //File level
                        string fullFileName = fileNode.ToolTipText;
                        string[] lines = File.ReadAllLines(fullFileName, Encoding.Default);
                        SearchTextInLines(lines, searchText, fullFileName,false);
                    }
                }
            }
        }


        private void SearchTextInFile(TabPage tabPage, string searchText, bool isExpand)
        {
            RichTextBox richTextBox = tabPage.Controls[0] as RichTextBox;
            string[] lines = richTextBox.Lines;
            SearchTextInLines(lines, searchText, tabPage.ToolTipText, isExpand);
        }

        private void SearchTextInLines(string[] lines, string searchText,string fileName,bool isExpand)
        {
            TreeNode fileNode = TreeViewUtils.GetNode(m_searchTreeView.Nodes, fileName);
            bool foundText = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                //No case sensitive
                if (line.ToUpper().Contains(searchText.ToUpper()))
                {
                    string text = "Line " + i + ": " + line + Environment.NewLine;
                    fileNode.Nodes.Add(text);
                    foundText = true;
                }
            }
            if (foundText)
            {
                if (isExpand)
                {
                    fileNode.Expand();
                }
            }
            else
            {
                m_searchTreeView.Nodes.Remove(fileNode);
            }
        }
    }
}
