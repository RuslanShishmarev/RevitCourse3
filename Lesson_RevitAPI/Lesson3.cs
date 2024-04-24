using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Linq;

namespace Lesson_RevitAPI
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var allWinds = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Windows)
                .WhereElementIsNotElementType()
                .ToElements();

            var allMirrored = allWinds.Where(x => (x as FamilyInstance).Mirrored);

            string markForMirrored = "mirrored";
            string scheduleName = "Table of mirrored windows";

            ViewSchedule schesuleForMirroredWindows = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSchedule))
                .FirstOrDefault(x => x.Name == scheduleName) as ViewSchedule;


            using (var newTrForWnds = new Transaction(doc, "Mark mirrored wnds"))
            {
                newTrForWnds.Start();

                // mark elements
                foreach (Element element in allMirrored)
                {
                    Parameter commentParameter = element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                    commentParameter.Set(markForMirrored);
                }

                if (schesuleForMirroredWindows is null)
                {
                    schesuleForMirroredWindows = CreateSchecule(doc, scheduleName, markForMirrored);
                }
                
                newTrForWnds.Commit();
            }

            commandData.Application.ActiveUIDocument.ActiveView = schesuleForMirroredWindows;

            TaskDialog.Show("Mirrored wnds", $"Count of mirrored windows: {allMirrored.Count()}");

            return Result.Succeeded;
        }

        private ViewSchedule CreateSchecule(Document doc, string scheduleName, string filterMark)
        {
            // create schedule
            var schesuleForMirroredWindows = ViewSchedule.CreateSchedule(
                document: doc,
                categoryId: new ElementId(BuiltInCategory.OST_Windows));

            schesuleForMirroredWindows.Name = scheduleName;

            var allAllowedFileds = schesuleForMirroredWindows.Definition
                .GetSchedulableFields();

            var commentField = allAllowedFileds
                .FirstOrDefault(x => x.ParameterId == new ElementId(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));

            var typeField = allAllowedFileds
                .FirstOrDefault(x => x.GetName(doc) == "Type");

            schesuleForMirroredWindows.Definition.AddField(typeField);
            ScheduleField commentFiled = schesuleForMirroredWindows.Definition.AddField(commentField);

            var filterByComment = new ScheduleFilter(commentFiled.FieldId, ScheduleFilterType.Equal, filterMark);
            schesuleForMirroredWindows.Definition.AddFilter(filterByComment);
            return schesuleForMirroredWindows;
        }
    }
}
