using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal class Project
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
                    return "Unnamed project";
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

        public string[] ProjectContent = new string[6] {"","","","","",""};
        public Dictionary<string, int> KeyToIndex;
        //public string Idea {  get; set; } = string.Empty;
        //public string Research {  get; set; } = string.Empty;
        //public string Planning {  get; set; } = string.Empty;
        //public string Resources {  get; set; } = string.Empty;
        //public string Review {  get; set; } = string.Empty;
        public string FullPath {  get; set; } = string.Empty;
        public List<Material> Materials { get; set; } = new List<Material>();
        public Project() { }
        public Project(string _name, string _folderPath)
        {
            this.Name = _name;
            this.startDate = DateTime.Now;
            KeyToIndex = new Dictionary<string, int>()
            {
                {"Idea", 0 },
                {"Research", 1},
                {"Planning", 2},
                {"Resources", 3},
                {"Review", 4},
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
