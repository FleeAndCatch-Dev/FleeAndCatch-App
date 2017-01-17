using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Devices.Robots;

namespace FleeAndCatch_App.Controller
{
    public static class RobotController
    {
        private static bool updated;
        private static List<Robot> robots = new List<Robot>();

        public static bool Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public static List<Robot> Robots
        {
            get { return robots; }
            set { robots = value; }
        }
    }
}
