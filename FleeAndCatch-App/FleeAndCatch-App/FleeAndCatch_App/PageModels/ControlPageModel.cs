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
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Controller;
using PropertyChanged;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class ControlPageModel : FreshMvvm.FreshBasePageModel
    {
        public Robot Robot { get; set; }
        public string Change { get; set; }
        public Color ChangeColor { get; set; }
        public ImageSource ImageSource { get; set; }
        private Szenario szenario;
        private Steering.SpeedType speed;
        private Steering.DirectionType direction;
        private bool refresh;

        public ControlPageModel()
        {
            
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            szenario = initData as Szenario;
            if (szenario == null) return;
            Robot = szenario.Robots[0];
            szenario.SzenarioType = ControlType.Control.ToString();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            Change = "Stop";
            ChangeColor = Color.FromHex("#8B0000");

            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Ui);
            CrossDeviceMotion.Current.SensorValueChanged += RefreshView;

            refresh = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(25), NewControlCmd);
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            refresh = false;
            CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);

            szenario.Robots[0].Active = false;
            szenario.SzenarioType = ControlType.End.ToString();
            var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, szenario);
            Client.SendCmd(cmd.GetCommand());

            var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<HomePageModel>();
            var navigation = new FreshMvvm.FreshNavigationContainer(page)
            {
                BarBackgroundColor = Color.FromHex("#008B8B"),
                BarTextColor = Color.White
            };
            Application.Current.MainPage = navigation;

            base.ViewIsDisappearing(sender, e);
        }

        public Command BChange_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Math.Abs(Robot.Speed) < 1)
                    {
                        //Start
                        Change = "Stop";
                        ChangeColor = Color.FromHex("#8B0000");

                        szenario.SzenarioType = ControlType.Start.ToString();
                    }
                    else
                    {
                        //Stop
                        Change = "Start";
                        ChangeColor = Color.FromHex("#006400");

                        szenario.SzenarioType = ControlType.Stop.ToString();
                    }
                    var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, szenario);
                    Client.SendCmd(cmd.GetCommand());
                });
            }
        }

        private bool NewControlCmd()
        {
            if (!refresh) return false;
            var control = (Control)szenario;
            control.Steering.Directiond = direction.ToString();
            control.Steering.Speed = speed.ToString();
            foreach (var t in RobotController.Robots)
            {
                if (t.Identification == Robot.Identification)
                    Robot = t;
            }

            //var robotList = new List<Robot> { Robot };

            /*var cmdSync = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationType.Current.ToString(), Client.Identification, robotList);
            Client.SendCmd(cmdSync.GetCommand());*/

            var cmdCtrl = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, control);
            Client.SendCmd(cmdCtrl.GetCommand());
            return true;
        }

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
                        x = Convert.ToDouble(((MotionVector)e.Value).X.ToString("F")) / 10;
                        y = Convert.ToDouble(((MotionVector)e.Value).Y.ToString("F")) / 10;
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
                {
                    if (x <= 0.25 && x >= -0.25)
                    {
                        //Gerade aus
                        if (y >= 0.25)
                        {
                            speed = Steering.SpeedType.Faster;
                            ImageSource = ImageSource.FromFile("Images/ic_expand_less_black_48dp.png");
                        } 
                        else if (y <= -0.25)
                        {
                            speed = Steering.SpeedType.Slower;
                            ImageSource = ImageSource.FromFile("Images/ic_expand_more_black_48dp.png");
                        } 
                        else
                        {
                            direction = Steering.DirectionType.StraightOn;
                            speed = Steering.SpeedType.Equal;
                            ImageSource = ImageSource.FromFile("");
                        }
                    }
                    else
                    {
                        //Drehen
                        if (x >= 0.25)
                        {
                            direction = Steering.DirectionType.Right;
                            ImageSource = ImageSource.FromFile("Images/ic_chevron_right_black_48dp.png");
                        }                            
                        else if (x <= -0.25)
                        {
                            direction = Steering.DirectionType.Left;
                            ImageSource = ImageSource.FromFile("Images/ic_chevron_left_black_48dp.png");
                        }
                            
                    }
                }
                else if (Device.Idiom == TargetIdiom.Tablet)
                {
                    if (y <= 0.25 && y >= -0.25)
                    {
                        //Gerade aus
                        if (x >= 0.25)
                            speed = Steering.SpeedType.Faster;
                        else if (x <= -0.25)
                            speed = Steering.SpeedType.Slower;
                        else
                        {
                            direction = Steering.DirectionType.StraightOn;
                            speed = Steering.SpeedType.Equal;
                        }
                    }
                    else
                    {
                        //Drehen
                        if (y >= 0.25)
                            direction = Steering.DirectionType.Right;
                        else if (y <= -0.25)
                            direction = Steering.DirectionType.Left;
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
