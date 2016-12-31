using Commands.Devices.Robots;
using System.Collections.Generic;

namespace Controller
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
