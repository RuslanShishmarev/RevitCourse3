using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Lesson_RevitAPI.ViewModels;

using System;

namespace Lesson_RevitAPI
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson7 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var myHandler = new MyEventHandler();

            var modalessWndView = new ModalessWndViewModel(commandData, myHandler);
            modalessWndView.Show();

            return Result.Succeeded;
        }
    }

    internal class MyEventHandler : IExternalEventHandler
    {
        private bool _withTransaction;
        private string _nameTransaction;
        private Action _innerAction;
        private ExternalEvent _externalEvent;

        public MyEventHandler()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Execute(UIApplication app)
        {
            if (_withTransaction)
            {
                using (Transaction newTr = new Transaction(app.ActiveUIDocument.Document))
                {
                    newTr.Start(_nameTransaction);
                    _innerAction?.Invoke();
                    newTr.Commit();
                }
            }
            else
            {
                _innerAction?.Invoke();
            }
        }

        public void Raise(Action myAction, bool withTransact = true, string nameTransaction = "")
        {
            _innerAction = myAction;
            _withTransaction = withTransact;
            _nameTransaction = nameTransaction == "" ? GetName() : nameTransaction;
            _externalEvent.Raise();
        }

        public string GetName()
        {
            return nameof(MyEventHandler);
        }
    }
}
