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

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProjectManagerClass projectManager;
        public MainWindow()
        {
            InitializeComponent();
            projectManager = new ProjectManagerClass();
            FrontPageListBox.ItemsSource = projectManager.Projects;
        }

        public void ExportProject(object sender, RoutedEventArgs e)
        {
            Project _project = (Project)FrontPageListBox.SelectedItem;
            projectManager.ExportProjectToJson(_project);
        }
        public void ImportProject(object sender, RoutedEventArgs e)
        {
            var _absolutePath = projectManager.projectAbsolutePath;
            ProjectCreator projectCreator = new ProjectCreator(this, projectManager);
            projectCreator.ImportProject(sender, e);
            projectCreator.SetAbsolutePath(_absolutePath);
            projectCreator.Show();
        }

        public void ButtonExit(object sender, RoutedEventArgs e)
        {
            FileSave(sender, e);
            Close();
        }

        public void ChangeCollectionDirectory(object sender, RoutedEventArgs e)
        {
            projectManager.ChangeCollectionDirectory();
        }

        public void ChangeCollection(object sender, RoutedEventArgs e)
        {
            var Path = projectManager.OpenFolderBrowser();
            if (Path == null || Path == string.Empty)
            {
                System.Windows.MessageBox.Show("No path was defined");
            }
            else
            {
                projectManager.GetCollection(Path);
            }
        }

        public void FileSave(object sender, RoutedEventArgs e)
        {
            projectManager.ExportCollectionToJson();
        }
        public void OpenCollectionEdit(object sender, RoutedEventArgs e)
        {
            CollectionEdit collectionEdit = new CollectionEdit(projectManager);
            collectionEdit.Show();
        }

        public void OpenCreateProject(object sender, RoutedEventArgs e)
        {
            var _absolutePath = projectManager.projectAbsolutePath;
            ProjectCreator projectCreator = new ProjectCreator(this, projectManager);
            projectCreator.SetAbsolutePath(_absolutePath);
            projectCreator.Show();
        }

        public void OpenProject(object sender, RoutedEventArgs e)
        {
            Project _project = (Project)FrontPageListBox.SelectedItem;
            ProjectCreator projectCreator = new ProjectCreator(this, projectManager, _project);
            projectCreator.SetAbsolutePath(_project.FullPath);
            projectCreator.Show();
        }

        private void PreviewProject(object sender, RoutedEventArgs e)
        {
            Project _project = (Project)FrontPageListBox.SelectedItem;
            projectManager.UnpackContentToRichTextBox(_project, FrontPageRichTextBox);
        }
    }
}