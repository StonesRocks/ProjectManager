using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wf = System.Windows.Forms;
using wc = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for ProjectCreator.xaml
    /// </summary>
    public partial class ProjectCreator : Window
    {
        public MainWindow mainWindow {  get; set; }
        private string absolutePath {  get; set; }
        private Project tempProject { get; set; }
        private string currentAttribute { get; set; } = "Idea";
        public ProjectCreator(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            tempProject = new Project();
        }

        public void SetProjectText(object sender, RoutedEventArgs e)
        {
            tempProject.ProjectContent[0] = RichTextBoxToString(CenterRichTextBox);
            var button = (wc.Button)sender;
            var projectattribute = button.Content as string;
            
        }

        public string RichTextBoxToString(wc.RichTextBox _myRichTextBox)
        {
            TextRange textRange = new TextRange(
                _myRichTextBox.Document.ContentStart,
                _myRichTextBox.Document.ContentEnd
                );
            MemoryStream stream = new MemoryStream();
            textRange.Save(stream, System.Windows.DataFormats.Rtf);
            StreamReader streamReader = new StreamReader(stream);
            string myData = streamReader.ReadToEnd();
            return myData;
        }

        public void SetAbsolutePath(string path)
        {
            absolutePath = path;
            TextBoxProjectPath.Text = absolutePath;
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
