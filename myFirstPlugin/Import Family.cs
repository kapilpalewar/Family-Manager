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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Diagnostics;
using Control = System.Windows.Forms.Control;

namespace myFirstPlugin
{    

    public partial class Import_Family : System.Windows.Forms.Form
    {
        private UIApplication uiapp;
        private UIDocument uidoc;
        private Autodesk.Revit.ApplicationServices.Application app;
        private Document doc;
        public string pathvalue;
        public string myList;

        public Import_Family(ExternalCommandData commandData)
        {
            InitializeComponent();
            uiapp = commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;

        }
        private void Import_Family_Load(object sender, EventArgs e)
        {
            // Hide progress baar
            progressBar1.Visible = false;

            // Clear check list box
            checkedListBox1.Items.Clear();

            // Get paths from Smartsheet
            Family_Paths familyPaths = new Family_Paths();
            List<Family_Paths.PathData> smartsheetPaths = familyPaths.GetPathsFromSheet();

            // Add Name values to checkedListBox1
            foreach (Family_Paths.PathData pathData in smartsheetPaths)
            {
                checkedListBox1.Items.Add(pathData.Name);
            }



            // Add Family_Categories to comboBox1
            foreach (string category in Family_Categories)
            {
                comboBox1.Items.Add(category);
            }


            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // Initialize search functionality
            textBox1.TextChanged += searchTextBox_TextChanged;
        }
        
