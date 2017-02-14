using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Controller;
using PropertyChanged;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SpectatorPageModel : FreshMvvm.FreshBasePageModel
    {
        public Szenario Szenario { get; set; }
        public List<Robot> Robots { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            Szenario = initData as Szenario;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            SzenarioController.Refresh = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(100), NewSpectatorCmd);
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            SzenarioController.Refresh = false;
            base.ViewIsDisappearing(sender, e);
        }

        private bool NewSpectatorCmd()
        {
            //Change the user interface
            if (SzenarioController.Changed)
            {
                foreach (var t in SzenarioController.Szenarios)
                {
                    if (Szenario.Id != t.Id) continue;
                    Szenario = t;
                    Robots = t.Robots;
                    break;
                }
                SzenarioController.Changed = false;
            }

            if (!SzenarioController.Refresh)
                return false;

            //do something
            var szenarios = new List<Szenario> {Szenario};
            var cmdSync = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.CurrentSzenario.ToString(), Client.Identification, szenarios, new List<Robot>());
            Client.SendCmd(cmdSync.ToJsonString());
            return true;
        }
    }
}
