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
            speed = Convert.ToString(pSpeed) + " cm/s";
            ultrasonic = "U: " + Convert.ToString(pUltrasonic) + " m";
            gyro = "G: " + Convert.ToString(pGyro) + " °";
        }

        public RobotModel(Robot pRobot)
        {
            identification = new RobotIdentificationModel(pRobot.Identification);
            active = Convert.ToString(pRobot.Active);
            position = new PositionModel(pRobot.Position);
            speed = pRobot.Speed + " cm/s";
            ultrasonic = pRobot.Ultrasonic+ " m";
            gyro = pRobot.Gyro + " °";
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
            x = "X: " + Convert.ToString(((double) pX) / 100) + " mm";
            y = "Y: " + Convert.ToString(((double) pY) / 100) + " mm";
            orientation = "O: " + Convert.ToString(pOrientation) + " °";
        }
        public PositionModel(Position pPosition)
        {
            x = "X: " + Convert.ToString(pPosition.X) + " mm";
            y = "Y: " + Convert.ToString(pPosition.Y) + " mm";
            orientation = "O: " + Convert.ToString(pPosition.Orientation) + " °";
        }

        public string X => x;
        public string Y => y;
        public string Orientation => orientation;
    }
}
