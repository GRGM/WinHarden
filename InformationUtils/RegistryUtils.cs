using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using InformationUtils.Properties;

namespace InformationUtils
{
    internal enum RegistryFormatType
    {
        OnlyName,
        OnlyKey,
        NameAndKey

    }
    public class RegistryUtils
    {
        private string m_inputFolder;

        public RegistryUtils(string inputFolder)
        {
            m_inputFolder = inputFolder;
        }


        internal static void WriteHKLMKeyValuesToFile(string outputFilename,string key, bool append, RegistryFormatType registryFormatType)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            StreamWriter outputFile = new StreamWriter(outputFilename, append, Encoding.Default);
            if (registryKey is null)
            {
                outputFile.WriteLine("Key not found in local machine hive: " + key);
            }
            else
            {
                WriteHKeyValuesToFile(outputFile, registryKey, registryFormatType);
            }
            outputFile.Close();
        }
        internal static void WriteHKCUKeyValuesToFile(string outputFilename, string key, bool append, RegistryFormatType registryFormatType)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(key);
            StreamWriter outputFile = new StreamWriter(outputFilename, append, Encoding.Default);
            if (registryKey is null)
            {
                outputFile.WriteLine("Key not found in current user hive: " + key);
            }
            else
            {
                WriteHKeyValuesToFile(outputFile, registryKey, registryFormatType);
            }
            outputFile.Close();
        }
        internal static void WriteFullHKLMValuesToFile(string outputFilename, string key, bool append, RegistryFormatType registryFormatType)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            StreamWriter outputFile = new StreamWriter(outputFilename, append, Encoding.Default);
            if (registryKey is null)
            {
                outputFile.WriteLine("Key not found in current local machine hive: " + key);
            }
            else
            {
                //We go through child in a recursive function.
                WriteFullHKLMValuesToFile(outputFile, registryKey, registryFormatType);
            }
            outputFile.Close();
        }

        private static void WriteFullHKLMValuesToFile(StreamWriter outputFile, RegistryKey key, RegistryFormatType registryFormatType)
        {
            WriteHKeyValuesToFile(outputFile, key, registryFormatType);
            //We go through child in a recursive function.
            string[] servicesList = key.GetSubKeyNames();
            foreach (string subKey in servicesList)
            {
                RegistryKey subkey = key.OpenSubKey(subKey);
                WriteFullHKLMValuesToFile(outputFile, subkey, registryFormatType);
            }
        }


        private static void WriteHKeyValuesToFile(StreamWriter outputFile, RegistryKey registryKey, RegistryFormatType registryFormatType)
        {
            outputFile.WriteLine("Registry key:" + registryKey.Name);
            foreach (string valueName in registryKey.GetValueNames())
            {
                WritetRegistryValue(outputFile,registryKey, valueName, registryFormatType);
            }
            outputFile.WriteLine();
        }

        private static void WritetRegistryValue(StreamWriter outputFile,RegistryKey registryKey, string valueName, RegistryFormatType registryFormatType)
        {
            string valueKey = GetRegistryValue(registryKey, valueName);
            switch (registryFormatType)
            {
                case RegistryFormatType.OnlyName:
                    outputFile.WriteLine(valueName);
                    break;
                case RegistryFormatType.OnlyKey:
                    outputFile.WriteLine(valueKey);
                    break;
                case RegistryFormatType.NameAndKey:
                    outputFile.WriteLine(valueName + " = " + valueKey);
                    break;
            }
        }

        private static string GetRegistryValue(RegistryKey registryKey,string valueName)
        {
            RegistryValueKind valueKind = registryKey.GetValueKind(valueName);
            string valueKey = "";
            //https://docs.microsoft.com/es-es/dotnet/api/microsoft.win32.registryvaluekind?view=net-6.0
            switch (valueKind)
            {
                case RegistryValueKind.String:
                    valueKey = (string)registryKey.GetValue(valueName);
                    break;
                case RegistryValueKind.ExpandString:
                    valueKey = (string)registryKey.GetValue(valueName);
                    break;
                case RegistryValueKind.Binary:
                    //https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa
                    StringBuilder hex = new StringBuilder();
                    byte[] valueByteKey = (byte[])registryKey.GetValue(valueName);
                    foreach (byte b in (byte[])valueByteKey)
                    {
                        hex.AppendFormat("{0:x2} ", b);
                    }
                    valueKey = hex.ToString();
                    break;
                case RegistryValueKind.DWord:
                    int valueIntKey = (int)registryKey.GetValue(valueName);
                    valueKey = valueIntKey.ToString();
                    break;
                case RegistryValueKind.QWord:
                    Int64 valueInt64Key = (Int64)registryKey.GetValue(valueName);
                    valueKey = valueInt64Key.ToString();
                    break;
                case RegistryValueKind.MultiString:
                    string[] valueStringKey = (string[])registryKey.GetValue(valueName);
                    StringBuilder stringAppend = new StringBuilder();
                    foreach (string s in (string[])valueStringKey)
                    {
                        stringAppend.AppendFormat("[{0:s}], ", s);
                    }
                    valueKey = stringAppend.ToString();
                    break;
                default:
                    valueKey = ("Unknown");
                    break;
            }
            //So we simplify generetad output files where several text lines are included in the value
            valueKey = valueKey.Replace(Environment.NewLine, " ");
            return valueKey;
        }


        private static StreamReader GetStreamReaderRegistryPolicy(string registryPolicyFile)
        {
            StreamReader inputFile = null;
            if (registryPolicyFile is null || registryPolicyFile.Trim() == "")
            {
                inputFile = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resources.Registry_Policy)));

            }
            else
            {
                FileStream inputFileStream = new FileStream(registryPolicyFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                inputFile = new StreamReader(inputFileStream, Encoding.Default);
            }
            return inputFile;
        }

        internal static void WriteRegistryPolicy(string registryPolicyFile, string outputFilename)
        {
            StreamReader inputFile = GetStreamReaderRegistryPolicy(registryPolicyFile);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);


            RegistryKey registryKey = null;
            string registryKeyLine = null;

            while (!inputFile.EndOfStream)
            {

                string line = inputFile.ReadLine().Trim();
                if (line.Trim() == "")
                {
                    continue;
                }

                if (line.StartsWith("Registry key:HKEY_"))
                {
                    outputFile.WriteLine();
                    outputFile.WriteLine(line);
                    registryKey = GetRegistryKey(line);
                    registryKeyLine = line;
                    continue;
                }

                //We are using as inputFile the same registry policy file that we will use to compare the values during the analysis phase.
                //So, this file contains lines with value names and corresponding value

                if (registryKey is null)
                {
                    outputFile.WriteLine(registryKeyLine + " was not found in registry policy file. It is not possible to obtain value: " + line);
                    continue;
                }

                int equalPos = line.IndexOf('=');
                string valueName = line.Substring(0, equalPos).Trim();
                try
                {
                    WritetRegistryValue(outputFile, registryKey, valueName, RegistryFormatType.NameAndKey);
                }
                catch
                {
                    outputFile.WriteLine(valueName + " value is not defined on this registry key.");
                }
            }
            inputFile.Close();
            outputFile.Close();
        }

        private static RegistryKey GetRegistryKey(string registryKey)
        {

            registryKey = registryKey.Replace("Registry key:", "");
            if(registryKey.StartsWith("HKEY_LOCAL_MACHINE"))
            {
                registryKey=registryKey.Replace(@"HKEY_LOCAL_MACHINE\", "");
                return Registry.LocalMachine.OpenSubKey(registryKey);
            }
            if (registryKey.StartsWith("HKEY_CURRENT_USER"))
            {
                registryKey = registryKey.Replace(@"HKEY_CURRENT_USER\", "");
                return Registry.LocalMachine.OpenSubKey(registryKey);
            }
            return null;


        }


        public static void SaveDefaultRegistryPolicyFile(string fileName)
        {
            StreamWriter configurationtFile = new StreamWriter(fileName, false, Encoding.Default);
            string line;
            using (StreamReader ms = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resources.Registry_Policy))))
            {
                while ((line = ms.ReadLine()) != null)
                {
                    configurationtFile.WriteLine(line);
                }
            }
            configurationtFile.Close();
        }

        public static void CompareRegistryPolicyFiles(string inputFileName, string registryKeyPolicy, string outputFilename)
       {
            StreamReader registrytFile = GetStreamReaderRegistryPolicy(registryKeyPolicy);
            string[] inputLines = File.ReadAllLines(inputFileName);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            while (!registrytFile.EndOfStream)
            {
                string line = registrytFile.ReadLine();
                CompareRegistryPolicy(line, inputLines, outputFile);
            }
            outputFile.Close();
            registrytFile.Close ();

        }
        private static void CompareRegistryPolicy(string registryPolicyLine, string[] inputLines, StreamWriter outputFile)
        {
            //We ignore lines wihtout = as names of the registry key
            int pos = registryPolicyLine.IndexOf("=");
            if (pos < 0)
            {
                return;
            }
            string searchToken = registryPolicyLine.Substring(0, pos).Trim();
            foreach (string inputLine in inputLines)
            {
                if (inputLine.StartsWith(searchToken))
                {
                    if (registryPolicyLine.Trim().ToUpper() != inputLine.Trim().ToUpper())
                    {
                        outputFile.WriteLine("EXPECTED: " + registryPolicyLine);
                        outputFile.WriteLine("FOUND: " + inputLine);
                        outputFile.WriteLine();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            outputFile.WriteLine("EXPECTED: " + registryPolicyLine);
            outputFile.WriteLine("Not found in extracted registry policy.");
            outputFile.WriteLine();
        }


    }
}
