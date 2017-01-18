using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

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
                    CoreMethods.PushPageModel<RobotListPageModel>();
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
                    //await Application.Current.MainPage.Navigation.PushAsync(new Szenario());
                });
            }
        }
        public Command BFollow_OnCommand
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
