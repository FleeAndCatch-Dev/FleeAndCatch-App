using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Commands.Devices.Robots;
using Communication;
using FleeAndCatch_App.pages.content.home.szenario.selection.control;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.home.szenario.selection
{
    public partial class RobotInformation : ContentPage
    {
        private Robot robot;

        public RobotInformation(Robot pRobot)
        {
            InitializeComponent();

            this.robot = pRobot;
            Title = pRobot.Identification.Subtype;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Title = robot.Identification.Subtype;

            LId.Text = Convert.ToString(robot.Identification.Id);
            LType.Text = robot.Identification.Type;
            LSubtype.Text = robot.Identification.Subtype;
            LRoletype.Text = robot.Identification.Roletype;

            LX.Text = Convert.ToString(robot.Position.X);
            LY.Text = Convert.ToString(robot.Position.Y);
            LOrientation.Text = Convert.ToString(robot.Position.Orientation);

            LSpeed.Text = Convert.ToString(robot.Speed);
        }

        protected override void OnDisappearing()
        {
            var cmd = new Commands.Control(CommandType.Control.ToString(), ControlType.End.ToString(), Client.Identification, robot, new Position.Steering(0, 0));
            Client.SendCmd(cmd.GetCommand());

            base.OnDisappearing();
        }

        private async void BStart_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new control.Control(robot));
        }
    }
}
