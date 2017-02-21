using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Szenarios;

namespace FleeAndCatch.Controller
{
    public static class SzenarioController
    {
        private static bool updated;
        private static List<Szenario> szenarios = new List<Szenario>();
        public static bool Changed { get; set; }
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
