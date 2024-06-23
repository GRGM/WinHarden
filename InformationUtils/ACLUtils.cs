using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Management;
using System.Collections;
using System.Drawing;


namespace InformationUtils
{
    public static class ACLUtils
    {
        /// Find services that you can modify using PS os sc for example
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool QueryServiceObjectSecurity(
            IntPtr serviceHandle,
            SecurityInfos secInfo,
            byte[] lpSecDesrBuf,
            uint bufSize,
            out uint bufSizeNeeded);





        public static void WritePermissionRecursiveFolder(StreamWriter outputPermissionsFile, string path)
        {
            outputPermissionsFile.WriteLine(path);
            ShowSecurityDirectory(path, outputPermissionsFile);
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                outputPermissionsFile.WriteLine(file);
                ShowSecurityFile(file, outputPermissionsFile);
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
                WritePermissionRecursiveFolder(outputPermissionsFile, directory);
            }

        }



        internal static void ShowSecurityRegistry(RegistrySecurity security, StreamWriter outputFile)
        {
            try
            {
                string ownner = security.GetOwner(typeof(NTAccount)).ToString().ToUpper().Trim();
                foreach (RegistryAccessRule ar in security.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    string sidValue = ar.IdentityReference.ToString();
                    string user = SIDUtils.TranslatSIDToUser(sidValue);
                    if(user is null)
                    {
                        user = sidValue;
                    }
                    //We replace CREATOR OWNER by corresponding user to know which SID has privileges
                    if (user.ToUpper().Trim().Contains("CREATOR OWNER"))
                    {
                        user = ownner;
                        sidValue = SIDUtils.GetSidValue(ownner);
                    }

                    outputFile.WriteLine("        User: "+ user);
                    outputFile.WriteLine("        SID: "+ sidValue);
                    outputFile.WriteLine("        Type: "+ ar.AccessControlType);
                    string rights = GetRegistryRights(ar);
                    outputFile.WriteLine("      Rights: "+ rights);
                    outputFile.WriteLine(" Inheritance: "+ ar.InheritanceFlags);
                    outputFile.WriteLine(" Propagation: "+ ar.PropagationFlags);
                    outputFile.WriteLine("   Inherited? "+ ar.IsInherited);
                    outputFile.WriteLine();

                }
            }
            catch
            {
                outputFile.WriteLine("User has not rights for getting access rules");
            }

        }

        private static void ShowSecurityFile(FileSecurity security, StreamWriter outputFile)
        {
            string owner = security.GetOwner(typeof(NTAccount)).ToString().ToUpper().Trim();
            AuthorizationRuleCollection authorizationRuleCollection = security.GetAccessRules(true, true, typeof(SecurityIdentifier));
            WriteFileSystemAccessRule(authorizationRuleCollection, owner, outputFile);
        }
        private static void ShowSecurityDirectory(DirectorySecurity security, StreamWriter outputFile)
        {
            string owner = security.GetOwner(typeof(NTAccount)).ToString().ToUpper().Trim();
            AuthorizationRuleCollection authorizationRuleCollection=security.GetAccessRules(true, true, typeof(SecurityIdentifier));
            WriteFileSystemAccessRule(authorizationRuleCollection, owner, outputFile);
        }

        private static void WriteFileSystemAccessRule(AuthorizationRuleCollection authorizationRuleCollection,string owner, StreamWriter outputFile)
        {
            foreach (FileSystemAccessRule ar in authorizationRuleCollection)
            {
                string sidValue = ar.IdentityReference.ToString();
                string user = SIDUtils.TranslatSIDToUser(sidValue);
                if (user is null)
                {
                    user = sidValue;
                }
                if (user.ToUpper().Trim().Contains("CREATOR OWNER"))
                {
                    user = owner;
                    sidValue = SIDUtils.GetSidValue(owner);
                }

                outputFile.WriteLine("        User: " + user);
                outputFile.WriteLine("        SID: " + sidValue);
                outputFile.WriteLine("        Type: " + ar.AccessControlType);
                string rights = GetFileRights((int)ar.FileSystemRights);
                outputFile.WriteLine(" Rights: " + rights);
                outputFile.WriteLine(" Inheritance: " + ar.InheritanceFlags);
                outputFile.WriteLine(" Propagation: " + ar.PropagationFlags);
                outputFile.WriteLine("   Inherited? " + ar.IsInherited);
                outputFile.WriteLine();
            }
        }





