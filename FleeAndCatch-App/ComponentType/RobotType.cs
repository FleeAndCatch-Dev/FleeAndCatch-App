using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentType
{
    public class RobotType
    {
        private string name;
        private Type type;

        /// <summary>
        /// Robot type of a robot in the szenario.
        /// </summary>
        public enum Type
        {
            ThreeWheelDrive, FourWheelDrive, ChainDrive
        }

        /// <summary>
        /// Create an object of the type robot.
        /// </summary>
        /// <param name="pType">Type of the robot</param>
        public RobotType(Type pType)
        {
            this.name = pType.ToString();
            this.type = pType;
        }
    }
}
