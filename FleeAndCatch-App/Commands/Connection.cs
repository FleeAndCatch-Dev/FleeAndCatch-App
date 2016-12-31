using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Commands.Devices;
using Commands.Identifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Connection : Command
    {
        [JsonProperty("device")]
        private Device device;

        /// <summary>
        /// Create an object of a connection command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as connaction type.</param>
        /// <param name="pClient">Client for representation of the device.</param>
        public Connection(string pId, string pType, ClientIdentification pIdentification, Device pDevice) : base(pId, pType, pIdentification)
        {
            this.device = pDevice;
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
                {"identification", identification.GetJObject()},
                {"device", device.GetJObject()}
            };

            return JsonConvert.SerializeObject(command);
        }
    }

    public enum ConnectionType
    {
        GetId, SetId, GetType, SetType, Disconnect
    }
}
