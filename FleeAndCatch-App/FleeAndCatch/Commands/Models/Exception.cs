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
        [JsonConverter(typeof(DeviceConverter))]
        private IDevice device;

        public Exception(string pType, string pMessage, IDevice pDevice)
        {
            this.type = pType;
            this.message = pMessage;
            this.device = pDevice;
        }

        public JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"type", type},
                {"message", message},
                {"device", device.GetJObject()}
            };
            return jsonIdentification;
        }

        public string Type => type;
        public string Message => message;
        public IDevice Device => device;
    }
}
