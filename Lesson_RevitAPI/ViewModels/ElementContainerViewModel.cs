using Autodesk.Revit.DB;

namespace Lesson_RevitAPI.ViewModels
{
    internal class ElementContainerViewModel : ViewModelBase
    {
        public Element Element { get; }

        public string Name => Element?.Name;

		private bool _isShowed = false;
		public bool IsShowed
        {
			get => _isShowed;
			private set 
			{
				_isShowed = value;
				OnPropertyChanged(nameof(IsShowed));
			}
		}

        public ElementContainerViewModel(Element element)
        {
            Element = element;
        }

        public string GetInfo()
        {
            IsShowed = true;
            var level = Element.Document.GetElement(Element.LevelId);
            return $"ID: {Element.Id.IntegerValue}\nName: {Element.Name}\nLevel: {level.Name}";
        }
    }
}