        private static void WriteSecurity(StreamWriter outputFile, ManagementBaseObject mbo, string user, string trusteeSID)
        {
            string aceTypeString = "";
            if (mbo["AceType"].ToString() == "1")
            {
                aceTypeString = "DENY";
            }
            else
            {
                aceTypeString = "ALLOW";
            }
            UInt32 aceFlags = (UInt32)mbo["AceFlags"];
            //https://docs.microsoft.com/en-US/dotnet/api/system.security.accesscontrol.aceflags?view=net-6.0
            string inheritanceFlags = GetInheritanceFlags(aceFlags);
            string propagationFlags = GetPropagationFlags(aceFlags);
            string inheritedFlags = GetInheritedFlags(aceFlags);

            UInt32 mask = (UInt32)mbo["AccessMask"];


            outputFile.WriteLine("        User: " + user);
            outputFile.WriteLine("        SID: " + trusteeSID);
            outputFile.WriteLine("        Type: " + aceTypeString);

            string rights = GetFileRights((int)mask);

            outputFile.WriteLine(" Rights: " + rights);
            outputFile.WriteLine(" Inheritance: " + inheritanceFlags);
            outputFile.WriteLine(" Propagation: " + propagationFlags);
            outputFile.WriteLine("   Inherited? " + inheritedFlags);
            outputFile.WriteLine();


        }

        private static void ShowSecurityFile(ManagementBaseObject Descriptor, StreamWriter outputFile)
        {
            //We use these function when File.GetAccessControl(file.Trim()) does not work by any reason or we are trying to obtain the 
            //security of a shared file/folder

            ManagementBaseObject OwnerObject = ((ManagementBaseObject)(Descriptor.Properties["Owner"].Value));
            string ownerName = "CREATOR OWNER";
            string ownerSID = SIDUtils.s_CREATOR_OWNER;
            if (OwnerObject != null)
            {
                PropertyDataCollection OwnerProperties = OwnerObject.Properties;
                ownerName = OwnerProperties["Name"].Value.ToString();
                ownerSID = OwnerProperties["SidString"].Value.ToString();
            }
            ManagementBaseObject[] DaclObject = ((ManagementBaseObject[])(Descriptor.Properties["Dacl"].Value));

            foreach (ManagementBaseObject mbo in DaclObject)
            {
                ManagementBaseObject Trustee = ((ManagementBaseObject)(mbo["Trustee"]));
                string trusteeSID = Trustee.Properties["SIDString"].Value.ToString();

                string trusteeName = SIDUtils.TranslatSIDToUser(trusteeSID);
                if (trusteeName is null)
                {
                    trusteeName = trusteeSID;
                }
                if (trusteeName.ToUpper().Trim().Contains("CREATOR OWNER"))
                {
                    trusteeName = ownerName;
                    trusteeSID = ownerSID;
                }
                WriteSecurity(outputFile, mbo, trusteeName, trusteeSID);
            }
        }


        internal static void ShowSecurityShare(ManagementBaseObject Descriptor, StreamWriter outputFile)
        {
            ShowSecurityFile(Descriptor, outputFile);
        }




        private static string GetInheritanceFlags(UInt32 aceFlags)
        {
            string inheritanceFlags = "";
            //https://docs.microsoft.com/en-US/dotnet/api/system.security.accesscontrol.aceflags?view=net-6.0
            // InheritanceFlags    15 A logical OR of ObjectInherit, ContainerInherit, NoPropagateInherit, and InheritOnly.
            if ((aceFlags & 1) == 1)
            {
                inheritanceFlags += "ObjectInherit ";
            }

             if ((aceFlags & 2) == 2)
            {
                inheritanceFlags += "ContainerInherit ";
            }
            if ((aceFlags & 4) == 4)
            {
                inheritanceFlags += "NoPropagateInherit ";
            }

            if ((aceFlags & 8) == 8)
            {
                inheritanceFlags += "InheritOnly ";
            }
            if(inheritanceFlags == "")
            {
                inheritanceFlags = "None";
            }
            return inheritanceFlags;
        }
        private static string GetPropagationFlags(UInt32 aceFlags)
        {
            string propagationFlags = "";
            //https://docs.microsoft.com/en-US/dotnet/api/system.security.accesscontrol.aceflags?view=net-6.0
            //https://docs.microsoft.com/en-US/dotnet/api/system.security.accesscontrol.propagationflags?view=net-6.0

            if ((aceFlags & 4) == 4)
            {
                propagationFlags += "NoPropagateInherit ";
            }

            if ((aceFlags & 8) == 8)
            {
                propagationFlags += "InheritOnly ";
            }
            if (propagationFlags == "")
            {
                propagationFlags = "None";
            }

            return propagationFlags;
        }
        private static string GetInheritedFlags(UInt32 aceFlags)
        {
            string inheritedFlags = "False";
            //https://docs.microsoft.com/en-US/dotnet/api/system.security.accesscontrol.aceflags?view=net-6.0
            if ((aceFlags & 16) == 16)
            {
                inheritedFlags = "True";
            }
            return inheritedFlags;
        }


