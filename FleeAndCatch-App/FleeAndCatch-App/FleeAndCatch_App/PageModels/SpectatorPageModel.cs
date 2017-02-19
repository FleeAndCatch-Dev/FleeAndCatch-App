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
using FleeAndCatch_App.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SpectatorPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<RobotModel> Robots { get; set; }

        private Szenario _szenario;

        public override void Init(object initData)
        {
            base.Init(initData);

            _szenario = initData as Szenario;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            //Start timer for synchronization commands
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
                    if (_szenario.Id != t.Id) continue;
                    _szenario = t;
                    Robots = new List<RobotModel>();
                    foreach (var t1 in _szenario.Robots)
                        Robots.Add(new RobotModel(t1));
                    break;
                }
                SzenarioController.Changed = false;
            }

            if (!SzenarioController.Refresh)
                return false;

            //Send synchronization command to get current szenario
            var szenarios = new List<Szenario> {_szenario};
            var cmdSync = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.CurrentSzenario.ToString(), Client.Identification, szenarios, new List<Robot>());
            Client.SendCmd(cmdSync.ToJsonString());
            return true;
        }
    }
}
