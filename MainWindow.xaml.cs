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
            var parser = new QCParser();
            var model = parser.Parse(PathBox.Text);
            //InfoView.Items.Add(BuildTree(model, "root"));
        }
        static TreeViewItem BuildTree(object obj, string name)
        {
            var item = new TreeViewItem();

            item.Name = name;
            Type type = obj.GetType();

            if(type.IsPrimitive||type == typeof(string))
                item.Name = obj.ToString();
            else
            {
                foreach(var field in type.GetFields())
                {
                    var child = BuildTree(field.GetValue(obj), field.Name);
                    item.Items.Add(child);
                }
            }

            return item;
        }
    }
}