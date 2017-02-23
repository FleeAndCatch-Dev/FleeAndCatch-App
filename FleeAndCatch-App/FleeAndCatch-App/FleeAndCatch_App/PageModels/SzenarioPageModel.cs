using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Szenarios;
using PropertyChanged;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioPageModel : FreshMvvm.FreshBasePageModel
    {
        public Command BSingle_OnCommand
        {
            get
            {
                return new Command( () =>
                {
                    CoreMethods.PushPageModel<SzenarioOptionsPageModel>();
                    RaisePropertyChanged();
                });
            }
        }
        public Command BMulti_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.DisplayAlert("Error: 399", "Sorry, this isn't implemented", "OK");
                });
            }
        }
        public Command BSpectator_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<SzenarioListPageModel>();
                    RaisePropertyChanged();
                });
            }
        }
        public Command BHelp_OnCommand
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
