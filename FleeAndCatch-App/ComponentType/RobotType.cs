using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentType
{
    public class RobotType : ClientType
    {
        private string name;
        private Type type;

        public new enum Type
        {
            ThreeWheelDrive, FourWheelDrive, ChainDrive
        }
        public RobotType(Type pType) : base((ClientType.Type) pType)
        {
        }
    }
}
