using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Diagnostics;
using Xamarin.Forms;
using Robots;

namespace FleeAndCatch_App.pages.content.control
{
    public partial class Control : ContentPage
    {
        private Commands.Robot robot;

        public Control(Commands.Robot robot)
        {
            InitializeComponent();

            this.robot = robot;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = Title + " " + Convert.ToString(robot.Identification.Id);

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += (s, a) =>
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
                            LResult.Text = "Langsamer";
                        }
                        else if (y <= -0.1)
                        {
                            LResult.Text = "Schneller";
                        }
                        else
                        {
                            LResult.Text = "Nichts";
                        }
                    }
                    else
                    {
                        if (x >= 0.1)
                        {
                            LResult.Text = "Rechts";
                        }
                        else if (x <= -0.1)
                        {
                            LResult.Text = "Links";
                        }
                    }
                });
                    
            };
        }

        protected override void OnDisappearing()
        {
            CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);

            base.OnDisappearing();
        }
    }
}
