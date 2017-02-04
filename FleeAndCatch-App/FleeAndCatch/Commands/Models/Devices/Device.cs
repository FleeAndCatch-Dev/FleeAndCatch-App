using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Devices
{
    public abstract class Device
    {
        [JsonProperty("active")]
        protected bool active;
        private bool pActive;

        protected Device(bool pActive)
        {
            this.pActive = pActive;
        }

        public abstract JObject GetJObject();

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
    }

    public class DeviceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Device));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object device = null;

            //Specification for the desrialisation of the device
            try
            {
                device = serializer.Deserialize<App>(reader);
            }
            catch
            {
                device = serializer.Deserialize<Robot>(reader);
            }

            return device;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }
    }
}
