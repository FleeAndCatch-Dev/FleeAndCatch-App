using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Connection : Command
    {
        [JsonProperty("identification")]
        private Identification identification;

        /// <summary>
        /// Create an object of a connection command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as connaction type.</param>
        /// <param name="pClient">Client for representation of the device.</param>
        public Connection(string pId, string pType, Identification pIdentification) : base(pId, pType)
        {
            identification = pIdentification;
        }

        /// <summary>
        /// Get the command as json string.
        /// </summary>
        /// <returns>Json string.</returns>
        public override string GetCommand()
        {
            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"identification", identification.GetJObject()}
            };

            return JsonConvert.SerializeObject(command);
        }

        public Identification Identification => identification;
    }

    public class ConnectionType : CommandType
    {
        /// <summary>
        /// Enumeration of connection type.
        /// </summary>
        public new enum Type
        {
            GetId, SetId, GetType, SetType, Disconnect
        }

        /// <summary>
        /// Create an object of the conection type.
        /// </summary>
        /// <param name="pType">Type as connection type.</param>
        public ConnectionType(Type pType) : base((CommandType.Type) pType)
        {
        }
    }
}