        internal static void ShowSecurityFile(string file, StreamWriter outputFile)
        {
            try
            {
                FileSecurity security = File.GetAccessControl(file.Trim());
                ShowSecurityFile(security, outputFile);

            }
            catch(Exception e)
            {

                //In some cases, for exmple when we try to obtain the access control on driver files, GetAccessControl fuction above generates an exception, nevertheless security descriptor may be obtained through WMI.

                //https://gist.github.com/yetanotherchris/4953130
                ManagementObject lfs = new ManagementObject(@"Win32_LogicalFileSecuritySetting.Path='" + file + "'");
                ManagementBaseObject outP = null;
                try
                {
                    outP = lfs.InvokeMethod("GetSecurityDescriptor", null, null);
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine("File does not exist: " + file + " . Exception:" + ex.Message);
                    return;
                }
                try
                {
                    if (((uint)(outP.Properties["ReturnValue"].Value)) == 0)
                    {
                        ManagementBaseObject Descriptor = ((ManagementBaseObject)(outP.Properties["Descriptor"].Value));
                        ShowSecurityFile(Descriptor, outputFile);
                    }
                    else
                    {
                        outputFile.WriteLine("User has not rights for getting access rules: " + e.Message);
                    }
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine("Uncontrolled error regarding to file: " + file + " . Exception:" + ex.Message);
                }
            }            
        }


        internal static void ShowSecurityDirectory(string directory, StreamWriter outputFile)
        {
            try
            {
                DirectorySecurity security = Directory.GetAccessControl(directory.Trim());
                ShowSecurityDirectory(security, outputFile);
            }
            catch (Exception e)
            {
                ManagementObject lfs = new ManagementObject(@"Win32_LogicalFileSecuritySetting.Path='" + directory + "'");
                ManagementBaseObject outP = null;
                try
                {
                    outP = lfs.InvokeMethod("GetSecurityDescriptor", null, null);
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine("Directory does not exist: " + directory + " . Exception:" + ex.Message);
                    return;
                }
                try
                {
                    if (((uint)(outP.Properties["ReturnValue"].Value)) == 0)
                    {
                        ManagementBaseObject Descriptor = ((ManagementBaseObject)(outP.Properties["Descriptor"].Value));
                        ShowSecurityFile(Descriptor, outputFile);
                    }
                    else
                    {
                        outputFile.WriteLine("User has not rights for getting access rules: " + e.Message);
                    }
                }
                catch (Exception ex)
                {
                    outputFile.WriteLine("Uncontrolled error regarding to directory: " + directory + " . Exception:" + ex.Message);
                }
            }
        }

