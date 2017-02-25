using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FleeAndCatch.Commands
{
    public class ConnectionCommand : Command
    {
        [JsonProperty("device")]
        [JsonConverter(typeof(DeviceJsonConverter))]
        private Device device;

        /// <summary>
        /// Create an object of a connection command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as connaction type.</param>
        /// <param name="pClient">Client for representation of the device.</param>
        public ConnectionCommand(string pId, string pType, ClientIdentification pIdentification, Device pDevice) : base(pId, pType, pIdentification)
        {
            this.device = pDevice;
        }
    }

    public enum ConnectionCommandType
    {
        Connect, Disconnect, Init
    }
}
