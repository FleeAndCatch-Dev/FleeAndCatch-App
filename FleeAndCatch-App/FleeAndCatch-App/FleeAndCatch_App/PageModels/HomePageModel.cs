using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Pages;
using PropertyChanged;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class HomePageModel : FreshMvvm.FreshBasePageModel
    {
        public Command BSzenario_OnCommand
        {
            get
            {
                return new Command( () =>
                {
                    CoreMethods.PushPageModel<SzenarioPageModel>();
                    RaisePropertyChanged();
                });
            }
        }
        public Command BHelp_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    
                });
            }
        }
        public Command BLogOut_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Client.Connected)
                    {
                        Client.Close();

                        var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<SignInPageModel>();
                        var navigation = new FreshMvvm.FreshNavigationContainer(page)
                        {
                            BarBackgroundColor = Color.FromHex("#008B8B"),
                            BarTextColor = Color.White
                        };
                        Application.Current.MainPage = navigation;
                        return;
                    }
                    await CoreMethods.DisplayAlert("Error", "Ups, that should not happen, please start the application again", "OK");
                });
            }
        }
    }
}
