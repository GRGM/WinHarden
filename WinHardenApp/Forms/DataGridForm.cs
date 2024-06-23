using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinHardenApp.Forms
{
    public partial class DataGridForm : Form
    {
        public DataGridForm(string file)
        {
            InitializeComponent();
            FillDataGridView(file);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FillDataGridView(string fileData)
        {
            DataTable table = GetDataTableFromCSV(fileData);
            csvDataGridView.DataSource = table;
            csvDataGridView.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCells);
        }



        internal static DataTable GetDataTableFromCSV(string fileData)
        {
            DataTable dataTable = new DataTable();
            FileStream inputFileStream = new FileStream(fileData, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            string headers = "";
            while (!inputFile.EndOfStream)
            {
                headers = inputFile.ReadLine();
                //We read lines until finding a line with headers
                if (headers.Trim() != "")
                {
                    break;
                }
            }
            string[] separator = new string[1] { @"," };
            string[] fields = headers.Split(separator,StringSplitOptions.None);
            foreach (string field in fields)
            {
                string fieldClean=field.Replace(@"""","");
                dataTable.Columns.Add(fieldClean, System.Type.GetType("System.String"));
            }

            string data = null;

            while ((data = inputFile.ReadLine()) != null)
            {
                if (data.Trim() == "")
                {
                    break;
                }
                char[] separatorChar = new char[] { '"' };
                string[] stringFields = data.Split(separatorChar);
                int lenght = stringFields.Length;
                //If there are no quotes, we process the line
                if (lenght == 1)
                {
                    AddRow(dataTable, data);
                    continue;
                }

                string dataConverted = "";
                for (int i = 0; i < lenght; i++)
                {
                    if (i % 2 == 0)
                    {
                        dataConverted += stringFields[i];
                    }
                    else
                    {
                        //We remove internal commas
                        dataConverted += stringFields[i].Replace(",", "");
                    }
                }
                AddRow(dataTable, dataConverted);

            }
            inputFile.Close();
            return dataTable;
        }

        private static void AddRow(DataTable dataTable,string line)
        {
            DataRow row = dataTable.NewRow();
            string[] separator = new string[1] { @"," };
            string[] fieldsRow = line.Split(separator, StringSplitOptions.None);
            int i = 0;
            foreach (string fieldRow in fieldsRow)
            {
                row[i] = fieldRow;
                i++;
            }
            dataTable.Rows.Add(row);
        }

    }
}
