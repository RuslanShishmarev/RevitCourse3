using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Lesson_RevitAPI.Views;

using System;

namespace Lesson_RevitAPI
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson5 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            var wnd = new RevitDataWindow();
            var allWallsTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType));

            wnd.AllElements.ItemsSource = allWallsTypes;

            wnd.AllElements.SelectionChanged += (s, e) =>
            {
                WallType selectedEl = wnd.AllElements.SelectedItem as WallType;
                const string materialNameDefault = "Не известно";

                var allLayers = selectedEl.GetCompoundStructure()?.GetLayers();
                if (allLayers is null)
                {
                    wnd.SelectedInfo.Text = materialNameDefault;
                    return;
                }

                string allInfo = "";
                for (int i = 0; i < allLayers.Count; i++)
                {
                    var layer = allLayers[i];
                    string materialName = materialNameDefault;
                    if (layer.MaterialId != null && layer.MaterialId != ElementId.InvalidElementId)
                    {
                        materialName = doc.GetElement(layer.MaterialId).Name;
                    }

                    double layWidth = Math.Round(layer.Width * 304.8);
                    allInfo += $"{i + 1}. {materialName} {layWidth} мм\n";
                }

                wnd.SelectedInfo.Text = allInfo;
            };
            wnd.ShowDialog();

            return Result.Succeeded;
        }
    }
}
