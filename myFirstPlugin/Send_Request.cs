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
    public partial class Send_Request : System.Windows.Forms.Form
    {
        private string accessToken = "4XXHcMUhOBZ3RMDqZApNeacxoKp0n1Cg8m3IN";
        private long sheetId = 5554046156885892;
        private int localPathsColumnIndex;
        private int nameColumnIndex;
        private int egnytePathsColumnIndex;

        public Send_Request(Autodesk.Revit.UI.ExternalCommandData commandData)
        {
            InitializeComponent();
        }

        private void Send_Request_Load(object sender, EventArgs e)
        {
            

        }   
        private void button1_Click(object sender, EventArgs e)
        {


        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Create the Smartsheet client
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();

            // Get the sheet
            Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

            // Create a new row
            Row newRow = new Row
            {
                ToTop = true, // Add the row at the bottom of the sheet
                Cells = new List<Cell>()
            };

            foreach (Column column in sheet.Columns)
            {
                if (column.Title == "Project Name")
                {
                    // Add cell for "Project Name" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = Project_Name.Text
                    };
                    newRow.Cells.Add(cell);
                }

                if (column.Title == "Project Number")
                {
                    // Add cell for "Project Number" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = Project_Number.Text
                    };
                    newRow.Cells.Add(cell);
                }

                if (column.Title == "Due Date")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = dateTimePicker1.Text
                    };
                    newRow.Cells.Add(cell);
                }
                if (column.Title == "Priority")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = Priority.Text
                    };
                    newRow.Cells.Add(cell);
                }
                if (column.Title == "File Name")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = File_Name.Text
                    };
                    newRow.Cells.Add(cell);
                }
                if (column.Title == "Requested By")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = Your_Name.Text
                    };
                    newRow.Cells.Add(cell);
                }
                if (column.Title == "Task Description")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = Task_description.Text
                    };
                    newRow.Cells.Add(cell);
                }
                if (column.Title == "Status")
                {
                    // Add cell for "Due Date" column
                    Cell cell = new Cell
                    {
                        ColumnId = column.Id,
                        Value = "Pending"
                    };
                    newRow.Cells.Add(cell);
                }
            }

            // Add the new row to the sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(sheetId, new List<Row> { newRow });

            if (addedRows.Count > 0)
            {
                MessageBox.Show("New row added successfully with values.");
            }
            else
            {
                MessageBox.Show("Failed to add the new row.");
            }

        }

        // add new line bellow this 


    }

}
