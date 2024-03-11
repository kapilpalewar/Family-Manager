using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Smartsheet.Api.Models;
using Smartsheet.Api;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using System.Drawing.Drawing2D;

namespace myFirstPlugin
{
    public partial class ShowRequest : System.Windows.Forms.Form
    {
        private string accessToken = "4XXHcMUhOBZ3RMDqZApNeacxoKp0n1Cg8m3IN";
        private long sheetId = 5554046156885892;
        private SmartsheetClient smartsheet;

        public ShowRequest(Autodesk.Revit.UI.ExternalCommandData commandData)
        {
            InitializeComponent();
            smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
            LoadSheetData();
        }

        private void ShowRequest_Load(object sender, EventArgs e)
        {

        }

        private void LoadSheetData()
        {
            Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);
            if (sheet == null)
            {
                MessageBox.Show("Sheet is null.", "Error");
                return;
            }

            IList<Column> columns = sheet.Columns;
            IList<Row> rows = sheet.Rows;

            UpdateDataGridView(columns, rows);
        }

        private void UpdateDataGridView(IList<Column> columns, IList<Row> rows)
        {
            dataGridView1.Columns.Clear();

            // Append column names to the DataGridView
            foreach (Column column in columns)
            {
                dataGridView1.Columns.Add(column.Title, column.Title);
            }

            // Loop through the rows and append the data to the DataGridView
            foreach (Row row in rows)
            {
                object[] rowValues = new object[columns.Count];
                int i = 0;
                foreach (Cell cell in row.Cells)
                {
                    rowValues[i] = cell.Value;
                    i++;
                }
                dataGridView1.Rows.Add(rowValues);
            }
        }
    }
}
