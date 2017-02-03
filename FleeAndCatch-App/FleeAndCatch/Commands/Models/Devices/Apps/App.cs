using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Devices.Apps
{
    public class App : Device
    {
        [JsonProperty("identification")]
        private AppIdentification identification;

        public App(AppIdentification pAppIdentification) : base(false)
        {
            identification = pAppIdentification;
        }

        public override JObject GetJObject()
        {
            var jsonApp = new JObject
            {
                {"identification", identification.GetJObject()},
                {"active", active}
            };
            return jsonApp;
        }

        public AppIdentification Identification => identification;
    }
}
