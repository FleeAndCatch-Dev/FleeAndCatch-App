using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Szenarios
{
    public abstract class Szenario
    {
        [JsonProperty("szenarioid")]
        protected string szenarioid;
        [JsonProperty("szenariotype")]
        protected string szenariotype;
        [JsonProperty("apps")]
        protected List<App> apps;
        [JsonProperty("robots")]
        protected List<Robot> robots;

        protected Szenario(string pSzenarioId, string pSzenarioType, List<App> pApps, List<Robot> pRobots)
        {
            this.szenarioid = pSzenarioId;
            this.szenariotype = pSzenarioType;
            this.apps = pApps;
            this.robots = pRobots;
        }

        public abstract JObject GetJObject();

        /*var appArray = new JArray();
            foreach (var t in apps)
                appArray.Add(t.GetJObject());

            var robotArray = new JArray();
            foreach (var t in robots)
                robotArray.Add(t.GetJObject());

            var jsonSzenario = new JObject
            {
                {"szenarioid", szenarioid},
                {"active", active},
                {"apps", appArray},
                {"robots", robotArray}
            };
            return jsonSzenario;*/

        public string SzenarioId => szenarioid;

        public string SzenarioType
        {
            get { return szenariotype; }
            set { szenariotype = value; }
        }
        public List<App> Apps => apps;
        public List<Robot> Robots => robots;
    }
}
