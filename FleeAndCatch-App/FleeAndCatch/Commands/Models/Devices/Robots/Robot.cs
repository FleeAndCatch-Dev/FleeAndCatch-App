using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Devices.Robots
{
    public class Robot : Device
    {
        [JsonProperty("identification")]
        protected RobotIdentification identification;
        [JsonProperty("position")]
        protected Position position;
        [JsonProperty("speed")]
        protected string speed;
        [JsonProperty("ultrasonic")]
        protected string ultrasonic;
        [JsonProperty("gyro")]
        protected string gyro;

        /// <summary>
        /// Create an object of an robot for a json command.
        /// </summary>
        /// <param name="pId">Id of the robot in communication.</param>
        public Robot(RobotIdentification pIdentification, bool pActive, Position pPosition, double pSpeed, double pUltrasonic, double pGyro) : base(pActive)
        {
            identification = pIdentification;
            position = pPosition;
            speed = Convert.ToString(pSpeed);
            ultrasonic = Convert.ToString(pUltrasonic);
            gyro = Convert.ToString(pGyro);
        }

        [JsonIgnore]
        public RobotIdentification Identification => identification;
        [JsonIgnore]
        public Position Position => position;
        [JsonIgnore]
        public string Speed => speed;
        [JsonIgnore]
        public string Ultrasonic => ultrasonic;
        [JsonIgnore]
        public string Gyro => gyro;
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

        [JsonIgnore]
        public double X => x;
        [JsonIgnore]
        public double Y => y;
        [JsonIgnore]
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

        [JsonIgnore]
        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        [JsonIgnore]
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
