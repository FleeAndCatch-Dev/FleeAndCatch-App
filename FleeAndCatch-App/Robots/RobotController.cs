using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Robots
{
    public static class RobotController
    {
        private static List<Robot> robots = new List<Robot>();

        public static List<Robot> Robots
        {
            get { return robots; }
            set { robots = value; }
        }
    }
}
