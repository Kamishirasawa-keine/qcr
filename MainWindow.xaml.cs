using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace qcre
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var fileDlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "QC File (*.qc)|*.qc"
            };

            bool? result = fileDlg.ShowDialog();
            
            if (result != true)
            {
                MessageBox.Show("Select a valid file", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var parser = new QCParser();
            currentModel = parser.Parse(fileDlg.FileName);
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {

        }

        private void CloseFile(object sender, RoutedEventArgs e)
        {
            currentModel = null;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private QCModel? currentModel = null;
    }
}