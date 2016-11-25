using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Xamarin.Forms;
using Robots;

namespace FleeAndCatch_App.pages.detail
{
    public partial class Home : ContentPage
    {
        private bool synchronised;

        public Home()
        {
            InitializeComponent();
        }

        private void UpdateRobotList()
        {
            var robots = RobotController.Robots;
        }

        private void BSynchronise_OnClicked(object sender, EventArgs e)
        {
            /*Client.SendCmd("{\"id\":\"Home\",\"type\":\"GetRobots\",\"apiid\":\"@@fleeandcatch@@\",\"errorhandling\":\"ignoreerrors\",\"client\":{\"id\":" + Client.Id + ",\"type\":\"App\"}}");
            //await Task.Delay(TimeSpan.FromMilliseconds(100));
            Device.BeginInvokeOnMainThread(UpdateRobotList);    */      
        }
    }
}
