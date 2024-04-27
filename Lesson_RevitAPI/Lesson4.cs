using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using System;
using System.Linq;

namespace Lesson_RevitAPI
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson4 : IExternalCommand
    {
        private readonly string _markFinished = "finished";
        private readonly string _messageIfFinished = "Already with finish";
        private readonly string _finishWallTypeName = "Finish_30";
        private readonly string _transactionName = "create finish";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            // select room element
            Reference selectedRef= uidoc.Selection.PickObject(ObjectType.Element, new ElementTypeFilter<Room>());
            Room selectedRoom = doc.GetElement(selectedRef) as Room;

            if (IsRoomFinished(selectedRoom))
            {
                TaskDialog.Show(selectedRoom.Name, _messageIfFinished);
                return Result.Cancelled;
            }
            // get all boundaries
            var options = new SpatialElementBoundaryOptions();
            var allLimits = selectedRoom.GetBoundarySegments(options).FirstOrDefault().Where(x => x.ElementId != null);
            
            // get type for finish walls
            WallType finishType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .FirstOrDefault(x => x.Name == _finishWallTypeName) as WallType;
            if (finishType == null)
            {
                return Result.Failed;
            }

            // create finish walls
            using (var tr = new Transaction(doc, _transactionName))
            {
                tr.Start();

                Plane plane = Plane.CreateByThreePoints(XYZ.Zero, XYZ.BasisX, XYZ.BasisY);
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                foreach (BoundarySegment segment in allLimits)
                {
                    Curve curve = segment.GetCurve();
                    Element limitEl = doc.GetElement(segment.ElementId);
                    if (limitEl.GetType() != typeof(Wall))
                    {
                        continue;
                    }

                    Curve placeForFinish = GetPlaceForFinish(curve, selectedRoom, finishType.Width / 2);

                    Wall limitWall = limitEl as Wall;

                    double limitWallHeight = limitWall
                        .get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM)
                        .AsDouble();

                    var finishWall = Wall.Create(
                        document: doc,
                        curve: placeForFinish,
                        wallTypeId: finishType.Id,
                        levelId: selectedRoom.LevelId,
                        height: limitWallHeight,
                        offset: 0,
                        flip: false,
                        structural: false);

                    // join finish walls with boundaries
                    JoinGeometryUtils.JoinGeometry(doc, finishWall, limitWall);
                }

                SetRommFinished(selectedRoom);
                tr.Commit();
            }


            return Result.Succeeded;
        }

        private void SetRommFinished(Room room)
        {
            room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(_markFinished);
        }

        private bool IsRoomFinished(Room room)
        {
            return room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString() == _markFinished;
        }

        private Curve GetPlaceForFinish(Curve curve, Room room, double offset)
        {
            var rotatedCurve = curve.CreateTransformed(Transform.CreateRotation(XYZ.BasisZ, Math.PI / 2));

            var point1 = rotatedCurve.GetEndPoint(0);
            var point2 = rotatedCurve.GetEndPoint(1);

            XYZ vectorOffset = null;

            if (room.IsPointInRoom(point1))
            {
                vectorOffset = (point1 - point2).Normalize();
            }
            else
            {
                vectorOffset = (point2 - point1).Normalize();
            }

            var placeForFinish = CreateOffset(curve, vectorOffset, offset);

            return placeForFinish;
        }

        private Curve CreateOffset(Curve curve, XYZ vector, double distance)
        {
            if (curve is Line line)
            {
                var point1 = curve.GetEndPoint(0);
                var point2 = curve.GetEndPoint(1);

                point1 = point1 + vector * distance;
                point2 = point2 + vector * distance;

                return Line.CreateBound(point1, point2);
            }

            return curve;
        }
    }

    internal class ElementTypeFilter<T> : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is T;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
