using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
