using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;

namespace FleeAndCatch_App.Controller
{
    public static class SzenarioController
    {
        private static bool updated;
        private static List<Szenario> szenarios = new List<Szenario>();
        public static bool ChangedPosition { get; set; }
        public static bool Refresh { get; set; }

        public static bool Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public static List<Szenario> Szenarios
        {
            get { return szenarios; }
            set { szenarios = value; }
        }
    }
}
