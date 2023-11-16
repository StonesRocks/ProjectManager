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
        ProjectManagerClass projectManager;
        public MainWindow()
        {
            InitializeComponent();
            projectManager = new ProjectManagerClass();
            List<Project> projects = projectManager.Projects;
            FrontPageListBox.ItemsSource = projects;
        }

        public void OpenCreateProject(object sender, RoutedEventArgs e)
        {
            ProjectCreator projectCreator = new ProjectCreator(this);
            projectCreator.Show();
        }
    }
}