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
            type = Enum.GetName(typeof(IdentificationType), ((IdentificationType)Enum.Parse(typeof(IdentificationType), pType)));
            subtype = Enum.GetName(typeof(RobotType), ((RobotType)Enum.Parse(typeof(RobotType), pSubType)));
            roletype = Enum.GetName(typeof(RoleType), ((RoleType)Enum.Parse(typeof(RoleType), pRoleType)));
        }
        public RobotIdentificationModel(RobotIdentification pIdentification)
        {
            id = Convert.ToString(pIdentification.Id);
            type = Enum.GetName(typeof(IdentificationType), ((IdentificationType)Enum.Parse(typeof(IdentificationType), pIdentification.Type)));
            subtype = Enum.GetName(typeof(RobotType), ((RobotType)Enum.Parse(typeof(RobotType), pIdentification.Subtype)));
            roletype = Enum.GetName(typeof(RoleType), ((RoleType)Enum.Parse(typeof(RoleType), pIdentification.Roletype)));
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
            x = "X: " + Convert.ToString(((double)((int)(pX * 0.1))) / 100) + " m";
            y = "Y: " + Convert.ToString(((double)((int)(pY * 0.1))) / 100) + " m";
            orientation = "O: " + Convert.ToString(((double)((int)(pOrientation * 100))) / 100) + " °";
        }
        public PositionModel(Position pPosition)
        {
            x = "X: " + Convert.ToString(((double)((int)(pPosition.X * 0.1))) / 100) + " m";
            y = "Y: " + Convert.ToString(((double)((int)(pPosition.Y * 0.1))) / 100) + " m";
            orientation = "O: " + Convert.ToString(((double)((int)(pPosition.Orientation * 100))) / 100) + " °";
        }

        public string X => x;
        public string Y => y;
        public string Orientation => orientation;
    }
}
