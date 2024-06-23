using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.Remoting.Contexts;

namespace WinHardenApp.Configuration
{





    internal class WinHardenConfiguration
    {
        internal const string ExtractionLabel = "Configuration";
        internal const string AnalysisLabel = "Analysis";

        private static WinHardenConfiguration s_configuration = GetInitiialWinHardenConfiguration();

        private string m_configurationFile=null;
        private ComplianceConfiguration m_complianceConfiguration;
        private TreeViewConfiguration m_treeViewConfiguration;
        private ServerDomainConfiguration m_serverDomainConfiguration;
        private WindowOptionsConfiguration m_windowOptionsConfiguration;


        private WinHardenConfiguration(string configurationFile, ComplianceConfiguration complianceConfiguration, 
            TreeViewConfiguration treeViewConfiguration, ServerDomainConfiguration serverDomainConfiguration, WindowOptionsConfiguration windowOptionsConfiguration)
        {
            m_configurationFile = configurationFile;
            m_complianceConfiguration = complianceConfiguration;
            m_treeViewConfiguration = treeViewConfiguration;
            m_serverDomainConfiguration = serverDomainConfiguration;
            m_windowOptionsConfiguration = windowOptionsConfiguration;
        }

        internal static void InitWinHardenConfiguration()
        {
            s_configuration = GetInitiialWinHardenConfiguration();
        }

        private static WinHardenConfiguration GetInitiialWinHardenConfiguration()
        {
            AuditPolicyConfiguration auditPolicyConfiguration = new AuditPolicyConfiguration(AuditPolicyComplianceType.WorkstationRecommendations, null);
            PasswordPolicyConfiguration passwordPolicyConfiguration = new PasswordPolicyConfiguration(null,null);
            SecurityPolicyConfiguration securityPolicyConfiguration = new SecurityPolicyConfiguration("", false,"");

            ComplianceConfiguration auditPolicyComplianceConfiguration = new ComplianceConfiguration(auditPolicyConfiguration, passwordPolicyConfiguration, securityPolicyConfiguration);

            ServerDomainConfiguration serverDomainConfiguration = ServerDomainConfiguration.GetInitialServerDomainConfiguration();


            WindowOptionsConfiguration windowOptionsConfiguration = WindowOptionsConfiguration.GetInitialWindowOptionsConfiguration();

            WinHardenConfiguration configuration = new WinHardenConfiguration(null, auditPolicyComplianceConfiguration, null, serverDomainConfiguration, windowOptionsConfiguration);
            return configuration;
        }


        internal static void UpdateComplianceConfiguration(AuditPolicyConfiguration auditPolicyConfiguration, PasswordPolicyConfiguration passwordPolicyConfiguration, SecurityPolicyConfiguration securityPolicyConfiguration)
        {
            ComplianceConfiguration auditPolicyComplianceConfiguration =new ComplianceConfiguration(auditPolicyConfiguration, passwordPolicyConfiguration, securityPolicyConfiguration);
            s_configuration.m_complianceConfiguration = auditPolicyComplianceConfiguration;
        }


        internal static void UpdateWindowOptionsConfiguration(WindowOptionsConfiguration windowOptionsConfiguration)
        {
            s_configuration.m_windowOptionsConfiguration = windowOptionsConfiguration;
        }


        internal static void UpdateTreeView(TreeView treeView)
        {

            s_configuration.m_treeViewConfiguration = new TreeViewConfiguration(treeView);
        }


        internal static void UpdateServerDomainConfiguration(ServerDomainConfiguration serverDomainConfiguration)
        {
            s_configuration.m_serverDomainConfiguration = serverDomainConfiguration;
        }


        internal static void UpdateConfigurationFile(string fileName)
        {
            s_configuration.m_configurationFile = fileName;
        }

        internal static void Close()
        {
            InitWinHardenConfiguration();
        }

