using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wf = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for ProjectCreator.xaml
    /// </summary>
    public partial class ProjectCreator : Window
    {
        public MainWindow mainWindow {  get; set; }
        public ProjectCreator(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public void OpenFolderBrowser(object sender, RoutedEventArgs e)
        {
            wf.FolderBrowserDialog dialog = new wf.FolderBrowserDialog();
            dialog.InitialDirectory = "";
            wf.DialogResult result = dialog.ShowDialog();

            if (result == wf.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;
            }
        }
    }
}
