using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinHardenApp.AnalyzeInformationUtils;

namespace WinHardenApp.Forms
{
    public partial class FindForm : Form
    {
        private SearchUtils m_searchUtils;
        private WinHardenForm m_winHardenForm;
        public enum SearchType
        {
            CurrentFile,             
            AllOpenFiles,                      
            AllWorkingFiles,                       
        }

        public FindForm(SearchType searchType, WinHardenForm winHardenForm)
        {
            InitializeComponent();
            switch (searchType)
            {
                case SearchType.CurrentFile:
                    openFileRadioButton.Checked = true;
                    break;
                case SearchType.AllOpenFiles:
                    allOpenFileRadioButton.Checked = true; 
                    break;
                case SearchType.AllWorkingFiles:
                    allWorkingFoldersFilesRadioButton.Checked = true;
                    break;
            }
            m_searchUtils = new SearchUtils(winHardenForm.SearchTreeView);
            m_winHardenForm = winHardenForm;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            string searchText= searchTextBox.Text.Trim();  
            if (openFileRadioButton.Checked)
            {
                TabPage selectedTabPage = m_winHardenForm.FilesTabControl.SelectedTab;
                m_searchUtils.SearchCurrentFileTreeView(searchText, selectedTabPage);
                return;
            }
            if (allOpenFileRadioButton.Checked)
            {
                m_searchUtils.SearchAllOpenFilesTreeView(searchText, m_winHardenForm.FilesTabControl);
                return;
            }
            if (allWorkingFoldersFilesRadioButton.Checked)
            {
                m_searchUtils.SearchAllWorkingFoldersFilesTreeView(searchText, m_winHardenForm.FilesTreeView);
                return;
            }
             
        }
    }
}
