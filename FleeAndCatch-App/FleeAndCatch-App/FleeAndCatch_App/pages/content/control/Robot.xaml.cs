using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using ComponentType;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.control
{
    public partial class Robot : ContentPage
    {
        private Commands.Robot robot;

        public Robot(Commands.Robot robot)
        {
            InitializeComponent();

            this.robot = robot;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Title = robot.Identification.Subtype;

            LId.Text = Convert.ToString(robot.Identification.Id);
            LType.Text = robot.Identification.Type;
            LSubtype.Text = robot.Identification.Subtype;
            LAddress.Text = robot.Identification.Address;
            LPort.Text = Convert.ToString(robot.Identification.Port);

            LX.Text = Convert.ToString(robot.Position.X);
            LY.Text = Convert.ToString(robot.Position.Y);
            LOrientation.Text = Convert.ToString(robot.Position.Orientation);

            LSpeed.Text = Convert.ToString(robot.Speed);
        }

        private async void BStart_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pages.content.control.Control(robot));
        }
    }
}
