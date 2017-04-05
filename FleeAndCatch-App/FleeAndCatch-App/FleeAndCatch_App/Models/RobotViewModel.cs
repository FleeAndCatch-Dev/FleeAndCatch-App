using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using PropertyChanged;

namespace FleeAndCatch_App.Models
{
    [ImplementPropertyChanged]
    public class RobotViewModel
    {
        private RobotIdentification identification;
        private Position position;
        private double speed;
        private string ultrasonic;
        private string gyro;
        private string imageUrl;

        public RobotViewModel(Robot pRobot)
        {
            identification = pRobot.Identification;
            position = pRobot.Position;
            speed = pRobot.Speed;
            ultrasonic = Convert.ToString(pRobot.Ultrasonic);
            gyro = Convert.ToString(pRobot.Gyro);

            switch ((RobotType) Enum.Parse(typeof(RobotType), pRobot.Identification.Subtype))
            {
                case RobotType.ThreeWheelDrive:
                    imageUrl = "FleeAndCatch_Logo.png";
                    break;
                default:
                    imageUrl = "FleeAndCatch_Logo.png";
                    break;
            }
        }
    }
}
