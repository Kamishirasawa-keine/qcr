using System.Diagnostics;
using System.IO;
using System.Reflection;
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
using Microsoft.Win32;

namespace qc_reader {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void BrowseFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "QC File (*.qc)|*.qc",
                Title = "Select a .qc file"
            };
            dialog.ShowDialog();
            PathBox.Text = dialog.FileName;
        }
        private void ParseFile(object sender, RoutedEventArgs e)
        {
            //TView_Root = new TreeViewItem();
            var parser = new QCParser(PathBox.Text);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var info = parser.Parse();
            stopwatch.Stop();

            PathBox.Text = stopwatch.ElapsedMilliseconds.ToString();

            var modelName = new TreeViewItem() { 
                Header = $"Model Name: {info.modelName}" 
            };
            var bodyGroups = new TreeViewItem()
            {
                Header = "Body Groups"
            };
            foreach(var bodyGroups_v in info.bodyGroups)
            {
                var item = new TreeViewItem() {
                    Header = bodyGroups_v.name
                };
                foreach(var model in bodyGroups_v.models)
                {
                    var nitem = new TreeViewItem();
                    if (model != "Blank")
                    {
                        nitem.Header = $"Model: {model}";
                    }else
                    {
                        nitem.Header = "<Empty>";
                    }
                    
                    item.Items.Add(nitem);
                }
                bodyGroups.Items.Add(item);
            }
            var models = new TreeViewItem()
            {
                Header = "Models"
            };
            var attachments = new TreeViewItem()
            {
                Header = "Attachments: "
            };
            foreach (var attachments_v in info.attachments)
            {
                var item = new TreeViewItem()
                {
                    Header = $"名称：{attachments_v.AttachmentName}，骨骼：{attachments_v.BoneName}"
                };

                attachments.Items.Add(item);
            }
            foreach (var model in info.models)
            {
                var item = new TreeViewItem()
                {
                    Header = $"{model.name} ({model.model})"
                };
                var flexFile = new TreeViewItem()
                {
                    Header = $"Flex File ({model.flexFile.name})"
                };
                var flexes = new TreeViewItem()
                {
                    Header = "Flexes"
                };
                foreach (var ffv in model.flexFile.flexes)
                {
                    var flex = new TreeViewItem()
                    {
                        Header = ffv.name
                    };
                    var frame = new TreeViewItem()
                    {
                        Header = $"Frame: {ffv.frame}"
                    };
                    flex.Items.Add(frame);
                    flexes.Items.Add(flex);
                }
                flexFile.Items.Add(flexes);
                var flexControllers = new TreeViewItem()
                {
                    Header = "Flex Controllers"
                };
                foreach (var ffv in model.flexControllers)
                {
                    var con = new TreeViewItem()
                    {
                        Header = "unimp"
                    };
                    flexControllers.Items.Add(con);
                }
                var vars = new TreeViewItem()
                {
                    Header = "vars"
                };
                foreach (var v in model.vars)
                {
                    var vv = new TreeViewItem()
                    {
                        Header = v.name
                    };
                    vv.Items.Add(new TreeViewItem()
                    {
                        Header = v.value
                    });
                    vars.Items.Add(vv);
                }
                item.Items.Add(vars);
                item.Items.Add(flexFile);
                item.Items.Add(flexControllers);
                //models.Items.Add(item);
            }
            var includedmodel = new TreeViewItem()
            {
                Header = "Included Model(s)"
            };
            foreach (var includedmodel_v in info.includedModels)
            {
                var item = new TreeViewItem()
                {
                    Header = includedmodel_v.model
                };

                includedmodel.Items.Add(item);
            }
            TView_Root.Items.Add(modelName);
            TView_Root.Items.Add(bodyGroups);
            //TView_Root.Items.Add(models);
            TView_Root.Items.Add(attachments);
            TView_Root.Items.Add(includedmodel);
        }
    }
}