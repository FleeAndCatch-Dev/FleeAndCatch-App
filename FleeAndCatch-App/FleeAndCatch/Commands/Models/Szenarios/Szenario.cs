using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Szenarios
{
    public abstract class Szenario
    {
        [JsonProperty("id")]
        protected int id;
        [JsonProperty("type")]
        protected string type;
        [JsonProperty("command")]
        protected string command;
        [JsonProperty("mode")]
        protected string mode;
        [JsonProperty("apps")]
        protected List<App> apps;
        [JsonProperty("robots")]
        protected List<Robot> robots;

        protected Szenario(int pId, string pType, string pCommand, string pMode, List<App> pApps, List<Robot> pRobots)
        {
            this.id = pId;
            this.type = pType;
            this.command = pCommand;
            this.mode = pMode;
            this.apps = pApps;
            this.robots = pRobots;
        }

        [JsonIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonIgnore]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [JsonIgnore]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        [JsonIgnore]
        public string Mode => mode;
        [JsonIgnore]
        public List<App> Apps => apps;
        [JsonIgnore]
        public List<Robot> Robots => robots;
    }

    public enum SzenarioMode
    {
        Single, Multi
    }
}
