using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;

namespace EasyMatrix
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RichTextFile_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = e.OriginalSource as Hyperlink;
            if (link != null)
            {
                Process.Start(link.NavigateUri.ToString());
            }
        }
    }
}
