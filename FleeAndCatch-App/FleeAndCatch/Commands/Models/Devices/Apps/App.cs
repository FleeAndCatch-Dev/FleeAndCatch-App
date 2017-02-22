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
        [JsonConverter(typeof(IdentificationJsonConverter))]
        private AppIdentification identification;

        public App(AppIdentification pAppIdentification) : base(false)
        {
            identification = pAppIdentification;
        }

        [JsonIgnore]
        public AppIdentification Identification => identification;
    }
}
