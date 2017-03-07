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
        [JsonProperty("robotid")]
        private int robotid;

        public App(AppIdentification pAppIdentification, int pRobotId) : base(false)
        {
            identification = pAppIdentification;
            robotid = pRobotId;
        }

        [JsonIgnore]
        public AppIdentification Identification => identification;

        [JsonIgnore]
        public int RobotId
        {
            get { return robotid; }
            set { robotid = value; }
        }
    }
}
