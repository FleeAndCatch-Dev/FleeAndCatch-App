using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.control
{
    public partial class Robot : ContentPage
    {
        private Commands.Robot robot;

        public Robot(Commands.Robot robot)
        {
            this.robot = robot;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Title = robot.Identification.Subtype;
        }
    }
}
