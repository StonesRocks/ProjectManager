using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for CollectionEdit.xaml
    /// </summary>
    public partial class CollectionEdit : Window
    {
        ProjectManagerClass projectManager;
        public CollectionEdit(ProjectManagerClass _projectManager)
        {
            InitializeComponent();
            this.projectManager = _projectManager;
            ListBoxCollection.ItemsSource = projectManager.projectCollection;
            ListBoxProjects.ItemsSource = projectManager.projectJSON;
        }

        public void AddJSONToCollection(object sender, RoutedEventArgs e)
        {
            Project project = (Project)ListBoxProjects.SelectedItem;
            projectManager.projectCollection.Add(project);
        }
        public void RemoveProjectFromCollection(object sender, RoutedEventArgs e)
        {
            Project project = (Project)ListBoxCollection.SelectedItem;
            projectManager.ExportProjectToJson(project);
            bool ProjectExists = false;
            foreach (var JSONproject in  projectManager.projectJSON)
            {
                if (JSONproject.projectID == project.projectID)
                {
                    ProjectExists = true;
                    break;
                }
            }
            if (!ProjectExists)
            {
                projectManager.projectJSON.Add(project);                
            }
            projectManager.projectCollection.Remove(project);
            projectManager.ExportCollectionToJson();
        }
    }
}
