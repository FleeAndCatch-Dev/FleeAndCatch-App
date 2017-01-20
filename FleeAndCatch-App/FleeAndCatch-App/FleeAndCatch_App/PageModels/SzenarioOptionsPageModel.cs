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
        public SzenarioOptionsPageModel()
        {
            
        }

        public Command BControl_OnCommand
        {
            get
            {
                return new Command( () =>
                {
                    CoreMethods.PushPageModel<RobotListPageModel>(SzenarioCommandType.Control);
                    RaisePropertyChanged();
                });
            }
        }
        public Command BSynchron_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.DisplayAlert("Error", "Sorry, this isn't implemented", "OK");
                    /*CoreMethods.PushPageModel<RobotListPageModel>(SzenarioType.Synchron);
                    RaisePropertyChanged();*/
                });
            }
        }
        public Command BFollow_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.DisplayAlert("Error", "Sorry, this isn't implemented", "OK");
                    /*CoreMethods.PushPageModel<RobotListPageModel>(SzenarioType.Follow);
                    RaisePropertyChanged();*/
                });
            }
        }
    }
}
