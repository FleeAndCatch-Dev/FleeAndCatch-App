using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Diagnostics;
using System.Threading;
using Commands;
using Commands.Components;
using Communication;
using Xamarin.Forms;
using static Commands.Devices.Robots.Position;
using UIKit;

namespace FleeAndCatch_App.pages.content.control
{
    public partial class Control : ContentPage
    {
        private Commands.Devices.Robots.Robot robot;
        private SpeedType speed;
        private DirectionType direction;
        private CancellationToken token;
        private Task refreshTask;
        private bool refresh;

        public Control(Commands.Devices.Robots.Robot robot)
        {
            InitializeComponent();

            this.robot = robot;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = Title + " " + Convert.ToString(robot.Identification.Id);

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += refreshView;

            refresh = true;
            //refreshTask = new Task(Refresh);
            //refreshTask = new Task(Refresh);
            //refreshTask.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                Commands.Control cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.Control.ToString(), Client.Identification, robot, new Steering((int)direction, (int)speed));
                Client.SendCmd(cmd.GetCommand());
                return true;
            });

        }

        protected override void OnDisappearing()
        {
            refresh = false;
            var cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.End.ToString(), Client.Identification, robot, new Steering(0, 0));
            Client.SendCmd(cmd.GetCommand());
            CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);

            base.OnDisappearing();
        }

        private void refreshView(object s, SensorValueChangedEventArgs a)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (a.SensorType)
                {
                    case MotionSensorType.Accelerometer:

                        LX.Text = ((MotionVector)a.Value).X.ToString("F");
                        LY.Text = ((MotionVector)a.Value).Y.ToString("F");
                        LZ.Text = ((MotionVector)a.Value).Z.ToString("F");
                        break;
                    case MotionSensorType.Gyroscope:
                        break;
                    case MotionSensorType.Magnetometer:
                        break;
                    case MotionSensorType.Compass:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var x = Convert.ToDouble(((MotionVector)a.Value).X.ToString("F"));
                var y = Convert.ToDouble(((MotionVector)a.Value).Y.ToString("F"));
                var z = Convert.ToDouble(((MotionVector)a.Value).Z.ToString("F"));

                if (x <= 0.2 && x >= -0.2)
                {
                    //Gerade aus
                    if (y >= 0.1)
                    {
                        LResult.Text = "Schneller";
                        speed = SpeedType.Faster;
                    }
                    else if (y <= -0.1)
                    {
                        LResult.Text = "Langsamer";
                        speed = SpeedType.Slower;
                    }
                    else
                    {
                        LResult.Text = "Nichts";
                        direction = DirectionType.StraightOn;
                        speed = SpeedType.Equal;
                    }
                }
                else
                {
                    if (x >= 0.1)
                    {
                        LResult.Text = "Rechts";
                        direction = DirectionType.Right;
                    }
                    else if (x <= -0.1)
                    {
                        LResult.Text = "Links";
                        direction = DirectionType.Left;
                    }
                }
            });
        }

        private void Refresh()
        {
            /*for (long i = 0; i < long.MaxValue; i++)
            {
                Commands.Control cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.Control.ToString(), Client.Identification, robot, new Steering((int)direction, (int)speed));
                Client.SendCmd(cmd.GetCommand());
                refreshTask.Wait(50);
            }*/


            //while (refresh)
            //{
                
            //    //await Task.Delay(TimeSpan.FromMilliseconds(50), token);
            //}
            
        }
    }
}
