using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using System.Windows;
using wf = System.Windows.Forms;
using wc = System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace ProjectManager
{
    public class ProjectManagerClass
    {
        public string projectAbsolutePath {  get; set; }
        public string collectionAbsolutePath { get; set; }
        public ObservableCollection<Project> Projects = new ObservableCollection<Project> { };
        public ObservableCollection<Project> projectCollection = new ObservableCollection<Project> { };
        public ObservableCollection<Project> projectJSON = new ObservableCollection<Project> { };
        public string projectFolderPath { get; set; }
        public string collectionFolderPath { get; set; }
        public string collectionHistoryFolderPath {  get; set; }
        public ProjectManagerClass()
        {
            // Look for CollectionFolder
            GetCollection("ProjectCollection");


            // Looks for the projectLibrary and if not found then creates it.
            GetProjects("projectLibrary");

            // Add both to Projects
            AddCollectionandJSONToProjects();
        }

        private void AddCollectionandJSONToProjects()
        {
            foreach(Project _project in projectCollection)
            {
                Projects.Add(_project);
            }
            foreach(Project _project in projectJSON)
            {
                Projects.Add(_project);
            }
        }

        public void GetOneProject()
        {
            var file = OpenFileBrowser();
            if (file == null || file == string.Empty)
            {
                System.Windows.MessageBox.Show("No file was selected");
                return;
            }
            string jsonString = File.ReadAllText(file);
            if (!string.IsNullOrEmpty(jsonString))
            {
                Project projectJSON = JsonSerializer.Deserialize<Project>(jsonString);
                bool projectExists = false;
                foreach (Project project in Projects)
                {
                    if (project.projectID == projectJSON.projectID)
                    {
                        projectExists = true;
                        break;
                    }
                }
                if (!projectExists)
                {
                    Projects.Add(projectJSON);
                }
            }
        }

        public void GetProjects(string _projectFolderPath)
        {
            CreateFolder($"{_projectFolderPath}");
            projectFolderPath = $"{_projectFolderPath}";
            string[] files = Directory.GetFiles($"{_projectFolderPath}", "*.json");

            // Get all json files in the folder
            foreach (string file in files)
            {
                string jsonString = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    Project projectJSON = JsonSerializer.Deserialize<Project>(jsonString);
                    bool projectExists = false;
                    foreach (Project project in projectCollection)
                    {
                        if (project.projectID == projectJSON.projectID)
                        {
                            projectExists = true;
                            break;
                        }
                    }
                    if (!projectExists)
                    {
                        this.projectJSON.Add(projectJSON);
                    }
                }
            }
            projectAbsolutePath = Path.GetFullPath(projectFolderPath);
        }
        public void ChangeCollectionDirectory()
        {
            var Path = OpenFolderBrowser();
            if (Path == null || Path == string.Empty)
            {
                System.Windows.MessageBox.Show("No path was defined");
            }
            else
            {
                collectionAbsolutePath = Path;
            }
        }
        public string OpenFileBrowser()
        {
            wf.OpenFileDialog dialog = new wf.OpenFileDialog();
            dialog.InitialDirectory = "";
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            wf.DialogResult result = dialog.ShowDialog();

            if (result == wf.DialogResult.OK)
            {
                string file = dialog.FileName;
                return file;
            }
            return string.Empty;
        }
        public string OpenFolderBrowser()
        {
            wf.FolderBrowserDialog dialog = new wf.FolderBrowserDialog();
            dialog.InitialDirectory = "";
            wf.DialogResult result = dialog.ShowDialog();

            if (result == wf.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;
                return folder;
            }
            return string.Empty;
        }

        public void GetCollection(string _collectionFolder)
        {
            string _historyFolder = "CollectionHistory";
            // Look for CollectionFolder
            CreateFolder($"{_collectionFolder}/{_historyFolder}");
            collectionFolderPath = $"{_collectionFolder}";
            collectionHistoryFolderPath = $"{_collectionFolder}/{_historyFolder}";
            string[] collectionFile = Directory.GetFiles($"{_collectionFolder}", "*.json");

            // Get all json files in the folder
            foreach (string file in collectionFile)
            {
                string jsonString = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    ObservableCollection<Project> collectionJSON = JsonSerializer.Deserialize<ObservableCollection<Project>>(jsonString);
                    projectCollection = collectionJSON;
                }
            }
            collectionAbsolutePath = Path.GetFullPath(collectionFolderPath);
        }

        // Dictionary to save correct information under correct label
        public Dictionary<string, int> KeyToIndex = new Dictionary<string, int>()
        {
            {"Idea", 0 },
            {"Research", 1},
            {"Planning", 2},
            {"Resources", 3},
            {"Review", 4},
        };

        public void EditName(Project project, string _newName)
        {
            project.Name = _newName;
        }

        private string[] myDictKeys = new string[] {"Idea", "Research", "Planning", "Resources", "Review"};
        public void UnpackContentToRichTextBox(Project _project, wc.RichTextBox _myRichTextBox)
        {
            _myRichTextBox.Document = new System.Windows.Documents.FlowDocument();

            for (int i = 0; i < _project.ProjectContent.Count(); i++)
            {
                AddFormattedText(_myRichTextBox, $"{myDictKeys[i]}\n", 20);

                var rtfText = _project.ProjectContent[i];
                TextRange textRange = new TextRange(_myRichTextBox.Document.ContentEnd, _myRichTextBox.Document.ContentEnd);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(rtfText)))
                {
                    ms.Position = 0;
                    textRange.Load(ms, System.Windows.DataFormats.Rtf);
                }
            }
            //foreach (string rtfText in _project.ProjectContent)
            //{
            //    _myRichTextBox.AppendText("\n");
            //    TextRange textRange = new TextRange(_myRichTextBox.Document.ContentEnd, _myRichTextBox.Document.ContentEnd);
            //    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(rtfText)))
            //    {
            //        ms.Position = 0;
            //        textRange.Load(ms, System.Windows.DataFormats.Rtf);
            //    }
            //}
        }

        public void SetEmptyProjectContent(Project _project, wc.RichTextBox _richtextbox)
        {
            for (int i = 0; i < _project.ProjectContent.Length; i++)
            {
                _project.ProjectContent[i] = 
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}}\viewkind4\uc1\pard\fs20\par}"));
            }
        }
        public void AddFormattedText(wc.RichTextBox richTextBox, string text, double fontSize)
        {
            // Create a new Run with the specified text and font size
            System.Windows.Documents.Run run = new System.Windows.Documents.Run(text)
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Bold
            };

            // Create a new Paragraph with the Run
            System.Windows.Documents.Paragraph paragraph = new System.Windows.Documents.Paragraph(run);

            // Add the Paragraph to the RichTextBox
            richTextBox.Document.Blocks.Add(paragraph);
        }

        // Unpacks string into RichTextBox content
        public void StringToRichTextBox(Project _project, wc.RichTextBox _myRichTextBox, int _contentIndex)
        {
            string rtfText = _project.ProjectContent[_contentIndex];
            TextRange textRange = new TextRange(_myRichTextBox.Document.ContentStart, _myRichTextBox.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(rtfText)))
            {
                ms.Position = 0;
                textRange.Load(ms, System.Windows.DataFormats.Rtf);
            }
            //// Create a TextRange that contains the entire content of the RichTextBox.
            //TextRange textRange = new TextRange(
            //    _myRichTextBox.Document.ContentStart,
            //    _myRichTextBox.Document.ContentEnd
            //);
            //
            //// Set the text of the TextRange to the string.
            //textRange.Text = _project.ProjectContent[_contentIndex];
        }

        // Saves RichTextBox content as a string
        public void RichTextBoxToString(Project _project, wc.RichTextBox _myRichTextBox, int _contentIndex)
        {
            TextRange textRange = new TextRange(_myRichTextBox.Document.ContentStart, _myRichTextBox.Document.ContentEnd);
            string rtfText; // string to save to memory
            using (MemoryStream ms = new MemoryStream())
            {
                textRange.Save(ms, System.Windows.DataFormats.Rtf);
                rtfText = Convert.ToBase64String(ms.ToArray());
            }
            _project.ProjectContent[_contentIndex] = rtfText;
            //string stringOfRichTextBox = new TextRange(
            //    _myRichTextBox.Document.ContentStart,
            //    _myRichTextBox.Document.ContentEnd
            //    ).Text;
            //_project.ProjectContent[_contentIndex] = stringOfRichTextBox;
        }
        public void ExportCollectionToJson()
        {
            var Collection = projectCollection;
            string jsonString = JsonSerializer.Serialize(Collection);
            string baseName = $"{DateTime.Today.ToString("yyyyMMdd")}Collection";

            // Generate the initial file path.
            string filePath = Path.Combine(collectionFolderPath, $"{baseName}.json");

            string sourceFolder = collectionFolderPath;
            string targetFolder = collectionHistoryFolderPath;

            // Ensure that the target directory exists.
            Directory.CreateDirectory(targetFolder);

            // Get the .json files in the source directory.
            string[] jsonFiles = Directory.GetFiles(sourceFolder, "*.json");

            foreach (string file in jsonFiles)
            {
                // Get the filename.
                string fileName = Path.GetFileName(file);

                // Create the paths for the source and destination files.
                string sourceFile = Path.Combine(sourceFolder, fileName);
                string destFile = Path.Combine(targetFolder, fileName);

                // Move the file.
                File.Move(sourceFile, destFile, true);
            }

            // While the file already exists...
            //while (File.Exists(filePath))
            //{
            //    // Prompt to rename or override collection
            //
            //    // Generate a new file path with the counter.
            //    filePath = Path.Combine(collectionFolderPath, $"{baseName}.json");
            //}

            // Write the JSON string to the file.
            File.WriteAllText(filePath, jsonString);
        }

        public void ExportProjectToJson(Project _project, string filePath = "")
        {
            if (_project == null) return;
            string jsonString = JsonSerializer.Serialize(_project);
            string baseName = _project.Name;
            int counter = 0;

            // Generate the initial file path if none is given.
            if (filePath == "")
            {
                filePath = Path.Combine(projectFolderPath, $"{baseName}.json");
            }

            // While the file already exists...
            if (File.Exists(filePath))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to overwrite existing file?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Overwrite the file by not changing its name
                }
                else
                {
                    while (File.Exists(filePath))
                    {
                        // Increment the counter.
                        counter++;
            
                        // Generate a new file path with the counter.
                        filePath = Path.Combine(projectFolderPath, $"{baseName}{counter}.json");
                    }
                }
            }
            _project.FullPath = Path.GetFullPath(filePath);
            // Write the JSON string to the file.
            File.WriteAllText(filePath, jsonString);
            System.Windows.MessageBox.Show($"Exported {filePath}");
        }
        public void RemoveProjectJson(Project _project)
        {
            if (_project == null) return;
            // Get the ID of the project.
            string projectId = _project.projectID;

            // Get a list of all JSON files in the directory.
            string[] jsonFiles = Directory.GetFiles("projectLibrary", "*.json");

            // Loop through each JSON file.
            foreach (string jsonFile in jsonFiles)
            {
                // Read the contents of the JSON file.
                string json = File.ReadAllText(jsonFile);

                // Deserialize the JSON into a Project object.
                Project fileProject = JsonSerializer.Deserialize<Project>(json);

                // Check if the fileProject has the same ID as the given project.
                if (fileProject.projectID == projectId)
                {
                    // Delete the JSON file.
                    File.Delete(jsonFile);
                    projectJSON.Remove(_project);
                    Projects.Remove(_project);
                }
            }
            //if (_project == null) return;
            //var filePath = _project.FullPath;
            //System.Windows.MessageBox.Show($"{filePath}");
            //projectJSON.Remove(_project);
            //if (File.Exists(filePath))
            //{
            //    System.Windows.MessageBox.Show($"{filePath}");
            //    File.Delete(filePath);
            //}
        }

        public void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