        // Search Functionality 
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBox1.Text.ToLower();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string fileName = row.Cells[1].Value.ToString().ToLower();
                row.Visible = fileName.Contains(searchQuery);
            }
        }

        // Sort by Categpries 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBox1.SelectedItem.ToString();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string category = row.Cells[2].Value.ToString();

                if (selectedCategory == "All Selected" || category == selectedCategory)
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }

        // Create a list of all Family Categories
        List<string> Family_Categories = new List<string>()
        {
            "All Selected",
            "Piers",
            "MEP Fabrication Hangers",
            "Ramps",
            "Cable Tray Fittings",
            "Structural Connections",
            "Planting",
            "Mass",
            "Air Terminals",
            "Communication Devices",
            "Piping Systems",
            "MEP Fabrication Containment",
            "Plumbing Fixtures",
            "Ceilings",
            "MEP Fabrication Ductwork",
            "Bridge Framing",
            "Conduit Fittings",
            "Sprinklers",
            "Doors",
            "Lighting Devices",
            "Curtain Systems",
            "Parking",
            "Ducts",
            "Bridge Cables",
            "Imports in Families",
            "Conduits",
            "Flex Pipes",
            "Structural Trusses",
            "HVAC Zones",
            "Medical Equipment",
            "Site",
            "Duct Systems",
            "Fire Protection",
            "Abutments",
            "Duct Placeholders",
            "Duct Accessories",
            "Furniture Systems",
            "Telephone Devices",
            "Lines",
            "Wires",
            "Pipes",
            "Audio Visual Devices",
            "Signage",
            "Topography",
            "Pipe Insulations",
            "Flex Ducts",
            "Structural Area Reinforcement",
            "Structural Framing",
            "Electrical Fixtures",
            "Data Devices",
            "Lighting Fixtures",
            "Duct Insulations",
            "Generic Models",
            "Electrical Equipment",
            "Curtain Panels",
            "Fire Alarm Devices",
            "Temporary Structures",
            "Roads",
            "Floors",
            "Windows",
            "Structural Path Reinforcement",
            "Parts",
            "Columns",
            "Bridge Decks",
            "Vertical Circulation",
            "Structural Fabric Reinforcement",
            "Vibration Management",
            "Curtain Wall Mullions",
            "Walls",
            "Pipe Fittings",
            "Structural Columns",
            "Pipe Placeholders",
            "Structural Rebar Couplers",
            "Cable Trays",
            "Structural Stiffeners",
            "Entourage",
            "MEP Fabrication Pipework",
            "Nurse Call Devices",
            "Areas",
            "Roofs",
            "Structural Fabric Areas",
            "Structural Rebar",
            "Shaft Openings",
            "Duct Fittings",
            "Specialty Equipment",
            "Pipe Accessories",
            "Expansion Joints",
            "Structural Foundations",
            "Security Devices",
            "Food Service Equipment",
            "Bearings",
            "Railings",
            "Duct Linings",
            "Structural Beam Systems",
            "Casework",
            "Mechanical Equipment",
            "Structural Tendons",
            "Furniture",
            "Rooms",
            "Hardscape",
            "Stairs",
            "Detail Items",
            "Spaces",
            "Generic Annotations"
        };


        private string lastSelectedPath; // Store the last selected path

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            // Create List for selected paths
            List<string> familyPaths = new List<string>();
            foreach (string s in checkedListBox1.CheckedItems)
            {
                familyPaths.Add(s);
            }

            // Create loop to get files and folders inside each path
            foreach (string familyPath in familyPaths)
            {
                // Get paths from Smartsheet
                Family_Paths familyPaths1 = new Family_Paths();
                List<Family_Paths.PathData> smartsheetPaths = familyPaths1.GetPathsFromSheet();

                // Find the corresponding PathData object based on the family name
                Family_Paths.PathData selectedPathData = smartsheetPaths.FirstOrDefault(p => p.Name == familyPath);

                if (selectedPathData != null)
                {
                    string localPath = selectedPathData.LocalPath;
                    string egnytePath = selectedPathData.EgnytePath;

                    string selectedPath = localPath;

                    // Check if the Local Path exists in the system; if not, use the Egnyte Path
                    if (!Directory.Exists(localPath))
                    {
                        selectedPath = egnytePath;
                    }
                    lastSelectedPath = selectedPath;

                    // Retrieve files and folders inside the selected path
                    string[] entries = Directory.GetFileSystemEntries(selectedPath);
                    foreach (string entry in entries)
                    {
                        if (File.Exists(entry) && Path.GetExtension(entry) == ".rfa")
                        {
                            // Process file
                            string fileName = Path.GetFileName(entry);
                            string filePath = entry; // Get the file path

                            Icon icon = Icon.ExtractAssociatedIcon(entry);
                            Image image = icon.ToBitmap();

                            // Resize the image to 32x32 pixels
                            Image previewImage = ResizeImage(image, 32, 32);

                            // Read the .rfa file as text and extract the category
                            string category = ExtractCategoryFromRFA(filePath);

                            int rowIndex = dataGridView1.Rows.Add();
                            DataGridViewRow row = dataGridView1.Rows[rowIndex];
                            row.Cells[0].Value = previewImage; // Set the resized image in the first column
                            row.Cells[1].Value = fileName; // Set the file name in the second column
                            row.Cells[3].Value = filePath; // Set the file path in the third column
                            row.Cells[2].Value = category; // Set the category in the fourth column
                        }
                        else if (Directory.Exists(entry))
                        {
                            // Process folder
                            string folderName = Path.GetFileName(entry);
                            string folderPath = entry; // Get the folder path

                            Icon folderIcon = new Icon("C:\\Users\\kapilp\\AppData\\Roaming\\Autodesk\\Revit\\Addins\\2022\\Resources\\folder.ico"); // Replace "folder_icon.ico" with the path to your folder icon file

                            // Resize the folder icon to 32x32 pixels
                            Image folderImage = folderIcon.ToBitmap();
                            Image previewImage = ResizeImage(folderImage, 32, 32);

                            int rowIndex = dataGridView1.Rows.Add();
                            DataGridViewRow row = dataGridView1.Rows[rowIndex];
                            row.Cells[0].Value = previewImage; // Set the resized folder icon in the first column
                            row.Cells[1].Value = folderName; // Set the folder name in the second column
                            row.Cells[3].Value = folderPath; // Set the folder path in the third column
                            row.Cells[2].Value = ""; // Leave the category column empty for folders
                        }
                    }
                }
            }
        }

        private string ExtractCategoryFromRFA(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                foreach (string category in Family_Categories)
                {
                    if (line.Contains(category))
                    {
                        return category;
                    }
                }
            }

            return "Unknown Category";
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the double-clicked cell belongs to a valid row
            if (e.RowIndex >= 0)
            {
                // Check if the row represents a file (with .rfa extension)
                string filePath = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                if (File.Exists(filePath))
                {
                    // Process the file
                    // Add your logic here for handling the double-click on a file
                    // For example, you can open or process the file
                    //MessageBox.Show("Double-clicked on a file: " + filePath);

                    ImportFamilies importer = new ImportFamilies();
                    importer.ImportAllFamilies(filePath, doc);
                }
                else
                {
                    // Process the folder
                    PopulateFolderItems(filePath);
                }
            }
        } 

        private void PopulateFolderItems(string folderPath)
        {
            currentFolderPath = folderPath; // Store the current folder path

            dataGridView1.Rows.Clear();

            try
            {
                // Get all files and folders inside the selected folder
                string[] entries = Directory.GetFileSystemEntries(folderPath);
                foreach (string entry in entries)
                {
                    if (File.Exists(entry) && Path.GetExtension(entry) == ".rfa")
                    {
                        // Process file
                        string fileName = Path.GetFileName(entry);
                        string filePath = entry; // Get the file path

                        Icon icon = Icon.ExtractAssociatedIcon(entry);
                        Image image = icon.ToBitmap();

                        // Resize the image to 32x32 pixels
                        Image filePreviewImage = ResizeImage(image, 32, 32);

                        // Read the .rfa file as text and extract the category
                        string category = ExtractCategoryFromRFA(filePath);

                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];
                        row.Cells[0].Value = filePreviewImage; // Set the resized image in the first column
                        row.Cells[1].Value = fileName; // Set the file name in the second column
                        row.Cells[3].Value = filePath; // Set the file path in the third column
                        row.Cells[2].Value = category; // Set the category in the fourth column
                    }
                    else if (Directory.Exists(entry))
                    {
                        // Process subfolder
                        string subfolderName = Path.GetFileName(entry);

                        Icon folderIcon = new Icon("C:\\Users\\kapilp\\AppData\\Roaming\\Autodesk\\Revit\\Addins\\2022\\Resources\\folder.ico"); // Replace "folder_icon.ico" with the path to your folder icon file

                        // Resize the folder icon to 32x32 pixels
                        Image folderImage = folderIcon.ToBitmap();
                        Image previewImage = ResizeImage(folderImage, 32, 32);

                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];
                        row.Cells[0].Value = previewImage; // Set the resized folder icon in the first column
                        row.Cells[1].Value = subfolderName; // Set the folder name in the second column
                        row.Cells[3].Value = entry; // Set the subfolder path in the third column
                        row.Cells[2].Value = ""; // Leave the category column empty for folders
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions when accessing the folder
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Resize an image to the specified width and height
        private Image ResizeImage(Image image, int width, int height)
        {
            Image resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        private string currentFolderPath; // Declare a variable to store the current folder path
        //Go Back button
        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFolderPath) && currentFolderPath != lastSelectedPath)
            {
                string parentFolderPath = Path.GetDirectoryName(currentFolderPath);
                if (Directory.Exists(parentFolderPath))
                {
                    PopulateFolderItems(parentFolderPath);
                    currentFolderPath = parentFolderPath;
                }
            }
        }

        // Import Family
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            List<string> selectedPaths = new List<string>();

            // Check if any rows are selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Disable the button while importing to prevent multiple imports
                button1.Enabled = false;

                // Set the maximum value of the progress bar
                progressBar1.Maximum = dataGridView1.SelectedRows.Count;

                // Iterate through the selected rows
                foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                {
                    // Check if the selected row represents an .rfa file
                    string filePath = selectedRow.Cells[3].Value.ToString();
                    if (File.Exists(filePath) && Path.GetExtension(filePath) == ".rfa")
                    {
                        selectedPaths.Add(filePath); // Add the path to the list of selected paths
                    }
                }

                // Check if any .rfa files are selected
                if (selectedPaths.Count > 0)
                {
                    // Perform the desired action with the selected .rfa file paths
                    // For example, you can access the paths using the selectedPaths list
                    foreach (string selectedPath in selectedPaths)
                    {
                        //MessageBox.Show("Selected .rfa file: " + selectedPath);
                        // Update the progress bar value
                        progressBar1.Value++;
                        ImportFamilies importer = new ImportFamilies();
                        importer.ImportAllFamilies(selectedPath, doc);
                    }
                    // Reset the progress bar value and enable the button
                    progressBar1.Value = 0;
                    button1.Enabled = true;
                    progressBar1.Visible= false;
                }
                else
                {
                    MessageBox.Show("No .rfa files selected.");
                }
            }
            else
            {
                MessageBox.Show("No rows selected.");
            }
        }





        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
    }
}
