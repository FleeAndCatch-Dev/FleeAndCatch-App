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
        [JsonProperty("mode")]
        protected string mode;
        [JsonProperty("apps")]
        protected List<App> apps;
        [JsonProperty("robots")]
        protected List<Robot> robots;

        protected Szenario(string pSzenarioId, string pSzenarioType, string pMode, List<App> pApps, List<Robot> pRobots)
        {
            this.szenarioid = pSzenarioId;
            this.szenariotype = pSzenarioType;
            this.mode = pMode;
            this.apps = pApps;
            this.robots = pRobots;
        }

        public abstract JObject GetJObject();

        public string SzenarioId => szenarioid;

        public string SzenarioType
        {
            get { return szenariotype; }
            set { szenariotype = value; }
        }

        public string Mode => mode;
        public List<App> Apps => apps;
        public List<Robot> Robots => robots;
    }

    public enum SzenarioMode
    {
        Single, Multi
    }
}
