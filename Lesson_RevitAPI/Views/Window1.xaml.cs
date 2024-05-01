using System.Windows;
using System.Windows.Controls;

namespace Lesson_RevitAPI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string _headText;
        private string _firstBlockDefault;
        public Window1()
        {
            InitializeComponent();
            _firstBlockDefault = this.firstBlock.Text;
        }

        private void headTB_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            _headText = textBlock.Text;
            textBlock.Text = "Что такое?";
        }

        private void headTB_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            textBlock.Text = _headText;
        }

        private void firstBlockInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.firstBlockInput.Text))
            {
                this.firstBlock.Text = _firstBlockDefault;
            }
            else
            {
                this.firstBlock.Text = this.firstBlockInput.Text;
            }
        }
    }
}
