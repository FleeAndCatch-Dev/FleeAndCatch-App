using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentType;

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

        /// <summary>
        /// Update the robots in the list robots.
        /// </summary>
        /// <param name="pRobots">Robots from server</param>
        public static void UpdateRobots(List<Commands.Robot> pRobots)
        {
            //Delete not existing robots
            for (var i = 0; i < robots.Count; i++)
            {
                var exist = pRobots.Any(t => t.Id == robots[i].Id);
                if (!exist)
                    robots.Remove(robots[i]);
            }

            //Add new robots
            for (var i = 0; i < pRobots.Count; i++)
            {
                var exist = robots.Any(t => pRobots[i].Id == robots[i].Id);
                if (exist) continue;
                var type = (RobotType.Type) Enum.Parse(typeof(RobotType.Type), pRobots[i].Type);
                switch (type)
                {
                    case RobotType.Type.ThreeWheelDrive:
                        robots.Add(new ThreeWheelDrive(pRobots[i].Id));
                        break;
                    case RobotType.Type.FourWheelDrive:
                        break;
                    case RobotType.Type.ChainDrive:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
