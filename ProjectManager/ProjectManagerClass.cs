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
        public string defaultAbsolutePath {  get; set; }
        private ObservableCollection<Project> projects = new ObservableCollection<Project> { };
        public ObservableCollection<Project> Projects
        {
            get
            {
                return projects;
            }
            set
            {
                projects = value;
            }
        }
        public string projectFolderPath { get; set; }
        public string collectionFolderPath { get; set; }
        public string collectionHistoryFolderPath {  get; set; }
        public ProjectManagerClass()
        {
            // Look for CollectionFolder
            CreateFolder("ProjectCollection/CollectionHistory");
            collectionFolderPath = "ProjectCollection";
            collectionHistoryFolderPath = "ProjectCollection/CollectionHistory";
            string[] collectionFile = Directory.GetFiles("ProjectCollection", "*.json");

            // Get all json files in the folder
            foreach (string file in collectionFile)
            {
                string jsonString = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    ObservableCollection<Project> collectionJSON = JsonSerializer.Deserialize<ObservableCollection<Project>>(jsonString);
                    Projects = collectionJSON;
                }
            }

            // Looks for the projectLibrary and if not found then creates it.
            CreateFolder("projectLibrary");
            projectFolderPath = "projectLibrary";
            string[] files = Directory.GetFiles("projectLibrary", "*.json");

            // Get all json files in the folder
            foreach (string file in files)
            {
                string jsonString = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    Project projectJSON = JsonSerializer.Deserialize<Project>(jsonString);
                    bool projectExists = false;
                    foreach (Project project in projects)
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
            defaultAbsolutePath = Path.GetFullPath(projectFolderPath);
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
            var Collection = projects;
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

        public void ExportProjectToJson(Project _project)
        {
            string jsonString = JsonSerializer.Serialize(_project);
            string baseName = _project.Name;
            int counter = 0;

            // Generate the initial file path.
            string filePath = Path.Combine(projectFolderPath, $"{baseName}.json");

            // While the file already exists...
            while (File.Exists(filePath))
            {
                // Increment the counter.
                counter++;

                // Generate a new file path with the counter.
                filePath = Path.Combine(projectFolderPath, $"{baseName}{counter}.json");
            }

            // Write the JSON string to the file.
            File.WriteAllText(filePath, jsonString);
        }

        public void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public void GetAbsolutePath(Project project)
        {
            project.FullPath = Path.GetFullPath(projectFolderPath);
        }
    }
}
