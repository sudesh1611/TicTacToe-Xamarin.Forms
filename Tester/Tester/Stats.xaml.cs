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
    public partial class Stats : ContentPage
    {
        public Stats()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            singlePlayerEasyStats.Text = "Won:" + (Application.Current.Properties["SingleEasyWins"] as string) + " | Lost:" + (Application.Current.Properties["SingleEasyLoses"] as string) + " | Tied:" + (Application.Current.Properties["SingleEasyTies"] as string);
            singlePlayerHardStats.Text = "Won:" + (Application.Current.Properties["SingleHardWins"] as string) + " | Lost:" + (Application.Current.Properties["SingleHardLoses"] as string) + " | Tied:" + (Application.Current.Properties["SingleHardTies"] as string);
            multiPlayerStats.Text = "Won:" + (Application.Current.Properties["DoubleWins"] as string) + " | Lost:" + (Application.Current.Properties["DoubleLoses"] as string) + " | Tied:" + (Application.Current.Properties["DoubleTies"] as string);
        }

        private void DeactivateButton_Clicked(object sender, EventArgs e)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicatorText.IsVisible = false;
            PasswordEntry.IsVisible = true;
            Cancel.IsVisible = true;
            overlay.IsVisible = true;
            mainError.IsVisible = false;
        }

        private async Task DeactivateAccount()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/DeActivateAccount";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("UserName", Application.Current.Properties["UserName"] as string), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"] as string) });
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
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                overlay.IsVisible = false;
                                Application.Current.Properties["UserName"] = "null";
                                Application.Current.MainPage = new Login();
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                mainError.Text = "Can't Reach Server. Try Again After Some Time";
                                mainError.IsVisible = true;
                                overlay.IsVisible = false;
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainError.Text = "Server Is Down. Try Again After Some Time";
                            mainError.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
                catch (Exception)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mainError.Text = "Check Your Internet Connection";
                        mainError.IsVisible = true;
                        overlay.IsVisible = false;
                    });
                }

            }
        }

        private async void LogOutButton_Clicked(object sender, EventArgs e)
        {
            LoadingIndicatorText.Text = "Logging Out...";
            LoadingIndicator.IsVisible = true;
            LoadingIndicatorText.IsVisible = true;
            PasswordEntry.IsVisible = false;
            Cancel.IsVisible = false;
            overlay.IsVisible = true;
            mainError.IsVisible = false;
            await Task.Run(async () =>
            {
                string url = "http://tictactoe.sudeshkumar.me/Android/UpdateStatsMob";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("UserName", Application.Current.Properties["UserName"] as string), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"] as string), new KeyValuePair<string, string>("SingleEasyWinsCount", Application.Current.Properties["SingleEasyWinsCount"] as string), new KeyValuePair<string, string>("SingleEasyLosesCount", Application.Current.Properties["SingleEasyLosesCount"] as string), new KeyValuePair<string, string>("SingleEasyTiesCount", Application.Current.Properties["SingleEasyTiesCount"] as string), new KeyValuePair<string, string>("SingleHardWinsCount", Application.Current.Properties["SingleHardWinsCount"] as string), new KeyValuePair<string, string>("SingleHardLosesCount", Application.Current.Properties["SingleHardLosesCount"] as string), new KeyValuePair<string, string>("SingleHardTiesCount", Application.Current.Properties["SingleHardTiesCount"] as string), new KeyValuePair<string, string>("DoubleWinsCount", Application.Current.Properties["DoubleWinsCount"] as string), new KeyValuePair<string, string>("DoubleLosesCount", Application.Current.Properties["DoubleLosesCount"] as string), new KeyValuePair<string, string>("DoubleTiesCount", Application.Current.Properties["DoubleTiesCount"] as string) });
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                        HttpResponseMessage response = await getResponse;
                        if (response.IsSuccessStatusCode)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                overlay.IsVisible = false;
                                Application.Current.Properties["UserName"] = "null";
                                Application.Current.MainPage = new Login();
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                mainError.Text = "Server Is Down. Try Again After Some Time";
                                mainError.IsVisible = true;
                                overlay.IsVisible = false;
                            });
                        }
                    }
                    catch (Exception)
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainError.Text = "Check Your Internet Connection";
                            mainError.IsVisible = true;
                            overlay.IsVisible = false;
                        });
                    }
                }
            });
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            PasswordEntry.IsVisible = false;
            Cancel.IsVisible = false;
            overlay.IsVisible = false;
        }

        private async void PasswordEntry_Completed(object sender, EventArgs e)
        {
            if(PasswordEntry.Text==null)
            {
                mainError.Text = "Password is Wrong";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            if(PasswordEntry.Text==Application.Current.Properties["Password"] as string)
            {
                await Task.Run(async () =>
                {
                    await DeactivateAccount();
                });
            }
            else
            {
                mainError.Text = "Password is Wrong";
                PasswordEntry.Text = "";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
        }
    }
}