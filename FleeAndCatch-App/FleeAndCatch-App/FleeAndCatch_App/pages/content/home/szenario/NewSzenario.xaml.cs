using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.home.szenario
{
    public partial class NewSzenario : ContentPage
    {
        public NewSzenario()
        {
            InitializeComponent();
        }

        private async void BControl_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Robots());
        }

        private void BSynchron_OnClicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BFollow_OnClicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