        internal static void ShowServiceSecurity(ServiceController sc, StreamWriter outputFile)
        {
            var GetServiceHandle = typeof(ServiceController).GetMethod("GetServiceHandle", BindingFlags.Instance | BindingFlags.NonPublic);

            object[] readRights = { 0x00020000 };
            IntPtr handle;
            try
            {
                handle = (IntPtr)GetServiceHandle.Invoke(sc, readRights);
            }
            //If we do not have privileges to get the handle, we return to analyse next service.
            catch
            {
                outputFile.WriteLine("Error trying to obtain service handle on: " + sc.ServiceName + " " + sc.DisplayName);
                outputFile.WriteLine();
                return;
            }

            ServiceControllerStatus status = sc.Status;
            byte[] psd = new byte[0];
            bool ok = QueryServiceObjectSecurity(handle, SecurityInfos.DiscretionaryAcl, psd, 0, out uint bufSizeNeeded);
            if (!ok)
            {
                int err = Marshal.GetLastWin32Error();
                if (err == 122 || err == 0)
                { // ERROR_INSUFFICIENT_BUFFER
                    // expected; now we know bufsize
                    psd = new byte[bufSizeNeeded];
                    ok = QueryServiceObjectSecurity(handle, SecurityInfos.DiscretionaryAcl, psd, bufSizeNeeded, out bufSizeNeeded);
                }
                else
                {
                    outputFile.WriteLine("Error calling QueryServiceObjectSecurity() to get DACL for " + sc.ServiceName + ": error code=" + err);
                    outputFile.WriteLine();
                    return;
                }
            }
            if (!ok)
            {
                outputFile.WriteLine("Error calling QueryServiceObjectSecurity() to get DACL for " + sc.ServiceName + " " + sc.DisplayName);
                outputFile.WriteLine();
                return;
            }

            // get security descriptor via raw into DACL form so ACE ordering checks are done for us.
            RawSecurityDescriptor rsd = new RawSecurityDescriptor(psd, 0);
            RawAcl racl = rsd.DiscretionaryAcl;
            DiscretionaryAcl dacl = new DiscretionaryAcl(false, false, racl);

            outputFile.WriteLine("Security of service or driver: " + sc.ServiceName + " " + sc.DisplayName);
            foreach (System.Security.AccessControl.CommonAce ace in dacl)
            {
                string sidValue = ace.SecurityIdentifier.Value;
                string user = SIDUtils.TranslatSIDToUser(sidValue);
                if (user == null)
                {
                    user = sidValue;
                }
                outputFile.WriteLine("        User: {0}", user);
                outputFile.WriteLine("        SID: {0}", sidValue);
                if (ace.AceType==AceType.AccessAllowed || ace.AceType == AceType.AccessAllowedCallback || ace.AceType == AceType.AccessAllowedCallbackObject || ace.AceType == AceType.AccessAllowedCompound || ace.AceType == AceType.AccessAllowedObject)
                {
                    outputFile.WriteLine("        Type: {0}", "Allow");
                }
                else
                {
                    outputFile.WriteLine("        Type: {0}", "Denied");
                }
                string rights = GetServiceRights(ace.AccessMask);
                outputFile.WriteLine(" Rights: {0}", rights);
                outputFile.WriteLine();
            }
        }

        private static string GetServiceRights(int accessRule)
        {
            string rightsStr = "";

            // Access rights for a service
            // https://docs.microsoft.com/en-us/windows/win32/services/service-security-and-access-rights#access-rights-for-a-service

            //SERVICE_QUERY_CONFIG(0x0001)   Required to call the QueryServiceConfig and QueryServiceConfig2 functions to query the service configuration.
            if ((accessRule & 1) == 1)
            {
                rightsStr += "SERVICE_QUERY_CONFIG ";
            }

            //SERVICE_CHANGE_CONFIG(0x0002)  Required to call the ChangeServiceConfig or ChangeServiceConfig2 function to change the service configuration.Because this grants the caller the right to change the executable file that the system runs, it should be granted only to administrators.
            if ((accessRule & 2) == 2)
            {
                rightsStr += "SERVICE_CHANGE_CONFIG ";
            }

            //SERVICE_QUERY_STATUS(0x0004)   Required to call the QueryServiceStatus or QueryServiceStatusEx function to ask the service control manager about the status of the service.
            //Required to call the NotifyServiceStatusChange function to receive notification when a service changes status.
            if ((accessRule & 4) == 4)
            {
                rightsStr += "SERVICE_QUERY_STATUS ";
            }

            //SERVICE_ENUMERATE_DEPENDENTS(0x0008)   Required to call the EnumDependentServices function to enumerate all the services dependent on the service.
            if ((accessRule & 4) == 4)
            {
                rightsStr += "SERVICE_ENUMERATE_DEPENDENTS ";
            }

            //SERVICE_START(0x0010)   16    Required to call the StartService function to start the service.
            if ((accessRule & 16) == 16)
            {
                rightsStr += "SERVICE_START ";
            }

            //SERVICE_STOP(0x0020)   32   Required to call the ControlService function to stop the service.
            if ((accessRule & 32) == 32)
            {
                rightsStr += "SERVICE_STOP ";
            }


            //SERVICE_PAUSE_CONTINUE(0x0040) 64 Required to call the ControlService function to pause or continue the service.
            if ((accessRule & 64) == 64)
            {
                rightsStr += "SERVICE_PAUSE_CONTINUE ";
            }

            //SERVICE_INTERROGATE(0x0080)  128    Required to call the ControlService function to ask the service to report its status immediately.
            if ((accessRule & 128) == 128)
            {
                rightsStr += "SERVICE_INTERROGATE ";
            }

            //SERVICE_USER_DEFINED_CONTROL(0x0100) 256   Required to call the ControlService function to specify a user-defined control code.
            if ((accessRule & 256) == 256)
            {
                rightsStr += "SERVICE_USER_DEFINED_CONTROL ";
            }

            //SERVICE_ALL_ACCESS(0xF01FF)     983551    Includes STANDARD_RIGHTS_REQUIRED in addition to all access rights in this table.
            if ((accessRule & 983551) == 983551)
            {
                rightsStr += "SERVICE_ALL_ACCESS ";
            }


            rightsStr += GetStandardRights((int)accessRule);
            return rightsStr;
        }



