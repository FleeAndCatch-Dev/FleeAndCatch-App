using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentType
{
    public class ClientType
    {
        private string name;
        private Type type;

        /// <summary>
        /// Type of a component in the szenario.
        /// </summary>
        public enum Type
        {
            App, Robot
        }

        /// <summary>
        /// Create an object of the type client.
        /// </summary>
        /// <param name="pType">Type of the client</param>
        public ClientType(Type pType)
        {
            this.name = pType.ToString();
            this.type = pType;
        }
    }
}
