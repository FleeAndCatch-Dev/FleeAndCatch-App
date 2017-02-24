using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace FleeAndCatch_App.Models
{
    [ImplementPropertyChanged]
    public class ConnectionModel
    {
        public string Address { get; set; }
        public bool Save { get; set; }
    }
}
