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
        public ProjectManagerClass projectManager { get; set; }
        private string currentAttribute { get; set; } = "Idea";
        public ProjectCreator(MainWindow mainWindow, ProjectManagerClass projectManager)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.projectManager = projectManager;
            tempProject = new Project();
        }
        int _currentContent = 0;
        public void SetProjectText(object sender, RoutedEventArgs e)
        {
            tempProject.ProjectContent[_currentContent] = RichTextBoxToString(CenterRichTextBox);
            var button = (wc.RadioButton)sender;
            var projectContentElement = button.Content as string;
            _currentContent = projectManager.KeyToIndex[projectContentElement];
            StringToRichTextBox();
        }

        public void StringToRichTextBox()
        {
            // Create a TextRange that contains the entire content of the RichTextBox.
            TextRange textRange = new TextRange(
                CenterRichTextBox.Document.ContentStart,
                CenterRichTextBox.Document.ContentEnd
            );

            // Set the text of the TextRange to the string.
            textRange.Text = tempProject.ProjectContent[_currentContent];

            //var myData = tempProject.ProjectContent[_currentContent];
            //// Create a MemoryStream from the RTF string.
            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(myData));
            //
            //// Create a TextRange that contains the entire content of the RichTextBox.
            //TextRange textRange = new TextRange(
            //    CenterRichTextBox.Document.ContentStart,
            //    CenterRichTextBox.Document.ContentEnd
            //);
            //
            //// Load the MemoryStream contents into the TextRange.
            //textRange.Load(stream, System.Windows.DataFormats.Rtf);
            //
        }
        public string RichTextBoxToString(wc.RichTextBox _myRichTextBox)
        {

            return new TextRange(
                _myRichTextBox.Document.ContentStart, 
                _myRichTextBox.Document.ContentEnd
                ).Text;

            //TextRange textRange = new TextRange(
            //    _myRichTextBox.Document.ContentStart,
            //    _myRichTextBox.Document.ContentEnd
            //    );
            //MemoryStream stream = new MemoryStream();
            //textRange.Save(stream, System.Windows.DataFormats.Rtf);
            //StreamReader streamReader = new StreamReader(stream);
            //string myData = streamReader.ReadToEnd();
            //streamReader.Close();
            //return myData;
        }

        public void SetAbsolutePath(string path)
        {
            absolutePath = path;
            TextBoxProjectPath.Text = absolutePath;
        }

        public void SaveProject(object sender, RoutedEventArgs e)
        {
            projectManager.TempProjectToProject(tempProject);
            Close();
        }

        public void OpenFolderBrowser(object sender, RoutedEventArgs e)
        {
            wf.FolderBrowserDialog dialog = new wf.FolderBrowserDialog();
            dialog.InitialDirectory = "";
            wf.DialogResult result = dialog.ShowDialog();

            if (result == wf.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;
                TextBoxProjectPath.Text = folder;
            }
        }
    }
}
