using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleeAndCatch.Commands.Models.Devices.Robots
{
    public class ThreeWheelDrive : Robot
    {
        public ThreeWheelDrive(int pId, bool pActive, Position pPosition, double pSpeed, double pUltrasonic, double pGyro) : base(new RobotIdentification(pId, IdentificationType.Robot.ToString(), RobotType.ThreeWheelDrive.ToString(), RoleType.Undefined.ToString()), pActive, pPosition, pSpeed, pUltrasonic, pGyro)
        {
        }
    }
}
