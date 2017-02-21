using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;

namespace FleeAndCatch_App.Models
{
    [ImplementPropertyChanged]
    public class RobotModel
    {
        private RobotIdentificationModel identification;
        protected string active;
        protected PositionModel position;
        protected string speed;
        protected string ultrasonic;
        protected string gyro;

        /// <summary>
        /// Create an object of an robot for a json command.
        /// </summary>
        /// <param name="pId">Id of the robot in communication.</param>
        public RobotModel(RobotIdentificationModel pIdentification, bool pActive, PositionModel pPosition, double pSpeed, double pUltrasonic, double pGyro)
        {
            identification = pIdentification;
            active = Convert.ToString(pActive);
            position = pPosition;
            var tempSpeed = (int)(pSpeed * 100);
            speed = Convert.ToString(((double)tempSpeed) / 100) + " cm/s";
            var tempUltrasonic = (int)(pUltrasonic * 100);
            ultrasonic = Convert.ToString("U: " + ((double)tempUltrasonic) / 100) + " m";
            var tempGyro = (int)(pGyro * 100);
            gyro = Convert.ToString("G: " + ((double)tempGyro) / 100) + " °";
        }

        public RobotModel(Robot pRobot)
        {
            identification = new RobotIdentificationModel(pRobot.Identification);
            active = Convert.ToString(pRobot.Active);
            position = new PositionModel(pRobot.Position);
            var tempSpeed = (int)(pRobot.Speed * 100);
            speed = Convert.ToString(((double)tempSpeed) / 100) + " cm/s";
            var tempUltrasonic = (int)(pRobot.Ultrasonic * 100);
            ultrasonic = Convert.ToString("U: " + ((double)tempUltrasonic) / 100) + " m";
            var tempGyro = (int)(pRobot.Gyro * 100);
            gyro = Convert.ToString("G: " + ((double)tempGyro) / 100) + " °";
        }

        public RobotIdentificationModel Identification => identification;
        public string Active => active;
        public PositionModel Position => position;
        public string Speed => speed;
        public string Ultrasonic => ultrasonic;
        public string Gyro => gyro;
    }

    [ImplementPropertyChanged]
    public class RobotIdentificationModel
    {
        protected string id;
        protected string type;
        private string subtype;
        private string roletype;

        public RobotIdentificationModel(int pId, string pType, string pSubType, string pRoleType)
        {
            id = Convert.ToString(pId);
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pType)));
            subtype = Enum.GetName(typeof(ComponentType.RobotType), ((ComponentType.RobotType)Enum.Parse(typeof(ComponentType.RobotType), pSubType)));
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pRoleType)));
        }
        public RobotIdentificationModel(RobotIdentification pIdentification)
        {
            id = Convert.ToString(pIdentification.Id);
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pIdentification.Type)));
            subtype = Enum.GetName(typeof(ComponentType.RobotType), ((ComponentType.RobotType)Enum.Parse(typeof(ComponentType.RobotType), pIdentification.Subtype)));
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pIdentification.Roletype)));
        }

        public string Id => id;
        public string Type => type;
        public string Subtype => subtype;
        public string Roletype => roletype;
    }

    [ImplementPropertyChanged]
    public class PositionModel
    {
        private string x;
        private string y;
        private string orientation;

        public PositionModel(double pX, double pY, double pOrientation)
        {
            var tempX = (int) (pX * 100);
            x = "X: " + Convert.ToString(((double) tempX) / 100) + " mm";
            var tempY = (int)(pY * 100);
            y = "Y: " + Convert.ToString(((double)tempY) / 100) + " mm";
            var tempO = (int)(pOrientation * 100);
            orientation = "O: " + Convert.ToString(((double)tempO) / 100) + " °";
        }
        public PositionModel(Position pPosition)
        {
            var tempX = (int)(pPosition.X * 100);
            x = "X: " + Convert.ToString(((double)tempX) / 100) + " mm";
            var tempY = (int)(pPosition.Y * 100);
            y = "Y: " + Convert.ToString(((double)tempY) / 100) + " mm";
            var tempO = (int)(pPosition.Orientation * 100);
            orientation = "O: " + Convert.ToString(((double)tempO) / 100) + " °";
        }

        public string X => x;
        public string Y => y;
        public string Orientation => orientation;
    }
}
