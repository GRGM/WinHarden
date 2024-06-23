using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

namespace WinHardenApp.Configuration
{

    internal enum ServerDomainType
    {

        DefaultConnectedServer,
        DNSDomain,
        UserDomain
    }

    internal class ServerDomainConfiguration
    {
        private ServerDomainType m_serverDomainType;
        private string m_connectedServer;
        private bool m_obtainGroups;

        internal ServerDomainConfiguration(ServerDomainType serverDomainType, string connectedServer, bool obtainGroups)
        {
            m_serverDomainType = serverDomainType;
            m_connectedServer = connectedServer;
            m_obtainGroups = obtainGroups;
        }

        internal static ServerDomainConfiguration GetInitialServerDomainConfiguration()
        {
            ServerDomainConfiguration serverDomainConfiguration = new ServerDomainConfiguration(ServerDomainType.DNSDomain, Environment.GetEnvironmentVariable("USERDNSDOMAIN"),false);
            return serverDomainConfiguration;
        }

        internal string GetServerDomainString()
        {
            string serverDomainString = "";
            switch (m_serverDomainType)
            {
                case ServerDomainType.DNSDomain:
                    serverDomainString="SERVER_DOMAIN_TYPE=DNSDomain".ToUpper()+Environment.NewLine;
                    break;
                case ServerDomainType.UserDomain:
                    serverDomainString="SERVER_DOMAIN_TYPE=UserDomain".ToUpper() + Environment.NewLine;
                    break;
                case ServerDomainType.DefaultConnectedServer:
                    serverDomainString="SERVER_DOMAIN_TYPE=ConnectedServer".ToUpper() + Environment.NewLine;
                    break;
            }
            serverDomainString += "CONNECTED_SERVER=" + m_connectedServer+Environment.NewLine;
            if (m_obtainGroups)
            {
                serverDomainString += "OBTAIN_GROUPS=TRUE" + Environment.NewLine;
            }
            else
            {
                serverDomainString += "OBTAIN_GROUPS=FALSE" + Environment.NewLine;
            }
            return serverDomainString;
        }

        internal static ServerDomainConfiguration GetServerDomainConfiguration(StreamReader inputFile)
        {
            ServerDomainType serverDomainType=ServerDomainType.UserDomain;
            string line = inputFile.ReadLine().ToUpper();
            if(!line.StartsWith("SERVER_DOMAIN_TYPE="))
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be SERVER_DOMAIN_TYPE=DNSDomain|UserDomain|ConnectedServer. Program has found: " + line);
            }
            string serverDomainTypeStr = line.Replace("SERVER_DOMAIN_TYPE=", "");
            if (serverDomainTypeStr == "DNSDomain".ToUpper())
            {
                serverDomainType = ServerDomainType.DNSDomain;

            }
            if (serverDomainTypeStr == "UserDomain".ToUpper())
            {
                serverDomainType = ServerDomainType.UserDomain;

            }
            if (serverDomainTypeStr == "ConnectedServer".ToUpper())
            {
                serverDomainType = ServerDomainType.DefaultConnectedServer;
            }
            line = inputFile.ReadLine().ToUpper();
            if (!line.StartsWith("CONNECTED_SERVER="))
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be CONNECTED_SERVER=server. Program has found: " + line);
            }
            string connectedServer = line.Replace("CONNECTED_SERVER=", "");

            line = inputFile.ReadLine().ToUpper();
            string obtainGroupsStr = line.Replace("OBTAIN_GROUPS=", "");
            bool obtainGroups = false;
            if (!(obtainGroupsStr == "True".ToUpper() || obtainGroupsStr == "False".ToUpper()))
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be OBTAIN_GROUPS=TRUE|FALSE. Program has found: " + line);
            }
            if (obtainGroupsStr == "True".ToUpper())
            {
                obtainGroups = true;

            }
            if (obtainGroupsStr == "False".ToUpper())
            {
                obtainGroups = false;

            }

            ServerDomainConfiguration serverDomainConfiguration = new ServerDomainConfiguration(serverDomainType, connectedServer, obtainGroups);
            return serverDomainConfiguration;
        }



        internal ServerDomainType ServerDomainType
        {
            get
            {
                return m_serverDomainType;
            }
        }

        internal string ConnectedServer
        {
            get { return m_connectedServer; }
        }

        internal bool ObtainGroups
        {
            get { return m_obtainGroups; }
        }

    }
}
