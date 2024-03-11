using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Autodesk.Revit.UI.Selection;
namespace myFirstPlugin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ShowRequestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Show the form 
            ShowRequest showrequestform = new ShowRequest(commandData);
            showrequestform.ShowDialog();

            return Result.Succeeded;
        }
    }
}
