using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;


namespace InformationUtils
{
    //This class has been slightly fixed to run in 64 bits machines. Besides, it provokes that this program has to be compiled in x64
    //https://david-homer.blogspot.com/2016/08/document-windows-advanced-audit-policy.html

    /// <summary>
    /// Provides management functions of the advanced audit policy (audit policy subcategory settings).
    /// </summary>
    public static class AuditPolicyUtils
    {
        //https://docs.microsoft.com/en-us/windows/win32/api/ntsecapi/ns-ntsecapi-audit_policy_information
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct AUDIT_POLICY_INFORMATION
        {
            public Guid SubCategoryGuid;
            public ulong PolicyType;
            public Guid CategoryGuid;
        }

        [DllImport("advapi32.dll")]
        private static extern void AuditFree(IntPtr buffer);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AuditEnumerateCategories(out IntPtr ppAuditCategoriesArray, out uint pCountReturned);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AuditLookupCategoryName(ref Guid pAuditCategoryGuid, out string ppszCategoryName);


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AuditEnumerateSubCategories(ref Guid pAuditCategoryGuid, bool bRetrieveAllSubCategories, out IntPtr ppAuditSubCategoriesArray, out uint pCountReturned);


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AuditLookupSubCategoryName(ref Guid pAuditSubCategoryGuid, out string ppszSubCategoryName);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AuditQuerySystemPolicy(Guid pSubCategoryGuids, ulong PolicyCount, out IntPtr ppAuditPolicy);


        internal static List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            IntPtr categoriesArray;
            uint categoryCount;
            bool isFound = AuditEnumerateCategories(out categoriesArray, out categoryCount);
            if (!isFound)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error()); 
            }
            for (long i = 0, elements = (long)categoriesArray; i < categoryCount; i++)
            {
                Guid guid = (Guid)Marshal.PtrToStructure((IntPtr)elements, typeof(Guid));
                categories.Add(Convert.ToString(guid));
                elements += Marshal.SizeOf(typeof(Guid));
            }
            AuditFree(categoriesArray);
            return categories;
        }


        internal static String GetCategoryName(String guidStr)
        {
            Guid guid=new Guid(guidStr);
            string category = null;
            bool isFound = AuditLookupCategoryName(ref guid, out category);
            if (!isFound) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
            if (category == null)
            {
                throw new Exception("Not found GUID: " + guid);
            }
            return category;
        }

        internal static List<string> GetSubCategories(String categoryGuidStr)
        {
            Guid categoryGuid = new Guid(categoryGuidStr);
            List<string> subcategories = new List<string>();
            IntPtr subcategoriesArray;
            uint subCategoryCount;
            bool isFound = AuditEnumerateSubCategories(ref categoryGuid, false, out subcategoriesArray, out subCategoryCount);
            if (!isFound) { throw new Win32Exception(Marshal.GetLastWin32Error()); }
            for (long i = 0, elements = (long)subcategoriesArray; i < subCategoryCount; i++)
            {
                Guid guid = (Guid)Marshal.PtrToStructure((IntPtr)elements, typeof(Guid));
                subcategories.Add(Convert.ToString(guid));
                elements += Marshal.SizeOf(typeof(Guid));
            }
            AuditFree(subcategoriesArray);
            return subcategories;
        }

        public static String GetSubCategoryName(String guidStr)
        {

            Guid guid = new Guid(guidStr);
            string subcategories = null;
            bool isFound = AuditLookupSubCategoryName(ref guid, out subcategories);
            if (!isFound)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return subcategories;
        }

        internal static string GetSystemPolicy(String subCategoryGuidStr)
        {
            Guid guid = new Guid(subCategoryGuidStr);
            IntPtr auditPolicy=IntPtr.Zero;
            bool success = AuditQuerySystemPolicy(guid, (long)1, out auditPolicy);
            if (!success)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            AUDIT_POLICY_INFORMATION policyInformation = (AUDIT_POLICY_INFORMATION)Marshal.PtrToStructure(auditPolicy, typeof(AUDIT_POLICY_INFORMATION));
            AuditFree(auditPolicy);
            ulong policyType = policyInformation.PolicyType;
            string policyTypeFlags = "";
            //We process possible value flags for AuditQuerySystemPolicy
            //https://docs.microsoft.com/en-us/windows/win32/api/ntsecapi/ns-ntsecapi-audit_policy_information
            if ((policyType & 1) == 1)
            {
                policyTypeFlags += "Sucess ";
            }
            if ((policyType & 2) == 2)
            {
                policyTypeFlags += "Failure ";
            }
            if ((policyType & 4) == 4)
            {
                policyTypeFlags += "None ";
            }
            if(policyTypeFlags=="")
            {
                policyTypeFlags = "None";
            }
            return policyTypeFlags;
        }
    }




}
