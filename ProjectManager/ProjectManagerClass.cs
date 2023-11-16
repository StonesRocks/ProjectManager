﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManager
{
    class ProjectManagerClass
    {
        private List<Project> _projects = new List<Project> { };
        public List<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
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
        }

        public void EditName(Project project, string _newName)
        {
            project.Name = _newName;
        }

        public void EditSummary(Project project, string _newSummary)
        {
            project.Summary = _newSummary;
        }

        public void EditDescription(Project project, string _newDescription)
        {
            project.Description = _newDescription;
        }

        public void CreateProject(string _name, string _summary)
        {
            Projects.Add(new Project(_name, _summary, folderPath));
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