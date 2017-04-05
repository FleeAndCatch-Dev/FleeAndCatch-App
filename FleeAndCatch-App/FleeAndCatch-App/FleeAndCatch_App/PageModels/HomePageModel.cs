using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch_App.Pages;
using PropertyChanged;
using Xamarin.Forms;
using FleeAndCatch.Communication;
using Exception = FleeAndCatch.Exception;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class HomePageModel : FreshMvvm.FreshBasePageModel
    {
        public Command BSzenario_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<OptionPageModel>();
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
                    await CoreMethods.PushPageModel<HelpPageModel>();
                    RaisePropertyChanged();
                });
            }
        }
        public Command BLogOut_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!Client.Connected) return;
                        Client.Close();

                        var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<SignInPageModel>();
                        var navigation = new FreshMvvm.FreshNavigationContainer(page)
                        {
                            BarBackgroundColor = Color.FromHex("#008B8B"),
                            BarTextColor = Color.White
                        };
                        Application.Current.MainPage = navigation;
                    }
                    catch (Exception ex)
                    {
                        await CoreMethods.DisplayAlert("Error: " + Convert.ToString(ex.Id), ex.Message, "OK");
                    }
                });
            }
        }
    }
}