        internal static bool SetConfiguration(string configurationFile)
        {
            if(configurationFile.Trim()=="")
            {
                return false;
            }
            if (!File.Exists(configurationFile))
            {
                return false;
            }
            FileStream inputFileStream = new FileStream(configurationFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            ComplianceConfiguration complianceConfiguration = null;
            ServerDomainConfiguration serverDomainConfiguration = null;
            TreeViewConfiguration treeViewConfiguration= null;
            WindowOptionsConfiguration windowOptionsConfiguration = null;

            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim().ToUpper();
                if (line == "")
                {
                    continue;
                }
                if(line=="[COMPLIANCE_POLICY]")
                {
                    complianceConfiguration = ComplianceConfiguration.GetComplianceConfiguration(inputFile);
                    continue;
                }
                if(line=="[TREE_VIEW]")
                {
                    treeViewConfiguration = TreeViewConfiguration.GetTreeViewConfiguration(inputFile);
                    continue;
                }
                if (line == "[SERVER_DOMAIN]")
                {
                    serverDomainConfiguration = ServerDomainConfiguration.GetServerDomainConfiguration(inputFile);
                    continue;
                }
                if (line == "[WINDOW_OPTIONS]")
                {
                    windowOptionsConfiguration = WindowOptionsConfiguration.GetWindowOptionsConfiguration(inputFile);
                    continue;
                }
            }
            inputFile.Close();
            WinHardenConfiguration configuration = new WinHardenConfiguration(configurationFile, complianceConfiguration, treeViewConfiguration, serverDomainConfiguration, windowOptionsConfiguration);
            s_configuration = configuration;
            return true;
        }


        internal static void SaveConfiguration()
        {
            StreamWriter configurationtFile = new StreamWriter(s_configuration.ConfigurationFile, false, Encoding.Default);
            if (s_configuration.ComplianceConfiguration != null)
            {
                configurationtFile.WriteLine("[COMPLIANCE_POLICY]");
                string auditPolicyStr = s_configuration.ComplianceConfiguration.GetAuditPolicyString();
                configurationtFile.WriteLine(auditPolicyStr);
            }
            if (s_configuration.ServerDomainConfiguration != null)
            {
                configurationtFile.WriteLine("[SERVER_DOMAIN]");
                string serverDomainSt = s_configuration.m_serverDomainConfiguration.GetServerDomainString();
                configurationtFile.WriteLine(serverDomainSt);
            }
            if (s_configuration.m_treeViewConfiguration.FilesTreeView.Nodes.Count > 0)
            {
                configurationtFile.WriteLine("[TREE_VIEW]");
                string treeFileViewStr = s_configuration.m_treeViewConfiguration.GetTreeFileViewString();
                configurationtFile.WriteLine(treeFileViewStr);
            }
            if (s_configuration.m_windowOptionsConfiguration != null)
            {
                configurationtFile.WriteLine("[WINDOW_OPTIONS]");
                string windowOptionsStr = s_configuration.m_windowOptionsConfiguration.GetWindowOptionsConfigurationString();
                configurationtFile.WriteLine(windowOptionsStr);
            }
            configurationtFile.Close();

        }


        internal string ConfigurationFile
        {
            get
            {
                return m_configurationFile;
            }
        }

        internal ComplianceConfiguration ComplianceConfiguration
        {
            get
            {
                return m_complianceConfiguration;
            }
        }


        internal TreeViewConfiguration TreeViewConfiguration
        {
            get
            {
                return m_treeViewConfiguration;
            }

        }

        internal static WinHardenConfiguration Configuration
        {
            get
            {
                return s_configuration;
            }
        }


        internal ServerDomainConfiguration ServerDomainConfiguration
        {
            get
            {
                return m_serverDomainConfiguration;
            }
        }

        internal WindowOptionsConfiguration WindowOptionsConfiguration
        {
            get
            {
                return m_windowOptionsConfiguration;
            }
        }

    }
}
