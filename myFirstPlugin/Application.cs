using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace myFirstPlugin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a split button
            SplitButtonData splitButtonData = new SplitButtonData("MySplitButton", "My Split Button");
            SplitButton splitButton = panel.AddItem(splitButtonData) as SplitButton;

            // Add buttons to the split button
            PushButtonData buttonData1 = new PushButtonData("FirstPlugin", "FirstPlugin", thisAssemblyPath, "myFirstPlugin.Command");
            PushButton button1 = splitButton.AddPushButton(buttonData1) as PushButton;
            button1.ToolTip = "Base-4";
            //Uri button1IconUri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "Base-44.ico"));
            //BitmapImage button1IconImage = new BitmapImage(button1IconUri);
            //button1.LargeImage = button1IconImage;

            PushButtonData buttonData2 = new PushButtonData("SendRequest", "Send Request", thisAssemblyPath, "myFirstPlugin.SendRequestCommand");
            PushButton button2 = splitButton.AddPushButton(buttonData2) as PushButton;
            button2.ToolTip = "Send a request";
            //Uri button2IconUri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "SendRequest.ico"));
            //BitmapImage button2IconImage = new BitmapImage(button2IconUri);
            //button2.LargeImage = button2IconImage;

            PushButtonData buttonData3 = new PushButtonData("ShowRequest", "Show Request", thisAssemblyPath, "myFirstPlugin.ShowRequestCommand");
            PushButton button3 = splitButton.AddPushButton(buttonData3) as PushButton;
            button3.ToolTip = "Send a request";
            //Uri button2IconUri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "SendRequest.ico"));
            //BitmapImage button2IconImage = new BitmapImage(button2IconUri);
            //button2.LargeImage = button2IconImage;



            return Result.Succeeded;
        }

        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            string tab = "Family Manager";
            RibbonPanel ribbonPanel = null;

            try
            {
                a.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {
                ribbonPanel = a.CreateRibbonPanel(tab, "Family Management");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return ribbonPanel;
        }
    }
}