        private static string GetStandardRights(int accessRule)
        {
            string rightsStr = "";


            //Delete  65536             bit 16
            //Specifies the right to delete a folder or file.
            if ((accessRule & 65536) == 65536)
            {
                rightsStr += "Delete ";
            }

            //ReadPermissions 131072   bit 17
            //Specifies the right to open and copy access and audit rules from a folder or file. This does not include the right to read data, file system attributes, and extended file system attributes.

            if ((accessRule & 131072) == 131072)
            {
                rightsStr += "ReadPermissions ";
            }

            //Read    131209
            //Specifies the right to open and copy folders or files as read - only.This right includes the ReadData right, ReadExtendedAttributes right, ReadAttributes right, and ReadPermissions right.

            if ((accessRule & 131209) == 131209)
            {
                rightsStr += "Read ";
            }

            // ReadAndExecute  131241
            //Specifies the right to open and copy folders or files as read - only, and to run application files.This right includes the Read right and the ExecuteFile right.

            if ((accessRule & 131241) == 131241)
            {
                rightsStr += "ReadAndExecute ";
            }

            //Modify  197055
            //Specifies the right to read, write, list folder contents, delete folders and files, and run application files.This right includes the ReadAndExecute right, the Write right, and the Delete right.

            if ((accessRule & 197055) == 197055)
            {
                rightsStr += "Modify ";
            }

            //FullControl 2032127
            //Specifies the right to exert full control over a folder or file, and to modify access control and audit rules.This value represents the right to do anything with a file and is the combination of all rights in this enumeration.

            if ((accessRule & 2032127) == 2032127)
            {
                rightsStr += "FullControl ";
            }

            //ChangePermissions   262144  bit 18
            //Specifies the right to change the security and audit rules associated with a file or folder.

            if ((accessRule & 262144) == 262144)
            {
                rightsStr += "ChangePermissions ";
            }

            //TakeOwnership   524288  bit 19
            //Specifies the right to change the owner of a folder or file.Note that owners of a resource have full access to that resource.

            if ((accessRule & 524288) == 524288)
            {
                rightsStr += "TakeOwnership ";
            }

            //Synchronize 1048576    bit 20
            //Specifies whether the application can wait for a file handle to synchronize with the completion of an I / O operation.This value is automatically set when allowing access and automatically excluded when denying access.

            if ((accessRule & 1048576) == 1048576)
            {
                rightsStr += "Synchronize ";
            }

            //2097152                 bit 21
            //4194304               bit 22
            //8388608                bit 23

            //16777216                bit 24
            //Right to access to SACL 

            if ((accessRule & 16777216) == 16777216)
            {
                rightsStr += "AccessSACL ";
            }


            // Bit 25. 26 and 27 are reserved.

            //https://docs.microsoft.com/en-us/windows/win32/secauthz/access-mask-format

            //268435456         bit 28
            //GENERIC_ALL All possible access rights

            if ((accessRule & 268435456) == 268435456)
            {
                rightsStr += "GENERIC_ALL ";
            }

            //536870912       bit 29
            //GENERIC_EXECUTE Execute access

            if ((accessRule & 536870912) == 536870912)
            {
                rightsStr += "GENERIC_EXECUTE ";
            }

            //1073741824       bit 30
            //GENERIC_WRITE   Write access

            if ((accessRule & 1073741824) == 1073741824)
            {
                rightsStr += "GENERIC_WRITE ";
            }

            //-2147483648       bit 31
            //GENERIC_READ	Read access

            if ((accessRule & -2147483648) == -2147483648)
            {
                rightsStr += "GENERIC_READ ";
            }
            return rightsStr;
        }


