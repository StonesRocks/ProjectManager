using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    public class Material
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public string Notes {  get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public Material(string _name, int _price)
        {
            Name = _name;
            Price = _price;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
