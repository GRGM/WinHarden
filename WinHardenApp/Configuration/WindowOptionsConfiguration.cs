using System;
using System.IO;


namespace WinHardenApp.Configuration
{
    internal class WindowOptionsConfiguration
    {
        private bool m_hideEmptyFiles;
        private bool m_colorFiles;
        private bool m_colorEveryOne;
        internal WindowOptionsConfiguration(bool hideEmptyFiles, bool colorFiles, bool colorEveryOne)
        {
            m_hideEmptyFiles = hideEmptyFiles;
            m_colorFiles = colorFiles;
            m_colorEveryOne = colorEveryOne;
        }



        internal static WindowOptionsConfiguration GetInitialWindowOptionsConfiguration()
        {
            WindowOptionsConfiguration windowOptionsConfiguration = new WindowOptionsConfiguration(false,false,false);
            return windowOptionsConfiguration;
        }

        internal string GetWindowOptionsConfigurationString()
        {
            string windowOptionsConfigurationString = "HIDE_EMPTY_FILES=" + m_hideEmptyFiles + Environment.NewLine;
            windowOptionsConfigurationString += "COLOR_FILES=" + m_colorFiles + Environment.NewLine;
            windowOptionsConfigurationString += "COLOR_EVERYONE=" + m_colorFiles + Environment.NewLine;
            return windowOptionsConfigurationString;
        }


        internal static WindowOptionsConfiguration GetWindowOptionsConfiguration(StreamReader inputFile)
        {
            bool hideEmptyFiles = StringUtils.GetBoolNextLine(inputFile, "HIDE_EMPTY_FILES");
            bool colorFiles = StringUtils.GetBoolNextLine(inputFile, "COLOR_FILES");
            bool colorEveryone = StringUtils.GetBoolNextLine(inputFile, "COLOR_EVERYONE");

            WindowOptionsConfiguration windowOptionsConfiguration = new WindowOptionsConfiguration(hideEmptyFiles, colorFiles, colorEveryone);
            return windowOptionsConfiguration;
        }


        internal bool HideEmptyFiles
        {
            get
            {
                return m_hideEmptyFiles;
            }
        }
        internal bool ColorFiles
        {
            get
            {
                return m_colorFiles;
            }
        }
        internal bool ColorEveryone
        {
            get
            {
                return m_colorEveryOne;
            }
        }
    }
}
