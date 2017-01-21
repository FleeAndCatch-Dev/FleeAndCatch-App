using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Devices.Robots
{
    public class Robot : IDevice
    {
        [JsonProperty("identification")]
        protected RobotIdentification identification;
        [JsonProperty("active")]
        protected bool active;
        [JsonProperty("position")]
        protected Position position;
        [JsonProperty("speed")]
        protected double speed;

        /// <summary>
        /// Create an object of an robot for a json command.
        /// </summary>
        /// <param name="pId">Id of the robot in communication.</param>
        public Robot(RobotIdentification pIdentification, bool pActive, Position pPosition, double pSpeed)
        {
            identification = pIdentification;
            active = pActive;
            position = pPosition;
            speed = pSpeed;
        }

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

        public RobotIdentification Identification => identification;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
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

    public class Steering
    {
        [JsonProperty("direction")]
        private string direction;
        [JsonProperty("speed")]
        private string speed;

        public Steering(int pDirection, int pSpeed)
        {
            this.direction = ((DirectionType)pDirection).ToString();
            this.speed = ((SpeedType)pSpeed).ToString();
        }

        public JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"direction", direction},
                {"speed", speed}
            };
            return jsonIdentification;
        }

        public string Directiond
        {
            get { return direction; }
            set { direction = value; }
        }

        public string Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public enum SpeedType
        {
            Slower = -1, Equal = 0, Faster = 1
        }

        public enum DirectionType
        {
            Left = -1, StraightOn = 0, Right = 1
        }
    }
}
