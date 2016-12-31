using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Component;

namespace Robots
{
    public class ThreeWheelDrive : Robot
    {
        public ThreeWheelDrive(int pId, string pAddress, int pPort, Position pPosition, double pSpeed) : base(new Identification(pId, pAddress, pPort, ComponentType.IdentificationType.Robot.ToString(), ComponentType.RobotType.ThreeWheelDrive.ToString()), false, pPosition, pSpeed)
        {
        }
    }
}
