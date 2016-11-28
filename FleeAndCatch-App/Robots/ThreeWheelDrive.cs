using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentType;

namespace Robots
{
    public class ThreeWheelDrive : Robot
    {
        public ThreeWheelDrive(int pId) : base(pId)
        {
            this.type = new RobotType(RobotType.Type.ThreeWheelDrive);
        }
    }
}
