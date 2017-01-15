using System;
using System.Collections.Generic;
using System.Linq;
using Commands.Devices.Apps;
using Commands.Devices.Robots;
using Commands.Identifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands.Devices
{
    public interface Device
    {
        JObject GetJObject();
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
            throw new NotImplementedException();
        }
    }
}
