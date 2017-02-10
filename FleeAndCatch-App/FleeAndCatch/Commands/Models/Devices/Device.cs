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

        protected Device(bool active)
        {
            this.active = active;
        }

        [JsonIgnore]
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
    }

    public class DeviceJsonConverter : JsonConverter
    {
        private Type[] _types;

        public DeviceJsonConverter()
        {
            
        }

        public DeviceJsonConverter(params Type[] types)
        {
            _types = types;
        }

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
            var t = JToken.FromObject(value);
            if(t.Type != JTokenType.Object)
                t.WriteTo(writer);
            else
            {
                var device = value as Device;
                var o = (JObject)t;
                if (device == null) return;
                if (device is App)
                {
                }
                else if(device is Robot)
                {
                        
                }
                o.WriteTo(writer);
            }
        }
    }
}
