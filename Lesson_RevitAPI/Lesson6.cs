using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Lesson_RevitAPI.ViewModels;
using Lesson_RevitAPI.Views;

namespace Lesson_RevitAPI
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson6 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            var vm = new RevitElementsMVVM_ViewModel(doc, new RevitElementsMVVM_View());

            vm.ShowDialog();

            return Result.Succeeded;
        }
    }
}
