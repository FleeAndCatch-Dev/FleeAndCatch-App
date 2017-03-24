using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Szenarios;
using PropertyChanged;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioOptionsPageModel : FreshMvvm.FreshBasePageModel
    {
        public Command BControl_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<RobotListPageModel>(SzenarioCommandType.Control);
                    RaisePropertyChanged();
                });
            }
        }
        public Command BSynchron_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<RobotListPageModel>(SzenarioCommandType.Synchron);
                    RaisePropertyChanged();
                });
            }
        }
        public Command BFollow_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<RobotListPageModel>(SzenarioCommandType.Follow);
                    RaisePropertyChanged();
                });
            }
        }
        public Command BFlee_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: 399", "Sorry, this isn't implemented", "OK");
                });
            }
        }
        public Command BCatch_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: 399", "Sorry, this isn't implemented", "OK");
                });
            }
        }
    }
}
