using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    public class ActiveDirectoryUtils
    {
       public static string GetDefaultLDAPServer()
       {
            string defaultLDAPServer = null;
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                defaultLDAPServer = context.ConnectedServer;
            }
            catch
            {
            }
            return defaultLDAPServer;
       }
    }
}
