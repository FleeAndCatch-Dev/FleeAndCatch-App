using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace FleeAndCatch_App.Models
{
    [ImplementPropertyChanged]
    public class SzenarioGroup
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public List<int> Items { get; set; }
        public int Choosen { get; set; }

        public SzenarioGroup(string pName, int pNumber)
        {
            this.Name = pName;
            this.Number = pNumber;

            this.Items = new List<int>();
            for (var i = 0; i < pNumber + 1; i++)
            {
                this.Items.Add(i);
            }
        }
    }
}
