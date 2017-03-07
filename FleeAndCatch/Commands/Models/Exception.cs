using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models
{
    public class Exception
    {
        [JsonProperty("type")]
        private string type;
        [JsonProperty("message")]
        private string message;
        [JsonProperty("device")]
        [JsonConverter(typeof(DeviceJsonConverter))]
        private Device device;

        public Exception(string pType, string pMessage, Device pDevice)
        {
            this.type = pType;
            this.message = pMessage;
            this.device = pDevice;
        }

        [JsonIgnore]
        public string Type => type;
        [JsonIgnore]
        public string Message => message;
        [JsonIgnore]
        public Device Device => device;
    }
}
