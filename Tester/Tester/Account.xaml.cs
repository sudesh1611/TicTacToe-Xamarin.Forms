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
    public partial class Account : ContentPage
    {
        public Account()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            fullName.Text = Application.Current.Properties["FullName"] as string;
            userNameEmail.Text = (Application.Current.Properties["UserName"] as string) + " | " + (Application.Current.Properties["EmailID"] as string);
            singlePlayerEasyStats.Text = "Won:" + (Application.Current.Properties["SingleEasyWins"] as string) + " | Lost:" + (Application.Current.Properties["SingleEasyLoses"] as string) + " | Tied:"+(Application.Current.Properties["SingleEasyTies"] as string);
            singlePlayerHardStats.Text = "Won:" + (Application.Current.Properties["SingleHardWins"] as string) + " | Lost:" + (Application.Current.Properties["SingleHardLoses"] as string) + " | Tied:" + (Application.Current.Properties["SingleHardTies"] as string);
            multiPlayerStats.Text = "Won:" + (Application.Current.Properties["DoubleWins"] as string) + " | Lost:" + (Application.Current.Properties["DoubleLoses"] as string) + " | Tied:" + (Application.Current.Properties["DoubleTies"] as string);
        }

        private async void DeActivateButton_Clicked(object sender, EventArgs e)
        {
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
    }
}