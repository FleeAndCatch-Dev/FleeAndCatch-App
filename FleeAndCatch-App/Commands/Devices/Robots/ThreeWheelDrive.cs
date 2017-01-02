using Commands.Components;
using Commands.Identifications;
using static Commands.Components.ComponentType;

namespace Commands.Devices.Robots
{
    public class ThreeWheelDrive : Robot
    {
        public ThreeWheelDrive(int pId, bool pActive, Position pPosition, double pSpeed) : base(new RobotIdentification(pId, IdentificationType.Robot.ToString(), RobotType.ThreeWheelDrive.ToString(), RoleType.Undefined.ToString()), pActive, pPosition, pSpeed)
        {
        }
    }
}
