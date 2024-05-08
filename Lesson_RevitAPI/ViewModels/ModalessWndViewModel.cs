using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Lesson_RevitAPI.Models;
using Lesson_RevitAPI.Views;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Lesson_RevitAPI.ViewModels
{
    internal class ModalessWndViewModel : ViewModelBase
    {
        private Dictionary<string, FilteredElementCollector> _allCategoriesAndCollectors { get; }
            = new Dictionary<string, FilteredElementCollector>();

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                AllElements = new ObservableCollection<Element>(_allCategoriesAndCollectors[_selectedCategory].ToElements());
            }
        }

        public List<string> AllCategoryNames { get; }

        private ObservableCollection<Element> _allElements = new ObservableCollection<Element>();
        public ObservableCollection<Element> AllElements
        {
            get => _allElements;
            private set
            {
                _allElements = value;
                OnPropertyChanged(nameof(AllElements));
            }
        }

        private Element _selectedElement;
        public Element SelectedElement
        {
            get => _selectedElement;
            set
            {
                _selectedElement = value;
                OnPropertyChanged(nameof(SelectedElement));
                _uidoc.Selection.SetElementIds(new[] { _selectedElement.Id });
            }
        }

        public ICommand MarkElementCommand { get; }
        public ICommand ShowElementCommand { get; }
        public ICommand DeleteElementCommand { get; }

        private UIDocument _uidoc;
        private Document _doc;
        private MyEventHandler _myEventHandler;
        private Window _view;

        public ModalessWndViewModel(ExternalCommandData commandData, MyEventHandler myEventHandler)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            _uidoc = uidoc;
            _doc = doc;
            _myEventHandler = myEventHandler;
            MarkElementCommand = new RelayCommand(() => _myEventHandler.Raise(MarkElement, nameTransaction: "Удаление"));
            ShowElementCommand = new RelayCommand(() => _myEventHandler.Raise(ShowElement, nameTransaction: "Показать"));
            DeleteElementCommand = new RelayCommand(() => _myEventHandler.Raise(DeleteElements, nameTransaction: "Маркировка"));

            _allCategoriesAndCollectors.Add("Стены", new FilteredElementCollector(_doc).OfClass(typeof(Wall)));
            _allCategoriesAndCollectors.Add("Перекрытия", new FilteredElementCollector(_doc).OfClass(typeof(Floor)));
            _allCategoriesAndCollectors.Add("Крыша", new FilteredElementCollector(_doc).OfClass(typeof(RoofBase)));

            AllCategoryNames = _allCategoriesAndCollectors.Keys.ToList();
            SelectedCategory = AllCategoryNames.First();

            _view = new ModalessWnd();
            _view.DataContext = this;
        }

        public void Show()
        {
            _view.Show();
        }

        private void ShowElement()
        {
            if (_uidoc.ActiveView is View3D view3d)
            {
                if (SelectedElement != null)
                    view3d.SetSectionBox(SelectedElement.get_BoundingBox(view3d));
                else TaskDialog.Show("Ошибка", "Выберите элемент");
            }
            else
            {
                var first3DView = new FilteredElementCollector(_doc)
                    .OfClass(typeof(View3D))
                    .FirstOrDefault() as View3D;

                if (first3DView is null) return;

                _uidoc.ActiveView = first3DView;
                first3DView.SetSectionBox(SelectedElement.get_BoundingBox(first3DView));
            }
        }

        private void DeleteElements()
        {
            if (SelectedElement != null)
            {
                var userAnswer = TaskDialog.Show(
                    "Удаление",
                    $"Вы уверены, что хотите удалить элемент {SelectedElement.Id}?",
                    TaskDialogCommonButtons.Ok);

                if (userAnswer == TaskDialogResult.Ok)
                {
                    _doc.Delete(SelectedElement.Id);
                    AllElements.Remove(SelectedElement);
                }
            }
        }

        private void MarkElement()
        {
            if (SelectedElement != null)
            {
                // действие...
            }
        }
    }
}
