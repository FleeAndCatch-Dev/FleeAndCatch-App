using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Communication;
using FleeAndCatch_App.Models;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<RobotModel> Robots { get; set; }
        public Szenario Szenario { get; set; }
        private bool accept;

        public override void Init(object initData)
        {
            base.Init(initData);

            Szenario = Client.Szenario;
            accept = false;

            if (Szenario == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: new", "The szenario doesn't exist", "OK");
                });
                return;
            }
            //Generate robot models for user interface
            Robots = new List<RobotModel>();
            foreach (var t in Szenario.Robots)
                Robots.Add(new RobotModel(t));
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            if (!accept)
            {
                foreach (var t in Szenario.Robots)
                    t.Active = false;

                //Send szenario end command
                Szenario.Command = SzenarioCommandType.Undefined.ToString();
                var command = new SzenarioCommand(CommandType.Szenario.ToString(), SzenarioCommandType.End.ToString(), Client.Identification, Szenario);
                Client.SendCmd(command.ToJsonString());
            }

            base.ViewIsDisappearing(sender, e);
        }

        public Command BStart_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    accept = true;
                    //Send szenario begin command to start the szenario
                    Szenario.Command = ControlType.Begin.ToString();
                    var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), Szenario.Type, Client.Identification, Szenario);
                    Client.SendCmd(cmd.ToJsonString());

                    Client.Szenario = Szenario;
                    await CoreMethods.PushPageModel<SzenarioPageModel>();
                    RaisePropertyChanged();
                });
            }
        }
    }
}
