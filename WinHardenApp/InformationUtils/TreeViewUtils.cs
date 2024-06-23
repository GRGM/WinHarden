using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinHardenApp.AnalyzeInformationUtils
{
    internal static class TreeViewUtils
    {

        internal static void ClearTreeView(TreeView treeView)
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            treeView.EndUpdate();
        }

        internal static TreeNode GetNode(TreeNodeCollection nodes, string name)
        {
            if (nodes.ContainsKey(name))
            {
                TreeNode existingNode = nodes[name];
                return existingNode;
            }
            else
            {
                TreeNode newNode = nodes.Add(name, name);
                return newNode;
            }

        }
        internal static TreeNode GetNode(TreeNodeCollection nodes, string name, string toolTip)
        {
            if (nodes.ContainsKey(name))
            {
                TreeNode existingNode = nodes[name];
                return existingNode;
            }
            else
            {
                TreeNode newNode = nodes.Add(name, name);
                newNode.ToolTipText = toolTip;
                return newNode;
            }

        }
    }
}
