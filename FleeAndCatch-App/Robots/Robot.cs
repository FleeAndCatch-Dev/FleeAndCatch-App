using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentType;

namespace Robots
{
    public abstract class Robot
    {
        protected int id;
        protected RobotType type;

        protected Robot(int pId)
        {
            this.id = pId;
        }

        public int Id => id;
        public RobotType Type => type;
    }
}
