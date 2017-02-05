using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Szenarios
{
    public class Control : Szenario
    {
        [JsonProperty("steering")]
        private Steering steering;

        public Control(string pSzenarioId, string pSzenarioType, string pMode, List<App> pApps, List<Robot> pRobots, Steering pSteering) : base(pSzenarioId, pSzenarioType, pMode, pApps, pRobots)
        {
            this.steering = pSteering;
        }

        public override JObject GetJObject()
        {
            var appArray = new JArray();
            foreach (var t in apps)
                appArray.Add(t.GetJObject());

            var robotArray = new JArray();
            foreach (var t in robots)
                robotArray.Add(t.GetJObject());

            var jsonControl = new JObject
            {
                {"szenarioid", szenarioid},
                {"szenariotype", szenariotype},
                {"mode", mode},
                {"apps", appArray},
                {"robots", robotArray},
                {"steering", steering.GetJObject()}
            };
            return jsonControl;
        }

        public Steering Steering => steering;
    }

    public enum ControlType
    {
        Begin, End, Start, Stop, Control
    }
}
