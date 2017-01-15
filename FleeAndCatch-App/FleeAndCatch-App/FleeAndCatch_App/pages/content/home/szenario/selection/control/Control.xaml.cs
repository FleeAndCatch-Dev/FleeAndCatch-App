using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using Commands.Devices.Robots;
using Communication;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.home.szenario.selection.control
{
    public partial class Control : ContentPage
    {
        private Robot robot;
        private Position.SpeedType speed;
        private Position.DirectionType direction;
        private CancellationToken token;
        private bool refresh;

        public Control(Robot pRobot)
        {
            InitializeComponent();

            this.robot = pRobot;
            Title = "Control " + robot.Identification.Subtype;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = Title + " " + Convert.ToString(robot.Identification.Id);

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += RefreshView;

            refresh = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(20), NewControlCmd);
        }

        protected override void OnDisappearing()
        {
            refresh = false;           
            CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);
            base.OnDisappearing();
        }

        private bool NewControlCmd()
        {
            if (!refresh) return false;
            Commands.Control cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.Control.ToString(), Client.Identification, robot, new Position.Steering((int)direction, (int)speed));
            Client.SendCmd(cmd.GetCommand());
            return true;
        }

        private void RefreshView(object s, SensorValueChangedEventArgs a)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                double x = 0, y = 0, z = 0;

                switch (Device.OS)
                {
                    case TargetPlatform.WinPhone:
                        x = Convert.ToDouble(((MotionVector)a.Value).X.ToString("F"));
                        y = Convert.ToDouble(((MotionVector)a.Value).Y.ToString("F"));
                        z = Convert.ToDouble(((MotionVector)a.Value).Z.ToString("F"));
                        break;
                    case TargetPlatform.Windows:
                        x = Convert.ToDouble(((MotionVector)a.Value).X.ToString("F"));
                        y = Convert.ToDouble(((MotionVector)a.Value).Y.ToString("F"));
                        z = Convert.ToDouble(((MotionVector)a.Value).Z.ToString("F"));
                        //await DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    case TargetPlatform.Android:
                        x = Convert.ToDouble(((MotionVector)a.Value).X.ToString("F")) / 10;
                        y = Convert.ToDouble(((MotionVector)a.Value).Y.ToString("F")) / 10;
                        z = Convert.ToDouble(((MotionVector)a.Value).Z.ToString("F")) / 10;
                        break;
                    case TargetPlatform.iOS:
                        await DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    case TargetPlatform.Other:
                        await DisplayAlert("Error", "Not supported OS", "OK");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (Device.Idiom == TargetIdiom.Phone)
                {
                    if (x <= 0.25 && x >= -0.25)
                    {
                        //Gerade aus
                        if (y >= 0.25)
                            speed = Position.SpeedType.Faster;
                        else if (y <= -0.25)
                            speed = Position.SpeedType.Slower;
                        else
                        {
                            direction = Position.DirectionType.StraightOn;
                            speed = Position.SpeedType.Equal;
                        }
                    }
                    else
                    {
                        //Drehen
                        if (x >= 0.25)
                            direction = Position.DirectionType.Right;
                        else if (x <= -0.25)
                            direction = Position.DirectionType.Left;
                    }
                }
                else if(Device.Idiom == TargetIdiom.Tablet)
                {
                    if (y <= 0.25 && y >= -0.25)
                    {
                        //Gerade aus
                        if (x >= 0.25)
                            speed = Position.SpeedType.Faster;
                        else if (x <= -0.25)
                            speed = Position.SpeedType.Slower;
                        else
                        {
                            direction = Position.DirectionType.StraightOn;
                            speed = Position.SpeedType.Equal;
                        }
                    }
                    else
                    {
                        //Drehen
                        if (y >= 0.25)
                            direction = Position.DirectionType.Right;
                        else if (y <= -0.25)
                            direction = Position.DirectionType.Left;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Not supported target", "OK");
                }
            });
        }

        private void BChange_OnClicked(object sender, EventArgs e)
        {
            if (Math.Abs(robot.Speed) < 1)
            {
                //Start
                BChange.Text = "Stop";
                BChange.BackgroundColor = Color.FromHex("#8B0000");

                Commands.Control cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.Start.ToString(), Client.Identification, robot, new Position.Steering((int)direction, (int)speed));
                Client.SendCmd(cmd.GetCommand());
            }
            else
            {
                //Stop
                BChange.Text = "Start";
                BChange.BackgroundColor = Color.FromHex("#006400");

                Commands.Control cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.Stop.ToString(), Client.Identification, robot, new Position.Steering((int)direction, (int)speed));
                Client.SendCmd(cmd.GetCommand());
            }
        }
    }
}
