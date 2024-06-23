using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinHardenApp.Configuration;

namespace WinHardenApp.Forms
{
    public partial class WindowOptionsForm : Form
    {
        private WinHardenForm m_winHardenForm;

        public WindowOptionsForm(WinHardenForm winHardenForm )
        {
            InitializeComponent();
            WindowOptionsConfiguration configuration = WinHardenConfiguration.Configuration.WindowOptionsConfiguration;
            hideEmptyFilesCheckBox.Checked = configuration.HideEmptyFiles;
            colorFilesCheckBox.Checked = configuration.ColorFiles;
            colorEveryoneCheckBox.Checked= configuration.ColorEveryone;
            m_winHardenForm =winHardenForm;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            WindowOptionsConfiguration windowOptionsConfiguration = new WindowOptionsConfiguration(hideEmptyFilesCheckBox.Checked,colorFilesCheckBox.Checked, colorEveryoneCheckBox.Checked);
            WinHardenConfiguration.UpdateWindowOptionsConfiguration(windowOptionsConfiguration);
            m_winHardenForm.UpdateFullTreeView();
            Close();
        }
    }
}
