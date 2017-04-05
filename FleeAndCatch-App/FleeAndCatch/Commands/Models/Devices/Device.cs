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
            if (reader.TokenType == JsonToken.StartArray)
            {
                List<Device> devices = null;
                var jsonArray = JArray.Load(reader);

                foreach (var t in jsonArray)
                {
                    if (jsonArray["identification"]["type"] == null) throw new System.Exception("Szenario is not implemented");
                    switch (jsonArray["identification"]["type"].ToString())
                    {
                        case "App":
                            devices.Add(t.ToObject<App>());
                            break;
                        case "Robot":
                            devices.Add(t.ToObject<Robot>());
                            break;
                    }
                }

                return devices;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                Device device = null;
                var jsonObject = JObject.Load(reader);

                if (jsonObject["identification"]["type"] == null) throw new System.Exception("Devie is not implemented");
                switch (jsonObject["identification"]["type"].ToString())
                {
                    case "App":
                        device = jsonObject.ToObject<App>();
                        break;
                    case "Robot":
                        device = jsonObject.ToObject<Robot>();
                        break;
                }
                return device;
            }
            throw new System.Exception("Not defined JsonToken");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            if(t.Type != JTokenType.Object)
                t.WriteTo(writer);
            else
            {
                var o = (JObject)t;
                o.WriteTo(writer);
            }
        }
    }
}
