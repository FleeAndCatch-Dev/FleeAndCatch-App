using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Components;

namespace FleeAndCatch.Commands.Models.Devices.Robots
{
    public class ThreeWheelDrive : Robot
    {
        public ThreeWheelDrive(int pId, bool pActive, Position pPosition, double pSpeed) : base(new RobotIdentification(pId, ComponentType.IdentificationType.Robot.ToString(), ComponentType.RobotType.ThreeWheelDrive.ToString(), ComponentType.RoleType.Undefined.ToString()), pActive, pPosition, pSpeed)
        {
        }
    }
}
