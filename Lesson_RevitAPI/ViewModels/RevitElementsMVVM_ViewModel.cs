using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Lesson_RevitAPI.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using DBDocument = Autodesk.Revit.DB.Document;

namespace Lesson_RevitAPI.ViewModels
{
    internal class RevitElementsMVVM_ViewModel : ViewModelBase
    {
        private ElementContainerViewModel _selectedElement;
        public ElementContainerViewModel SelectedElement
        {
            get => _selectedElement;
            set 
            { 
                _selectedElement = value;
                OnPropertyChanged(nameof(SelectedElement));
            }
        }

        private ObservableCollection<ElementContainerViewModel> _allElements;
        public ObservableCollection<ElementContainerViewModel> AllElements 
        {
            get => _allElements;
            private set
            {
                _allElements = value;
                OnPropertyChanged(nameof(AllElements));
            }
        }

        private string _searchStr;
        public string SearchStr 
        {
            get => _searchStr;
            set
            {
                _searchStr = value;
                FilterElements(_searchStr);
            }
        }

        public ICommand ShowInfoCommand { get; }

        private DBDocument _doc;
        private Window _wnd;
        private List<ElementContainerViewModel> _defaultElements;

        public RevitElementsMVVM_ViewModel(DBDocument doc, Window wnd)
        {
            _doc = doc;
            _wnd = wnd;
            _defaultElements = new FilteredElementCollector(_doc)
                .OfClass(typeof(Wall))
                .Select(x => new ElementContainerViewModel(x))
                .ToList();
            AllElements = new ObservableCollection<ElementContainerViewModel>(_defaultElements);

            ShowInfoCommand = new RelayCommand(ShowInfo);

            _wnd.DataContext = this;
        }

        public void Show() => _wnd.Show();
        public void ShowDialog() => _wnd.ShowDialog();

        private void FilterElements(string search)
        {
            if (string.IsNullOrEmpty(search.Trim(' ')))
            {
                AllElements = new ObservableCollection<ElementContainerViewModel>(_defaultElements);
            }
            else
            {
                AllElements = new ObservableCollection<ElementContainerViewModel>(_defaultElements
                    .Where(x => x.Name.ToLower().Contains(search.ToLower())));
            }
        }

        private void ShowInfo()
        {
            if (SelectedElement != null)
            {
                TaskDialog.Show("Инфо", SelectedElement.GetInfo());
            }
            _wnd.Focus();
        }
    }
}
