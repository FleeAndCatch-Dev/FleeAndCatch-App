using System;
using FleeAndCatch_App.pages.content.home.szenario;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.home
{
    public partial class Szenario : ContentPage
    {
        public Szenario()
        {
            InitializeComponent();
        }

        private async void BNewSzenario_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewSzenario());
        }

        private async void BSpectator_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Spectator());
        }

        private async void BHelp_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }
    }
}
