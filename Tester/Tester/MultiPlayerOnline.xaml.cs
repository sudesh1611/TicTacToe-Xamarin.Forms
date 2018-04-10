using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiPlayerOnline : ContentPage
    {
        string PlayerName = "";
        string Result = "";
        public MultiPlayerOnline()
        {
            InitializeComponent();
            mainLabel.ScaleTo(1.1, 3000, Easing.Linear);
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            Application.Current.Properties["OnlinePlayerName"] = "";
            overlay.IsVisible = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AnimateThis(null, null);
            Task.Run(() =>
            {
                refreshButton_Clicked(null, null);
            });
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
            overlay.IsVisible = true;
            LoadingIndicatorText.Text = "Searching For Player..";
            await Task.Run(async () =>
            {
                await ConnectToGame();
                Result = "";
                while (Result=="")
                {
                    await WaitingForPlayer();
                    await Task.Delay(3000);
                }
                if (Result == "Success")
                {
                    Device.BeginInvokeOnMainThread( () =>
                    {
                        LoadingIndicatorText.Text = "Player Found. Initializing Game...";
                    });
                    await CheckIfOpponentAlive();
                    if(Result=="True")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            overlay.IsVisible = false;
                            Navigation.PushAsync(new PlayMultiplayerOnline());
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PlayerOnline.Text = "Lost Connection With Player";
                            overlay.IsVisible = false;
                        });
                    }
                }
            });
            
        }

        private async Task ConnectToGame()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/ConnectToGame";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PlayerName", Application.Current.Properties["FullName"] as string), new KeyValuePair<string, string>("PlayerID", Application.Current.Properties["ID"] as string) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Result = "";
                            PlayerOnline.Text = "Server Is Down. Try Again After Some Time";
                            beginButton.IsVisible = false;
                            refreshButton.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Result = "";
                        PlayerOnline.Text = "Check Your Internet Connection";
                        beginButton.IsVisible = false;
                        refreshButton.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                    return;
                }
            }
        }

        private async Task WaitingForPlayer()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/WaitingForPlayer";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("ID", Application.Current.Properties["ID"] as string) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent == "True")
                        {
                            Result = "Success";
                        }
                        else
                        {
                            Result = "";
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Result = "Server Error";
                            PlayerOnline.Text = "Server Is Down. Try Again After Some Time";
                            beginButton.IsVisible = false;
                            refreshButton.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Result = "Connection Error";
                        PlayerOnline.Text = "Check Your Internet Connection";
                        beginButton.IsVisible = false;
                        refreshButton.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                    return;
                }
            }
        }

        private async void refreshButton_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                overlay.IsVisible = true;
                LoadingIndicatorText.Text = "Loading...";
            });
            string url = "http://tictactoe.sudeshkumar.me/Android/OnlinePlayers";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, null);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent == "NoPlayers")
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PlayerOnline.Text = "You Are Online";
                                Application.Current.Properties["OnlinePlayerName"] = "";
                                beginButton.IsVisible = true;
                                refreshButton.IsVisible = false;
                                overlay.IsVisible = false;
                            });
                        }
                        else
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PlayerOnline.Text = myContent + " Is Online";
                                Application.Current.Properties["OnlinePlayerName"] = myContent;
                                beginButton.IsVisible = true;
                                refreshButton.IsVisible = false;
                                overlay.IsVisible = false;
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PlayerOnline.Text = "Server Is Down. Try Again After Some Time";
                            beginButton.IsVisible = false;
                            refreshButton.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PlayerOnline.Text = "Check Your Internet Connection";
                        beginButton.IsVisible = false;
                        refreshButton.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                quitButton.IsVisible = true;
            });
        }

        private async void quitButton_Clicked(object sender, EventArgs e)
        {
            Result = "Quitting";
            string url = "http://tictactoe.sudeshkumar.me/Android/EmptyThePlayersFile";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("id", Application.Current.Properties["ID"] as string) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, null);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent == "Deleted")
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PlayerOnline.Text = "You Are Online";
                                Application.Current.Properties["OnlinePlayerName"] = "";
                                beginButton.IsVisible = true;
                                refreshButton.IsVisible = false;
                                overlay.IsVisible = false;
                            });
                        }
                        else
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                beginButton.IsVisible = true;
                                refreshButton.IsVisible = false;
                                overlay.IsVisible = false;
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PlayerOnline.Text = "Server Is Down. Try Again After Some Time";
                            beginButton.IsVisible = false;
                            refreshButton.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PlayerOnline.Text = "Check Your Internet Connection";
                        beginButton.IsVisible = false;
                        refreshButton.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                }
            }
        }

        protected override void OnDisappearing()
        {
            Result = "Backing";
            quitButton_Clicked(null, null);
            base.OnDisappearing();
        }

        private async Task CheckIfOpponentAlive()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/CheckIfOpponentAlive";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("ID", Application.Current.Properties["ID"] as string) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent == "True")
                        {
                            Result = "True";
                        }
                        else
                        {
                            Result = "False";
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Result = "Server Error";
                            PlayerOnline.Text = "Server Is Down. Try Again After Some Time";
                            beginButton.IsVisible = false;
                            refreshButton.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Result = "Connection Error";
                        PlayerOnline.Text = "Check Your Internet Connection";
                        beginButton.IsVisible = false;
                        refreshButton.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                    return;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Result = "Backing";
            if (overlay.IsVisible)
            {
                quitButton_Clicked(null, null);
                return base.OnBackButtonPressed();
            }
            else
            {
                return base.OnBackButtonPressed();
            }
            
        }
    }
}