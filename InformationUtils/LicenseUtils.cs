using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    internal class LicenseUtils
    {
        private string m_extractionBaseFolder;


        public LicenseUtils(string extractionBaseFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
        }
        internal void GetLicenseStatus()
        {
            string outputFilename = m_extractionBaseFolder + @"\LicenseStatus.txt";
            using (StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM SoftwareLicensingProduct");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    foreach(PropertyData propertyData in obj.Properties)
                    {
                        outputFile.WriteLine(propertyData.Name+" = " + propertyData.Value);
                    }
                }
            }
        }
    }
}
