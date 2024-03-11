using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace myFirstPlugin
{
    public class ImportFamilies
    {
        public void ImportAllFamilies(string filePath, Document doc)
        {
            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Import Families");

                Family family = null;
                doc.LoadFamily(filePath, out family);
                Debug.Print(filePath);

               // MessageBox.Show("Family has been loaded in project");

                transaction.Commit();
            }
        }


    }
}
