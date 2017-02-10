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

        [JsonIgnore]
        public Steering Steering => steering;
    }

    public enum ControlType
    {
        Begin, End, Start, Stop, Control
    }
}
