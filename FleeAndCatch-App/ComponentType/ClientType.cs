using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentType
{
    public class ClientType
    {
        protected string name;
        protected Type type;

        public enum Type
        {
            App, Robot
        }

        public ClientType(Type pType)
        {
            this.name = pType.ToString();
            this.type = pType;
        }
    }
}