        private static string GetRegistryRights(RegistryAccessRule ar)
        {
            string rightsStr = "";
            //https://docs.microsoft.com/en-us/windows/win32/sysinfo/registry-key-security-and-access-rights


            //The valid access rights for registry keys include the DELETE, READ_CONTROL, WRITE_DAC, and WRITE_OWNER standard access rights.Registry keys do not support the SYNCHRONIZE standard access right.

            //The following table lists the specific access rights for registry key objects.

            //Value   Meaning


            //KEY_CREATE_LINK(0x0020)              32
            //Reserved for system use.


            //KEY_WOW64_64KEY(0x0100)            
            //Indicates that an application on 64 - bit Windows should operate on the 64 - bit registry view. This flag is ignored by 32 - bit Windows.For more information, see Accessing an Alternate Registry View.
            //This flag must be combined using the OR operator with the other flags in this table that either query or access registry values.
            //Windows 2000: This flag is not supported.

            //KEY_WOW64_32KEY(0x0200)
            //Indicates that an application on 64 - bit Windows should operate on the 32 - bit registry view.This flag is ignored by 32 - bit Windows.For more information, see Accessing an Alternate Registry View.
            //This flag must be combined using the OR operator with the other flags in this table that either query or access registry values.
            //Windows 2000: This flag is not supported.

            //KEY_EXECUTE(0x20019)
            //Equivalent to KEY_READ.


            //KEY_QUERY_VALUE(0x0001)       1
            //Required to query the values of a registry key.
            if (((int)ar.RegistryRights & 1) == 1)
            {
                rightsStr += "KEY_QUERY_VALUE ";
            }


            //KEY_SET_VALUE(0x0002)         2
            //Required to create, delete, or set a registry value.

            if (((int)ar.RegistryRights & 2) == 2)
            {
                rightsStr += "KEY_SET_VALUE ";
            }

            //KEY_CREATE_SUB_KEY(0x0004)        4 
            //Required to create a subkey of a registry key.

            if (((int)ar.RegistryRights & 4) == 4)
            {
                rightsStr += "KEY_CREATE_SUB_KEY ";
            }


            //KEY_ENUMERATE_SUB_KEYS(0x0008)       8
            //Required to enumerate the subkeys of a registry key.

            if (((int)ar.RegistryRights & 8) == 8)
            {
                rightsStr += "KEY_ENUMERATE_SUB_KEYS ";
            }

            //KEY_NOTIFY(0x0010)                   16
            //Required to request change notifications for a registry key or for subkeys of a registry key.

            if (((int)ar.RegistryRights & 16) == 16)
            {
                rightsStr += "KEY_NOTIFY ";
            }


            //KEY_WRITE(0x20006)   131078
            //Combines the STANDARD_RIGHTS_WRITE, KEY_SET_VALUE, and KEY_CREATE_SUB_KEY access rights.

            if (((int)ar.RegistryRights & 131078) == 131078)
            {
                rightsStr += "KEY_WRITE ";
            }


            //KEY_READ(0x20019)
            //Combines the STANDARD_RIGHTS_READ, KEY_QUERY_VALUE, KEY_ENUMERATE_SUB_KEYS, and KEY_NOTIFY values.

            if (((int)ar.RegistryRights & 131097) == 131097)
            {
                rightsStr += "KEY_READ ";
            }


            //KEY_ALL_ACCESS(0xF003F)        983103
            //Combines the STANDARD_RIGHTS_REQUIRED, KEY_QUERY_VALUE, KEY_SET_VALUE, KEY_CREATE_SUB_KEY, KEY_ENUMERATE_SUB_KEYS, KEY_NOTIFY, and KEY_CREATE_LINK access rights.

            if (((int)ar.RegistryRights & 983103) == 983103)
            {
                rightsStr += "KEY_ALL_ACCESS ";
            }

            rightsStr += GetStandardRights((int)ar.RegistryRights);

            return rightsStr;
        }

