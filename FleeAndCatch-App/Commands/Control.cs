using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands.Devices.Robots;
using Commands.Identifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Commands.Devices.Robots.Position;

namespace Commands
{
    public class Control : Command
    {
        [JsonProperty("robot")]
        private Robot robot;
        [JsonProperty("steering")]
        private Steering steering;

        public Control(string pId, string pType, ClientIdentification pIdentification, Robot pRobot, Steering pSteering) : base(pId, pType, pIdentification)
        {
            this.robot = pRobot;
            this.steering = pSteering;
        }

        public override string GetCommand()
        {
            var control = new JObject
            {
                {"robot", robot.GetJObject()},
                {"control",  steering.GetJObject()}
            };

            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"identification", identification.GetJObject()},
                {"control", control}
            };

            return JsonConvert.SerializeObject(command);
        }
    }

    public enum ControlType
    {
        Begin, End, Start, Stop
    }
}
