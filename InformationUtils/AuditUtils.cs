using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    internal class AuditUtils
    {
        public static void WriteAuditConfigurationRecursiveFolder(StreamWriter outputAuditFile, string path)
        {
            outputAuditFile.WriteLine(path);
            ShowAuditFolder(path, outputAuditFile);
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                outputAuditFile.WriteLine(file);
                ShowAuditFile(file, outputAuditFile);
            }

            string[] directories;
            try
            {
                directories = Directory.GetDirectories(path);
            }
            catch
            {
                //the folder is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return;
            }
            //We carry out look in-wide
            foreach (string directory in directories)
            {
                WriteAuditConfigurationRecursiveFolder(outputAuditFile, directory);
            }

        }

        private static void ShowAuditFile(string path, StreamWriter outputAuditFile)

        {
            try
            {
                FileSecurity fileSecurity = new FileSecurity(path, AccessControlSections.Audit);

                // Retrieve the audit rules 
                AuthorizationRuleCollection auditRules = fileSecurity.GetAuditRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

                // Display the audit configuration 
                WriteFileSystemAuditRules(outputAuditFile, auditRules);
            }
            catch (Exception ex)
            {
                outputAuditFile.WriteLine($"Error retrieving audit configuration: {ex.Message}");
            }
        }

        private static void ShowAuditFolder(string path, StreamWriter outputAuditFile)
        {
            try
            {
                // Get the directory security 
                DirectorySecurity directorySecurity = new DirectorySecurity(path, AccessControlSections.Audit);

                //// Retrieve the audit rules 
                AuthorizationRuleCollection auditRules = directorySecurity.GetAuditRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

                // Display the audit configuration 
                WriteFileSystemAuditRules(outputAuditFile, auditRules);
            }

            catch (Exception ex)
            {
                outputAuditFile.WriteLine($"Error retrieving audit configuration: {ex.Message}");
            }
        }


        private static void WriteFileSystemAuditRules(StreamWriter outputAuditFile,AuthorizationRuleCollection auditRules)
        {
            foreach (FileSystemAuditRule rule in auditRules)
            {
                string sidValue = rule.IdentityReference.ToString();
                string user = SIDUtils.TranslatSIDToUser(sidValue);
                if (user is null)
                {
                    user = sidValue;
                }

                outputAuditFile.WriteLine("        User: " + user);
                outputAuditFile.WriteLine("        SID: " + sidValue);
                outputAuditFile.WriteLine("        Type: " + rule.AuditFlags);
                string rights = ACLUtils.GetFileRights((int)rule.FileSystemRights);
                outputAuditFile.WriteLine(" Rights: " + rights);
                outputAuditFile.WriteLine(" Inheritance: " + rule.InheritanceFlags);
                outputAuditFile.WriteLine(" Propagation: " + rule.PropagationFlags);
                outputAuditFile.WriteLine("   Inherited? " + rule.IsInherited);
                outputAuditFile.WriteLine();
            }
        }



    }
}