        internal static string GetFileRights(int fileSystemRights)
        {
            string rightsStr = "";
            //https://docs.microsoft.com/en-us/dotnet/api/system.security.accesscontrol.filesystemrights?view=net-6.0


            //ListDirectory   1          bit 0
            //Specifies the right to read the contents of a directory.
            if ((fileSystemRights & 1) == 1)
            {
                rightsStr += "ListDirectory ";
            }

            //ReadData    1              bit 0
            //Specifies the right to open and copy a file or folder.This does not include the right to read file system attributes, extended file system attributes, or access and audit rules.

            if ((fileSystemRights & 1) == 1)
            {
                rightsStr += "ReadData ";
            }

            //CreateFiles 2              bit 1
            //Specifies the right to create a file.This right requires the Synchronize value.

            if ((fileSystemRights & 2) == 2)
            {
                rightsStr += "CreateFiles ";
            }

            //WriteData   2             bit 1
            //Specifies the right to open and write to a file or folder.This does not include the right to open and write file system attributes, extended file system attributes, or access and audit rules.


            if ((fileSystemRights & 2) == 2)
            {
                rightsStr += "WriteData ";
            }

            //AppendData  4                bit 2
            //Specifies the right to append data to the end of a file.

            if ((fileSystemRights & 4) == 4)
            {
                rightsStr += "AppendData ";
            }


            //CreateDirectories   4         bit 2
            //Specifies the right to create a folder This right requires the Synchronize value.

            if ((fileSystemRights & 4) == 4)
            {
                rightsStr += "CreateDirectories ";
            }

            //ReadExtendedAttributes  8         bit 3
            //Specifies the right to open and copy extended file system attributes from a folder or file. For example, this value specifies the right to view author and content information. This does not include the right to read data, file system attributes, or access and audit rules.


            if ((fileSystemRights & 8) == 8)
            {
                rightsStr += "ReadExtendedAttributes ";
            }

            //WriteExtendedAttributes 16        bit 4
            //Specifies the right to open and write extended file system attributes to a folder or file.This does not include the ability to write data, attributes, or access and audit rules.

            if ((fileSystemRights & 16) == 16)
            {
                rightsStr += "WriteExtendedAttributes ";
            }


            //ExecuteFile 32                  bit 5
            //Specifies the right to run an application file.

            if ((fileSystemRights & 32) == 32)
            {
                rightsStr += "ExecuteFile ";
            }

            //Traverse    32                 bit 5
            //Specifies the right to list the contents of a folder and to run applications contained within that folder.

            if ((fileSystemRights & 32) == 32)
            {
                rightsStr += "Traverse ";
            }

            //DeleteSubdirectoriesAndFiles    64        bit 6
            //Specifies the right to delete a folder and any files contained within that folder.

            if ((fileSystemRights & 64) == 64)
            {
                rightsStr += "DeleteSubdirectoriesAndFiles ";
            }

            //ReadAttributes  128                       bit 7
            //Specifies the right to open and copy file system attributes from a folder or file.For example, this value specifies the right to view the file creation or modified date. This does not include the right to read data, extended file system attributes, or access and audit rules.


            if ((fileSystemRights & 128) == 128)
            {
                rightsStr += "ReadAttributes ";
            }


            //WriteAttributes 256                      bit 8
            //Specifies the right to open and write file system attributes to a folder or file.This does not include the ability to write data, extended attributes, or access and audit rules.

            if ((fileSystemRights & 256) == 256)
            {
                rightsStr += "WriteAttributes ";
            }

            //Write   278                        
            //Specifies the right to create folders and files, and to add or remove data from files.This right includes the WriteData right, AppendData right, WriteExtendedAttributes right, and WriteAttributes right.

            if ((fileSystemRights & 278) == 278)
            {
                rightsStr += "Write ";
            }

            rightsStr += GetStandardRights(fileSystemRights);

            return rightsStr;


        }

        private static string CompleteExecutablePath(string taskRun)
        {
            if (File.Exists(taskRun + ".EXE"))
            {
                return taskRun + ".EXE";
            }

            if (File.Exists(taskRun + ".SYS"))
            {
                return taskRun + ".SYS";
            }

            if (File.Exists(taskRun + ".DLL"))
            {
                return taskRun + ".DLL";
            }
            if (File.Exists(taskRun + ".BAT"))
            {
                return taskRun + ".BAT";
            }
            return null;
        }

