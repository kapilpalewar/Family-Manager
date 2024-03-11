using Autodesk.Revit.DB;
using Smartsheet.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Smartsheet.Api;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;

namespace myFirstPlugin
{
    public class Family_Paths
    {
        private Document document;
        private string accessToken = "4XXHcMUhOBZ3RMDqZApNeacxoKp0n1Cg8m3IN";
        private long sheetId = 2201704728579972;

        public class PathData
        {
            public string LocalPath { get; set; }
            public string Name { get; set; }
            public string EgnytePath { get; set; }
        }

        public List<PathData> GetPathsFromSheet()
        {
            // Create the Smartsheet client
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetAccessToken(accessToken).Build();

            // Get the sheet
            Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);

            // Find the column indices
            int localPathsColumnIndex = -1;
            int nameColumnIndex = -1;
            int egnytePathsColumnIndex = -1;

            foreach (Column column in sheet.Columns)
            {
                if (column.Title == "Local Paths")
                    localPathsColumnIndex = column.Index.Value;

                if (column.Title == "Name")
                    nameColumnIndex = column.Index.Value;

                if (column.Title == "Egnyte Paths")
                    egnytePathsColumnIndex = column.Index.Value;

                // Break the loop if all column indices are found
                if (localPathsColumnIndex != -1 && nameColumnIndex != -1 && egnytePathsColumnIndex != -1)
                    break;
            }

            if (localPathsColumnIndex == -1 || nameColumnIndex == -1 || egnytePathsColumnIndex == -1)
            {
                // One or more columns not found
                return new List<PathData>();
            }

            // Retrieve data from all columns
            List<PathData> paths = new List<PathData>();
            foreach (Row row in sheet.Rows)
            {
                if (row.Cells.Count > localPathsColumnIndex && row.Cells.Count > nameColumnIndex && row.Cells.Count > egnytePathsColumnIndex)
                {
                    Cell localPathsCell = row.Cells[localPathsColumnIndex];
                    Cell nameCell = row.Cells[nameColumnIndex];
                    Cell egnytePathsCell = row.Cells[egnytePathsColumnIndex];

                    if (localPathsCell.Value != null && localPathsCell.Value.ToString() != "")
                    {
                        PathData pathData = new PathData()
                        {
                            LocalPath = localPathsCell.Value.ToString(),
                            Name = nameCell.Value != null ? nameCell.Value.ToString() : "",
                            EgnytePath = egnytePathsCell.Value != null ? egnytePathsCell.Value.ToString() : ""
                        };
                        paths.Add(pathData);
                    }
                }
            }

            return paths;
        }
    }
}
