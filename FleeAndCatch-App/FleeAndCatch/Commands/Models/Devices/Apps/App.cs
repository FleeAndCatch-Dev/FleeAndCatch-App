using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Devices.Apps
{
    public class App : IDevice
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
