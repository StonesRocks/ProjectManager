﻿using System.Text;
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

        public void FileSave(object sender, RoutedEventArgs e)
        {
            projectManager.ExportCollectionToJson();
        }

        public void OpenCreateProject(object sender, RoutedEventArgs e)
        {
            var _absolutePath = projectManager.defaultAbsolutePath;
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