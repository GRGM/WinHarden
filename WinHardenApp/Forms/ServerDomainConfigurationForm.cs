using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinHardenApp.Configuration;
using InformationUtils;

namespace WinHardenApp.Forms
{
    public partial class ServerDomainConfigurationForm : Form
    {
        private TreeView m_filesTreeView;

        public ServerDomainConfigurationForm(TreeView filesTreeView)
        {
            InitializeComponent();
            WinHardenConfiguration configuration = WinHardenConfiguration.Configuration;

            GetCurrentWorkstationLDAP();
            m_filesTreeView= filesTreeView;
            foreach (TreeNode workstationNode in filesTreeView.Nodes)
            {
                workstationListBox.Items.Add(workstationNode.Text);  
            }
            UpdateLdapTextBox();

            switch (configuration.ServerDomainConfiguration.ServerDomainType)
            {
                case ServerDomainType.DNSDomain:
                    DNSDomainRadioButton.Checked = true;
                    break;
                case ServerDomainType.UserDomain:
                    userDomainRadioButton.Checked = true;
                    break;
                case ServerDomainType.DefaultConnectedServer:
                    defaultServerRadioButton.Checked = true;
                    break;
            }
            if (configuration.ServerDomainConfiguration.ConnectedServer != "")
            {
                ldapTextBox.Text = configuration.ServerDomainConfiguration.ConnectedServer;
            }
            obtainGroupsCheckBox.Checked = configuration.ServerDomainConfiguration.ObtainGroups;

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            ServerDomainType serverDomainType = ServerDomainType.DNSDomain;

            if (DNSDomainRadioButton.Checked)
            {
                serverDomainType = ServerDomainType.DNSDomain;
            }
            if (userDomainRadioButton.Checked)
            {
                serverDomainType = ServerDomainType.UserDomain;
            }
            if (defaultServerRadioButton.Checked)
            {
                serverDomainType = ServerDomainType.DefaultConnectedServer;
            }

            string connectedServer=ldapTextBox.Text;
            
            ServerDomainConfiguration serverDomainConfiguration = new ServerDomainConfiguration(serverDomainType, connectedServer, obtainGroupsCheckBox.Checked);
            WinHardenConfiguration.UpdateServerDomainConfiguration(serverDomainConfiguration);
            MessageBox.Show("Server domain configuration has been defined");
            Close();
        }


        private void GetCurrentWorkstationLDAP()
        {
            string connectedServer = ActiveDirectoryUtils.GetDefaultLDAPServer();
            if (connectedServer == null)
            {
                connectedServer = "";
            }

            serverLabel.Text = connectedServer;
            DNSDomainLabel.Text = Environment.GetEnvironmentVariable("USERDNSDOMAIN");
            userDomainLabel.Text = Environment.GetEnvironmentVariable("USERDOMAIN");
        }




        private void currentWorksationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (currentWorksationRadioButton.Checked)
            {
                workstationListBox.Enabled = false;
                GetCurrentWorkstationLDAP();
            }
        }

        private void extractedWorkstationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(extractedWorkstationRadioButton.Checked)
            {
                workstationListBox.Enabled = true;
            }
        }


        private void UpdateLdapTextBox()
        {
            if (DNSDomainRadioButton.Checked)
            {
                ldapTextBox.Text = DNSDomainLabel.Text;

            }
            if (userDomainRadioButton.Checked)
            {
                ldapTextBox.Text = userDomainLabel.Text;

            }
            if (defaultServerRadioButton.Checked)
            {
                ldapTextBox.Text = serverLabel.Text;
            }
            ldapTextBox.Select(0, 0);
        }

        private void DNSDomainRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLdapTextBox();
        }

        private void userDomainRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLdapTextBox();
        }

        private void defaultServerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLdapTextBox();
        }

        private void workstationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWorkstation=workstationListBox.SelectedItem as string;
            TreeNode node=m_filesTreeView.Nodes[selectedWorkstation];
            TreeNode extractionNode= node.Nodes[WinHardenConfiguration.ExtractionLabel];
            if (!extractionNode.Nodes.ContainsKey("activeDirectory.txt"))
            {
                MessageBox.Show("ActiveDirectory.txt file has not been found.");
                return;
            }
            TreeNode fileNode = extractionNode.Nodes["activeDirectory.txt"];
            string fullpath = fileNode.ToolTipText;
            StreamReader inputFile = new StreamReader(fullpath, Encoding.Default);
 
            //We read first header line
            string connectedServer=inputFile.ReadLine().Replace("DefaultLDAPServer=","");
            string userDNSDomain = inputFile.ReadLine().Replace("USERDNSDOMAIN=", "");
            string userDomain = inputFile.ReadLine().Replace("USERDOMAIN=", "");

            inputFile.Close();
            serverLabel.Text = connectedServer;
            DNSDomainLabel.Text = userDNSDomain;
            userDomainLabel.Text = userDomain;

        }

        private void connectDomainServerButton_Click(object sender, EventArgs e)
        {
            PrincipalContext context;
            string connectedServer = WinHardenConfiguration.Configuration.ServerDomainConfiguration.ConnectedServer;
            try
            {

                context = new PrincipalContext(ContextType.Domain, WinHardenConfiguration.Configuration.ServerDomainConfiguration.ConnectedServer);
                MessageBox.Show("Successful domain server connection: " + connectedServer);
            }
            catch
            {
                MessageBox.Show("Failed domain server connection: " + connectedServer);
            }
        }
    }
}
