﻿using System;
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

namespace ProjectManager
{
    public class ProjectManagerClass
    {
        public string defaultAbsolutePath {  get; set; }
        private List<Project> projects = new List<Project> { };
        public List<Project> Projects
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
        public string folderPath { get; set; }
        public ProjectManagerClass()
        {
            // Looks for the projectLibrary and if not found then creates it.
            CreateFolder("projectLibrary");
            folderPath = "projectLibrary";

            // Get all json files in the folder
            string[] files = Directory.GetFiles("projectLibrary", "*.json");
            // Deserialiezed Projects
            foreach (string file in files)
            {
                string jsonString = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    Project projectJSON = JsonSerializer.Deserialize<Project>(jsonString);
                    Projects.Add(projectJSON);
                }
            }
            defaultAbsolutePath = Path.GetFullPath(folderPath);
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

        // Unpacks string into RichTextBox content
        public void StringToRichTextBox(Project _project, wc.RichTextBox _myRichTextBox, int _contentIndex)
        {
            // Create a TextRange that contains the entire content of the RichTextBox.
            TextRange textRange = new TextRange(
                _myRichTextBox.Document.ContentStart,
                _myRichTextBox.Document.ContentEnd
            );

            // Set the text of the TextRange to the string.
            textRange.Text = _project.ProjectContent[_contentIndex];
        }

        // Saves RichTextBox content as a string
        public void RichTextBoxToString(Project _project, wc.RichTextBox _myRichTextBox, int _contentIndex)
        {
            string stringOfRichTextBox = new TextRange(
                _myRichTextBox.Document.ContentStart,
                _myRichTextBox.Document.ContentEnd
                ).Text;
            _project.ProjectContent[_contentIndex] = stringOfRichTextBox;
        }

        public void ExportToJson(Project _project)
        {
            string jsonString = JsonSerializer.Serialize(_project);
            string baseName = _project.Name;
            int counter = 0;

            // Generate the initial file path.
            string filePath = Path.Combine(folderPath, $"{baseName}.json");

            // While the file already exists...
            while (File.Exists(filePath))
            {
                // Increment the counter.
                counter++;

                // Generate a new file path with the counter.
                filePath = Path.Combine(folderPath, $"{baseName}{counter}.json");
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
            project.FullPath = Path.GetFullPath(folderPath);
        }
    }
}
