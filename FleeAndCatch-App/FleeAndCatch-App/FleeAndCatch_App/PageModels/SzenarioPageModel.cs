using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Communication;
using FleeAndCatch.Controller;
using FleeAndCatch_App.Models;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;
using static Xamarin.Forms.TargetPlatform;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioPageModel : FreshMvvm.FreshBasePageModel
    {
        public RobotModel Robot { get; set; }
        public string Change { get; set; }
        public Color ChangeColor { get; set; }
        public string DirectionImage { get; set; }

        private Steering.SpeedType _speed;
        private Steering.DirectionType _direction;

        public override void Init(object initData)
        {
            base.Init(initData);

            if (Client.Szenario == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: 320", "The szenario doesn't exist", "OK");
                });
                return;
            }
            Robot = new RobotModel(Client.Szenario.Robots[0]);

            Client.Szenario.Command = Client.Szenario.Type;
            _speed = 0;
            _direction = 0;
            Change = "Stop";
            ChangeColor = Color.FromHex("#8B0000");
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            //Start the sensors on the current device
            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += RefreshView;

            //Start timer for the control commands
            SzenarioController.Refresh = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(50), NewControlCmd);
        }
        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            //Stop the timer and navigate to the home page
            SzenarioController.Refresh = false;

            base.ViewIsDisappearing(sender, e);
        }

        /// <summary>
        /// Starts and stops the robot
        /// </summary>
        public Command BChange_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Change == "Start")
                    {
                        //Start
                        Change = "Stop";
                        ChangeColor = Color.FromHex("#8B0000");

                        Client.Szenario.Command = ControlType.Start.ToString();
                    }
                    else
                    {
                        //Stop
                        Change = "Start";
                        ChangeColor = Color.FromHex("#006400");

                        Client.Szenario.Command = ControlType.Stop.ToString();
                    }
                    var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), Client.Szenario.Type, Client.Identification, Client.Szenario);
                    Client.SendCmd(cmd.ToJsonString());
                });
            }
        }

        /// <summary>
        /// Start a new control command and sends it
        /// </summary>
        /// <returns></returns>
        private bool NewControlCmd()
        {
            //Change the user interface
            if (SzenarioController.Changed)
            {
                foreach (var t in RobotController.Robots)
                {
                    if (Convert.ToInt32(Robot.Identification.Id) != t.Identification.Id) continue;
                    Robot = new RobotModel(t);
                    break;
                }
                SzenarioController.Changed = false;
            }

            if (!SzenarioController.Refresh)
            {
                //stop sensors
                CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);
                //set object active -> false
                foreach (var t in Client.Szenario.Robots)
                    t.Active = false;
                Client.Device.Active = false;
                //remove the szenario
                SzenarioController.Szenarios.Remove(Client.Szenario);

                //Send the control end command
                Client.Szenario.Command = ControlType.Undefinied.ToString();
                var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), SzenarioCommandType.End.ToString(), Client.Identification, Client.Szenario);
                Client.SendCmd(JsonConvert.SerializeObject(cmd));

                Client.Szenario = null;

                //navigate to startpage
                var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<HomePageModel>();
                var navigation = new FreshMvvm.FreshNavigationContainer(page)
                {
                    BarBackgroundColor = Color.FromHex("#008B8B"),
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = navigation;

                return false;
            }

            Client.Szenario.Command = ControlType.Control.ToString();
            Client.Szenario.Steering = new Steering((int)_direction, (int)_speed);

            var command = new SzenarioCommand(CommandType.Szenario.ToString(), Client.Szenario.Type, Client.Identification, Client.Szenario);
            Client.SendCmd(command.ToJsonString());

            return true;
        }

        /// <summary>
        /// Refresh the user interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshView(object sender, SensorValueChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                double x = 0, y = 0, z = 0;

                switch (Device.OS)
                {
                    case WinPhone:
                    case Windows:
                        x = Convert.ToDouble(((MotionVector)e.Value).X.ToString("F"));
                        y = Convert.ToDouble(((MotionVector)e.Value).Y.ToString("F"));
                        break;
                    case Android:
                        x = (Convert.ToDouble(((MotionVector)e.Value).X.ToString("F")) / 10) * (-1);
                        y = (Convert.ToDouble(((MotionVector)e.Value).Y.ToString("F")) / 10) * (-1);
                        break;
                    case iOS:
                        await CoreMethods.DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    case Other:
                        await CoreMethods.DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (Device.Idiom == TargetIdiom.Phone)
                    if (x <= 0.2 && x >= -0.2)
                    {
                        //Gerade aus
                        if (y >= 0.2)
                        {
                            _speed = Steering.SpeedType.Faster;
                            DirectionImage = "ic_expand_less_black_48dp.png";
                        }
                        else if (y <= -0.2)
                        {
                            _speed = Steering.SpeedType.Slower;
                            DirectionImage = "ic_expand_more_black_48dp.png";
                        }
                        else
                        {
                            _direction = Steering.DirectionType.StraightOn;
                            _speed = Steering.SpeedType.Equal;
                            DirectionImage = "";
                        }
                    }
                    else
                    {
                        //Drehen
                        if (x >= 0.2)
                        {
                            if (_direction == Steering.DirectionType.Left)
                            {
                                _direction = Steering.DirectionType.StraightOn;
                                DirectionImage = "";
                            }
                            else
                            {
                                _direction = Steering.DirectionType.Right;
                                DirectionImage = "ic_chevron_right_black_48dp.png";
                            }
                        }
                        else if (x <= -0.2)
                        {
                            if (_direction == Steering.DirectionType.Right)
                            {
                                _direction = Steering.DirectionType.StraightOn;
                                DirectionImage = "";
                            }
                            else
                            {
                                _direction = Steering.DirectionType.Left;
                                DirectionImage = "ic_chevron_left_black_48dp.png";
                            }
                        }
                    }
                else if (Device.Idiom == TargetIdiom.Tablet)
                {
                    if (y <= 0.2 && y >= -0.2)
                    {
                        //Gerade aus
                        if (x >= 0.2)
                        {
                            _speed = Steering.SpeedType.Faster;
                            DirectionImage = "ic_expand_less_black_48dp.png";
                        }
                        else if (x <= -0.2)
                        {
                            _speed = Steering.SpeedType.Slower;
                            DirectionImage = "ic_expand_more_black_48dp.png";
                        }
                        else
                        {
                            _direction = Steering.DirectionType.StraightOn;
                            _speed = Steering.SpeedType.Equal;
                            DirectionImage = "";
                        }
                    }
                    else
                    {
                        //Drehen
                        if (y >= 0.2)
                        {
                            _direction = Steering.DirectionType.Right;
                            DirectionImage = "ic_chevron_right_black_48dp.png";
                        }
                        else if (y <= -0.2)
                        {
                            _direction = Steering.DirectionType.Left;
                            DirectionImage = "ic_chevron_left_black_48dp.png";
                        }
                    }
                }
                else
                {
                    await CoreMethods.DisplayAlert("Error", "Not supported target", "OK");
                }
            });
        }
    }
}
