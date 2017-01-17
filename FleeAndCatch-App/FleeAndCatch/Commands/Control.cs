using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands
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
            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"identification", identification.GetJObject()},
                {"robot", robot.GetJObject()},
                {"steering",  steering.GetJObject()}
            };

            return JsonConvert.SerializeObject(command);
        }
    }

    public enum ControlType
    {
        Begin, End, Start, Stop, Control
    }
}
