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
        private bool newProject = false;
        public MainWindow mainWindow {  get; set; }
        private string absolutePath {  get; set; }
        private Project thisProject { get; set; }
        public ProjectManagerClass projectManager { get; set; }
        public ProjectCreator(MainWindow mainWindow, ProjectManagerClass projectManager)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.projectManager = projectManager;
            this.Title = $"Project Creator";
            thisProject = new Project();
            projectManager.SetEmptyProjectContent(thisProject, CenterRichTextBox);
            newProject = true;
        }

        public ProjectCreator(MainWindow mainWindow, ProjectManagerClass projectManager, Project _project)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.projectManager = projectManager;
            thisProject = _project;
            // Set window settings
            TextBoxProjectName.Text = _project.Name;
            SetAbsolutePath(thisProject.FullPath);
            ButtonFinish.Content = "Finish";
            ButtonBrowse.Content = "Change";
            projectManager.StringToRichTextBox(thisProject, CenterRichTextBox, 0);
            this.Title = $"Project: {_project.Name}";
            newProject = false;
        }
        int _currentContent = 0;
        public void SetProjectText(object sender, RoutedEventArgs e)
        {
            projectManager.RichTextBoxToString(thisProject, CenterRichTextBox, _currentContent);
            var button = (wc.RadioButton)sender;
            string projectContentKey = (string)button.Content;
            _currentContent = projectManager.KeyToIndex[projectContentKey];
            projectManager.StringToRichTextBox(thisProject, CenterRichTextBox, _currentContent);
        }

        // Sets up the default absolute path
        public void SetAbsolutePath(string path)
        {
            absolutePath = path;
            thisProject.FullPath = absolutePath;
            TextBoxProjectPath.Text = absolutePath;
        }

        public void FileSave(object sender, RoutedEventArgs e)
        {
            projectManager.RichTextBoxToString(thisProject, CenterRichTextBox, _currentContent);
            thisProject.FullPath = absolutePath;
            if (newProject)
            {
                projectManager.Projects.Add(thisProject);
            }
            projectManager.defaultAbsolutePath = TextBoxProjectPath.Text;
            if (TextBoxProjectName.Text != null && TextBoxProjectName.Text != "Project name")
            {
                thisProject.Name = TextBoxProjectName.Text;
            }
            projectManager.ExportCollectionToJson();
            Close();
        }

        public void SaveProject(object sender, RoutedEventArgs e)
        {
            projectManager.RichTextBoxToString(thisProject, CenterRichTextBox, _currentContent);
            thisProject.FullPath = absolutePath;
            if (newProject)
            {
                projectManager.Projects.Add(thisProject);
            }
            projectManager.defaultAbsolutePath = TextBoxProjectPath.Text;
            if (TextBoxProjectName.Text != null && TextBoxProjectName.Text != "Project name")
            {
                thisProject.Name = TextBoxProjectName.Text;
            }
            projectManager.ExportProjectToJson(thisProject);
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

        private void OnNameFocus(object sender, RoutedEventArgs e)
        {
            var _textbox = (wc.TextBox)sender;
            if (_textbox.Text == "Project name")
            {
                _textbox.Text = string.Empty;
            }
        }
        private void OnNameLostFocus(object sender, RoutedEventArgs e)
        {
            var _textbox = (wc.TextBox)sender;
            if (_textbox.Text == string.Empty)
            {
                _textbox.Text = "Project name";
            }
        }
    }
}
