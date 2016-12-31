using Commands.Identifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands.Devices.Apps
{
    public class App : Device
    {
        [JsonProperty("identification")]
        private AppIdentification identification;
        [JsonProperty("active")]
        private bool active;

        public App(AppIdentification pAppIdentification)
        {
            identification = pAppIdentification;
            active = false;
        }

        public JObject GetJObject()
        {
            var jsonApp = new JObject
            {
                {"identification", identification.GetJObject()},
                {"active", active}
            };
            return jsonApp;
        }

        public AppIdentification Identification => identification;
        public bool Active => active;
    }
}
