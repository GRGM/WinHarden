using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace InformationUtils
{
    public class ProfileAccountUtils
    {
        private string m_userProfileFolder = null;

        private ProfileAccountUtils(string userProfileFolder)
        {
            m_userProfileFolder = userProfileFolder;
        }

        public static ProfileAccountUtils GetProfileAccountUtils(string inputFileName)
        {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);


            string userProfileFolder = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line.StartsWith("USERPROFILE"))
                {
                    int slashPoistion = line.LastIndexOf(@"\");
                    userProfileFolder = line.Substring(0, slashPoistion);
                    int startPosition = userProfileFolder.IndexOf("=");
                    userProfileFolder = userProfileFolder.Substring(startPosition + 1);
                    break;
                }

            }
            inputFile.Close();
            ProfileAccountUtils profileAccountUtils = new ProfileAccountUtils(userProfileFolder);
            return profileAccountUtils;
        }

        public bool IsUserProfileFolderSelfAccessed(string objectToReview, string line)
        {
            int separator = line.IndexOf(@"\");
            if (separator < 0)
            {
                separator = line.IndexOf(@":");
            }
            string user = line.Substring(separator + 1).Trim();
            string startingFolder= m_userProfileFolder + @"\" + user;
            if (objectToReview.ToUpper().StartsWith(startingFolder.ToUpper()))
            { 
                return true;
            }
            return false;
        }
    }
}