        private static string GetTaskPathFromTaskRun(string taskRun)
        {
            int extension = 0;

            int exeIndex = taskRun.LastIndexOf(".EXE");
            int dllIndex = taskRun.LastIndexOf(".DLL");
            int sysIndex = taskRun.LastIndexOf(".SYS");
            //It may happen that there are .ps1 files without powershell.exe
            int ps1Index = taskRun.LastIndexOf(".PS1");
            int batIndex = taskRun.LastIndexOf(".BAT");

            if (dllIndex >= 0)
            {
                extension = dllIndex;
            }
            if (sysIndex >= 0)
            {
                extension = sysIndex;
            }
            if (ps1Index >= 0)
            {
                extension = ps1Index;
            }
            if (batIndex >= 0)
            {
                extension = batIndex;
            }
            //We place the comparation of .EXE in last place because sometime .Exe receive as a parameter a .DLL. So, in this case, executable path is until .exe extension.
            if (exeIndex >= 0)
            {
                extension = exeIndex;
            }

            string taskPath = taskRun.Substring(0, extension + 4);
            return taskPath;
        }
        private static string GetTaskPathAsScriptParameter(string taskRun)
        {
            //In this functiontaskPath we try to find .ps1, .vbs, .vbe, WSF,.WSH scripts that are invoked as parameters of powershell, cscript and wscript
            string taskPath = null;
            if (taskRun.Contains(".PS1") && taskRun.Contains("POWERSHELL.EXE"))
            {
                int exeIndex = taskRun.LastIndexOf("POWERSHELL.EXE");
                int pss1Index = taskRun.LastIndexOf(".PS1")+4;

                int startingIndex = exeIndex + "POWERSHELL.EXE".Length;
                int count = pss1Index - startingIndex;
                taskPath = taskRun.Substring(startingIndex, count).Trim();
                return taskPath;
            }
            if ((taskRun.Contains(".VBS") || taskRun.Contains(".VBE") || taskRun.Contains(".WSF") || taskRun.Contains(".WSH")))
            {
                if (taskRun.Contains("CSCRIPT.EXE") || taskRun.Contains("WSCRIPT.EXE"))
                {
                    int exeIndex = taskRun.LastIndexOf("SCRIPT.EXE");
                    int extension = 0;
                    int vbsIndex = taskRun.LastIndexOf(".VBS");
                    int vbeIndex = taskRun.LastIndexOf(".VBE");
                    int wsfIndex = taskRun.LastIndexOf(".WSF");
                    int wshIndex = taskRun.LastIndexOf(".WSH");

                    if (vbsIndex >= 0)
                    {
                        extension = vbsIndex+4;
                    }
                    if (vbeIndex >= 0)
                    {
                        extension = vbeIndex+4;
                    }
                    if (wsfIndex >= 0)
                    {
                        extension = wsfIndex + 4;
                    }
                    if (wshIndex >= 0)
                    {
                        extension = wshIndex + 4;
                    }
                    int startingIndex = exeIndex + "SCRIPT.EXE".Length;
                    int count = extension-startingIndex;
                    taskPath = taskRun.Substring(startingIndex, count).Trim();
                    return taskPath;
                }
            }
            return taskPath;
        }


        internal static string GetExecutablePath(string taskRun)
        {
            //This funcion is used to remove the parameters of taskRun
            taskRun = taskRun.Replace(@"\??\", "").ToUpper();
            //Sometimes, path do not include executable extension. So, we try to identify it and added it
            if (!(taskRun.Contains(".EXE") || taskRun.Contains(".DLL") || taskRun.Contains(".SYS") || taskRun.Contains(".BAT")) )
            {
                taskRun = CompleteExecutablePath(taskRun);
            }
            if(taskRun==null)
            {
                return null;
            }

            string taskPath = GetTaskPathAsScriptParameter(taskRun);
            if(taskPath==null)
            {
                taskPath = GetTaskPathFromTaskRun(taskRun);
            }

            string windir = Environment.GetEnvironmentVariable("WINDIR");
            string systemRoot = Environment.GetEnvironmentVariable("SYSTEMROOT");
            string programFiles86 = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)");
            //We do this one because obtaining PROGRAMFILES environment variable continues providing Program Files 86
            string programFiles = programFiles86.ToUpper().Replace(" (X86)","");

            taskPath = taskPath.Replace(@"%WINDIR%", windir);
            taskPath = taskPath.Replace(@"%SYSTEMROOT%", systemRoot);
            taskPath = taskPath.Replace(@"%PROGRAMFILES(X86)%", programFiles86);
            taskPath = taskPath.Replace(@"%PROGRAMFILES%", programFiles);
            taskPath = taskPath.Replace(@"""", "");
            taskPath = taskPath.Replace(@"\SYSTEMROOT", windir);
            if(taskPath.StartsWith(@"SYSTEM32\"))
            {
                taskPath = taskPath.Replace(@"SYSTEM32\", systemRoot+ @"\SYSTEM32\");
            }
            //If after all previous changes, we have a relative path, we consider that it is a executable placed on C:\Windows\System32
            if(!taskPath.Contains(":"))
            {
                taskPath = windir + @"\SYSTEM32\" + taskPath;
            }
            return taskPath;
        }


        internal static List<string> GetPartialPaths(string pathName)
        {
            List<string> partialPaths = new List<string>();
            string[] separator = new string[1] { @" " };
            string[] paths = pathName.Split(separator, StringSplitOptions.None);
            string partialPath = "";
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                if (i == 0)
                {
                    partialPath = path;
                }
                else
                {
                    partialPath += @" " + path;
                }
                int lastIndex2 = partialPath.LastIndexOf(@"\");
                string partialFolderPath = partialPath.Substring(0, lastIndex2);
                if (Directory.Exists(partialFolderPath))
                {
                    if (!partialPaths.Contains(partialFolderPath))
                    {
                        partialPaths.Add(partialFolderPath+"\\");
                    }
                }
            }
            //We do not have execute last interation because it is the full path.
            return partialPaths;
        }
    }
}
