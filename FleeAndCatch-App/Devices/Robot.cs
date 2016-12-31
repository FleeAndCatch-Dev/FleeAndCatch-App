using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Robot : Device
    {
        [JsonProperty("identification")]
        private Identification identification;
        [JsonProperty("active")]
        private bool active;
        [JsonProperty("position")]
        private Position position;
        [JsonProperty("speed")]
        private double speed;

        /// <summary>
        /// Create an object of an robot for a json command.
        /// </summary>
        /// <param name="pId">Id of the robot in communication.</param>
        public Robot(Identification pIdentification, bool pActive, Position pPosition, double pSpeed)
        {
            identification = pIdentification;
            active = pActive;
            position = pPosition;
            speed = pSpeed;
        }

        /// <summary>
        /// Get a json obejct of the robot.
        /// </summary>
        /// <returns>Json object.</returns>
        public JObject GetJObject()
        {
            var jsonRobot = new JObject
            {
                {"identification", identification.GetJObject()},
                {"active", active},
                { "position", position.GetJObject()},
                {"speed", speed}
            };
            return jsonRobot;
        }

        public Identification Identification => identification;
        public bool Active => active;
        public Position Position => position;
        public double Speed => speed;
    }

    public class Position
    {
        [JsonProperty("x")]
        private double x;
        [JsonProperty("y")]
        private double y;
        [JsonProperty("orientation")]
        private double orientation;

        public Position(double pX, double pY, double pOrientation)
        {
            x = pX;
            y = pY;
            orientation = pOrientation;
        }

        public JObject GetJObject()
        {
            var jsonPosition = new JObject
            {
                {"x", x},
                {"y", y},
                {"orientation", orientation}
            };
            return jsonPosition;
        }

        public double X => x;
        public double Y => y;
        public double Orientation => orientation;
    }
}
