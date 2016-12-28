using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class ComponentType
    {
        public enum IdentificationType
        {
            App, Robot
        }

        public enum AppType
        {
            App
        }

        public enum RobotType
        {
            ThreeWheelDrive, FourWheelDrive, ChainDrive
        }
    }
}
