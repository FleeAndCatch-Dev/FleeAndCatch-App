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
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class ControlPageModel : FreshMvvm.FreshBasePageModel
    {
        public RobotModel Robot { get; set; }
        public string Change { get; set; }
        public Color ChangeColor { get; set; }
        public ImageSource ImageSource { get; set; }
        private Szenario _szenario;
        private Robot _robot;
        private Steering.SpeedType _speed;
        private Steering.DirectionType _direction;

        /// <summary>
        /// Set the current robot and the szenario type
        /// </summary>
        /// <param name="initData"></param>
        public override void Init(object initData)
        {
            base.Init(initData);

            _szenario = initData as Szenario;
            if (_szenario == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: 320", "The szenario doesn't exist", "OK");
                });
                return;
            }
            _robot = _szenario.Robots[0];
            Robot = new RobotModel(_robot);
            _szenario.Command = ControlType.Control.ToString();
        }

        /// <summary>
        /// Init motion control and start a timer for this methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            Change = "Stop";
            ChangeColor = Color.FromHex("#8B0000");

            //Start the sensors on the current device
            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += RefreshView;

            //Start timer for the control commands
            SzenarioController.Refresh = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(50), NewControlCmd);
        }

        /// <summary>
        /// Stop the motion control, send a end command and navigate to the root page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            //Stop the timer and navigate to the home page
            SzenarioController.Refresh = false;
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
                    if (Math.Abs(Convert.ToDouble(_robot.Speed)) < 0.1)
                    {
                        //Start
                        Change = "Stop";
                        ChangeColor = Color.FromHex("#8B0000");

                        _szenario.Command = ControlType.Start.ToString();
                    }
                    else
                    {
                        //Stop
                        Change = "Start";
                        ChangeColor = Color.FromHex("#006400");

                        _szenario.Command = ControlType.Stop.ToString();
                    }
                    var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, _szenario);
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
                    if (_robot.Identification.Id != t.Identification.Id) continue;
                    Robot = new RobotModel(t);
                    break;
                }
                SzenarioController.Changed = false;
            }

            if (!SzenarioController.Refresh)
            {
                //stop sensors
                //navigate to startpage
                CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);
                //set object active -> false
                _szenario.Robots[0].Active = false;
                Client.Device.Active = false;
                //remove the szenario
                SzenarioController.Szenarios.Remove(_szenario);

                //Send the control end command
                _szenario.Command = ControlType.Undefined.ToString();
                var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), SzenarioCommandType.End.ToString(), Client.Identification, _szenario);
                Client.SendCmd(JsonConvert.SerializeObject(cmd));

                var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<HomePageModel>();
                var navigation = new FreshMvvm.FreshNavigationContainer(page)
                {
                    BarBackgroundColor = Color.FromHex("#008B8B"),
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = navigation;

                return false;
            }
            var control = (Control)_szenario;
            control.Command = ControlType.Control.ToString();
            control.Steering.Direction = _direction.ToString();
            control.Steering.Speed = _speed.ToString();
            //Send the control command
            var cmdCtrl = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, control);
            Client.SendCmd(cmdCtrl.ToJsonString());
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
                    case TargetPlatform.WinPhone:
                    case TargetPlatform.Windows:
                        x = Convert.ToDouble(((MotionVector)e.Value).X.ToString("F"));
                        y = Convert.ToDouble(((MotionVector)e.Value).Y.ToString("F"));
                        break;
                    case TargetPlatform.Android:
                        x = (Convert.ToDouble(((MotionVector)e.Value).X.ToString("F")) / 10) * (-1);
                        y = (Convert.ToDouble(((MotionVector)e.Value).Y.ToString("F")) / 10) * (-1);
                        break;
                    case TargetPlatform.iOS:
                        await CoreMethods.DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    case TargetPlatform.Other:
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
                            ImageSource = ImageSource.FromFile("ic_expand_less_black_48dp.png");
                        }
                        else if (y <= -0.2)
                        {
                            _speed = Steering.SpeedType.Slower;
                            ImageSource = ImageSource.FromFile("ic_expand_more_black_48dp.png");
                        }
                        else
                        {
                            _direction = Steering.DirectionType.StraightOn;
                            _speed = Steering.SpeedType.Equal;
                            ImageSource = ImageSource.FromFile("");
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
                                ImageSource = ImageSource.FromFile("");
                            }
                            else
                            {
                                _direction = Steering.DirectionType.Right;
                                ImageSource = ImageSource.FromFile("ic_chevron_right_black_48dp.png");
                            }
                        }
                        else if (x <= -0.2)
                        {
                            if (_direction == Steering.DirectionType.Right)
                            {
                                _direction = Steering.DirectionType.StraightOn;
                                ImageSource = ImageSource.FromFile("");
                            }
                            else
                            {
                                _direction = Steering.DirectionType.Left;
                                ImageSource = ImageSource.FromFile("ic_chevron_left_black_48dp.png");
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
                            ImageSource = ImageSource.FromFile("ic_expand_less_black_48dp.png");
                        }
                        else if (x <= -0.2)
                        {
                            _speed = Steering.SpeedType.Slower;
                            ImageSource = ImageSource.FromFile("ic_expand_more_black_48dp.png");
                        }
                        else
                        {
                            _direction = Steering.DirectionType.StraightOn;
                            _speed = Steering.SpeedType.Equal;
                            ImageSource = ImageSource.FromFile("");
                        }
                    }
                    else
                    {
                        //Drehen
                        if (y >= 0.2)
                        {
                            _direction = Steering.DirectionType.Right;
                            ImageSource = ImageSource.FromFile("ic_chevron_right_black_48dp.png");
                        }                           
                        else if (y <= -0.2)
                        {
                            _direction = Steering.DirectionType.Left;
                            ImageSource = ImageSource.FromFile("ic_chevron_left_black_48dp.png");
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
