using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MultiPlayerOffline : ContentPage
	{
		public MultiPlayerOffline ()
		{
			InitializeComponent ();
            mainLabel.ScaleTo(1.1, 3000, Easing.Linear);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

        private async void StartGame_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["TempPlayerOne"] = player1Entry.Text;
            Application.Current.Properties["TempPlayerTwo"] = player2Entry.Text;
            Application.Current.Properties["temp1"] = "0";
            Application.Current.Properties["temp2"] = "0";
            Application.Current.Properties["temp3"] = "0";
            if (modeSwitch.IsToggled)
            {
                Application.Current.Properties["TempPlayerTurn"] = "Second";
            }
            else
            {
                Application.Current.Properties["TempPlayerTurn"] = "First";
            }
            await Navigation.PushAsync(new PlayMultiplayerOffline());
        }

        private void player1Entry_Focused(object sender, FocusEventArgs e)
        {
            player1Entry.Text = "";
        }

        private void player1Entry_Completed(object sender, EventArgs e)
        {
            if(player1Entry.Text=="")
            {
                player1Entry.Text = "Player 1";
                
            }
            if(player1Entry.Text.Length >14)
            {
                player1Entry.Text = player1Entry.Text.Substring(0, 14);
            }
            player1Label.Text = player1Entry.Text;
            player2Entry.Focus();
        }

        private void player2Entry_Focused(object sender, FocusEventArgs e)
        {
            player2Entry.Text = "";
        }

        private void player2Entry_Completed(object sender, EventArgs e)
        {
            if(player2Entry.Text=="")
            {
                player2Entry.Text = "Player 2";
            }
            if (player2Entry.Text.Length > 14)
            {
                player2Entry.Text = player2Entry.Text.Substring(0, 14);
            }
            Player2Label.Text = player2Entry.Text;
        }
    }
}