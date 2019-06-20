using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P110_Disconnected
{
    class ComboBoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ComboBoxItem(int id, string name)
        {
            Id = id; Name = name;
        }

        public override string ToString() => Name;
    }
}
