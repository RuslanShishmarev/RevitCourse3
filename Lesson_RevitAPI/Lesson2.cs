using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lesson_RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Lesson2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            string projectName = doc.Title;
            TaskDialog.Show("Project name", projectName);

            // get selected elements
            var selectedIds = uidoc.Selection.GetElementIds();
            var selectedElement = selectedIds.Select(x => doc.GetElement(x));
            var selectedWalls = selectedElement.Where(x => x is Wall);
            int wallCount = selectedWalls.Count();
            TaskDialog.Show("All names of selected elements", wallCount.ToString());

            string allNames = string.Join("\n", selectedElement.Select(x => x.Name));

            TaskDialog.Show("All names of selected elements", allNames);

            var collector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls);

            var allWalls = collector.Where(x => x is Wall).ToList();
            var allWallsTypes = collector.Where(x => x is WallType).ToList();

            var allWallsAsList = new List<Wall>();
            var allWallsTypesAsList = new List<WallType>();

            foreach (var wallOrType in collector)
            {
                if (wallOrType is Wall wall) allWallsAsList.Add(wall);
                if (wallOrType is WallType wallType) allWallsTypesAsList.Add(wallType);
            }

            return Result.Succeeded;
        }
    }
}
