using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using wc = System.Windows.Controls;

namespace ProjectManager
{
    public class Project
    {
        private DateTime _startDate = DateTime.MinValue;
        public DateTime startDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (_startDate == DateTime.MinValue)
                {
                    _startDate = value;
                }
            }
        }

        public DateTime lastActive { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                if (_name == string.Empty)
                {
                    return "UnnamedProject";
                }
                return _name;
            }
            set
            {
                if (value == string.Empty || value == _name)
                {
                    return;
                }
                var _value = value;
                _name = char.ToUpper(_value[0]) + _value.Substring(1);
            }
        }
        private string _projectID;
        public string projectID
        {
            get { return _projectID; }
            set 
            {
                if (_projectID == null)
                { 
                    _projectID = $"{Name}{DateTime.Today.ToString("yyyyMMdd")}"; 
                } 
            }
        }

        public string[] ProjectContent { get; set; } = new string[5] { "", "", "", "", "" };
        public string FullPath {  get; set; } = string.Empty;
        public List<Material> Materials { get; set; } = new List<Material>();
        public Project()
        {
            this.startDate = DateTime.Now;
        }
        public Project(string _name, string _folderPath)
        {
            this.Name = _name;
            this.startDate = DateTime.Now;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
