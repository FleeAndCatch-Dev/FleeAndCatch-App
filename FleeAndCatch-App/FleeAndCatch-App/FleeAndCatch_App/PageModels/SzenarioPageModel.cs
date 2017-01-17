using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    public class SzenarioPageModel : FreshMvvm.FreshBasePageModel
    {
        public SzenarioPageModel()
        {
            
        }

        public Command BNewSzenario_OnCommand
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
        public Command BSpectator_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    //await Application.Current.MainPage.Navigation.PushAsync(new Szenario());
                });
            }
        }
        public Command BHelp_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    //await Application.Current.MainPage.Navigation.PushAsync(new Szenario());
                });
            }
        }
    }
}
