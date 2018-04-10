using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tester.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SinglePlayer : ContentPage
	{
		public SinglePlayer ()
		{
			InitializeComponent ();
            
        }


        private async void StartGame_Clicked(object sender, EventArgs e)
        {
            if(modeSwitch.IsToggled)
            {
                await Navigation.PushAsync(new PlaySingleHard());
            }
            else
            {
                await Navigation.PushAsync(new PlaySingleEasy());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainLabel.ScaleTo(1.1, 3000, Easing.Linear);
            easyWins.Text = (Application.Current.Properties["SingleEasyWins"] as string) + "W";
            easyLoses.Text = (Application.Current.Properties["SingleEasyLoses"] as string) + "L";
            easyTies.Text = (Application.Current.Properties["SingleEasyTies"] as string) + "T";
            hardWins.Text = (Application.Current.Properties["SingleHardWins"] as string) + "W";
            hardLoses.Text = (Application.Current.Properties["SingleHardLoses"] as string) + "L";
            hardTies.Text = (Application.Current.Properties["SingleHardTies"] as string) + "T";
            AnimateThis(null, null);
        }

        private async void AnimateThis(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        beginButton.ScaleTo(1.2, 700, Easing.Linear);
                    });
                    await Task.Delay(900);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        beginButton.ScaleTo(1, 700, Easing.Linear);
                    });
                    await Task.Delay(900);
                }
            });
        }
    }
}