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
        public string Summary { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public List<Material> Materials { get; set; } = new List<Material>();
        public Project() { }

        public Project(string _name, string _summary, string _folderPath)
        {
            Name = _name;
            Summary = _summary;
            this.startDate = DateTime.Now;
        }
        public Project(string _name, string _summary, string _description, string _folderPath)
        {
            this.Name = _name;
            this.Description = _description;
            this.startDate = DateTime.Now;
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
