using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Tester.Models;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainLabel.ScaleTo(1.1, 3000, Easing.Linear);
        }

        private void userNameEntry_Completed(object sender, EventArgs e)
        {
            passwordEntry.Focus();
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = true;
            mainError.IsVisible = false;
            string UserName = userNameEntry.Text;
            string Password = passwordEntry.Text;
            if (string.IsNullOrEmpty(UserName))
            {
                userNameEntry.Text = "";
                mainError.Text = "Username Can't Be Empty";
                overlay.IsVisible = false;
                mainError.IsVisible = true;
                return;
            }
            if (UserName.Length > 15)
            {
                {
                    userNameEntry.Text = "";
                    mainError.Text = "Username Length Shouldn't Exceed 15";
                    mainError.IsVisible = true;
                    overlay.IsVisible = false;
                    return;
                }
            }
            if (string.IsNullOrEmpty(Password))
            {
                passwordEntry.Text = "";
                mainError.Text = "Password Can't Be Empty";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            if (Password.Length > 15)
            {
                passwordEntry.Text = "";
                mainError.Text = "Password Length Shouldn't Exceed 15";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            UserName = UserName.ToLower();
            await Task.Run(async () =>
            {
                string url = "http://tictactoe.sudeshkumar.me/Android/LogInUser";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("UserName", UserName), new KeyValuePair<string, string>("Password", Password) });
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                        HttpResponseMessage response = await getResponse;
                        if(response.IsSuccessStatusCode)
                        {
                            var myContent = await response.Content.ReadAsStringAsync();
                            if (myContent == "UserNameWrong")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    userNameEntry.Text = "";
                                    passwordEntry.Text = "";
                                    mainError.Text = "Username Is Incorrect";
                                    mainError.IsVisible = true;
                                });
                            }
                            else if (myContent == "PasswordWrong")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    passwordEntry.Text = "";
                                    mainError.Text = "Password Is Incorrect";
                                    mainError.IsVisible = true;
                                });
                            }
                            else
                            {
                                var user = JsonConvert.DeserializeObject<UserAccount>(myContent);
                                Application.Current.Properties["UserName"] = user.UserName;
                                Application.Current.Properties["FullName"] = user.FullName;
                                Application.Current.Properties["ID"] = user.ID.ToString();
                                Application.Current.Properties["SingleEasyWins"] = user.SingleEasyWins.ToString();
                                Application.Current.Properties["SingleEasyLoses"] = user.SingleEasyLoses.ToString();
                                Application.Current.Properties["SingleEasyTies"] = user.SingleEasyTies.ToString();
                                Application.Current.Properties["SingleHardWins"] = user.SingleHardWins.ToString();
                                Application.Current.Properties["SingleHardLoses"] = user.SingleHardLoses.ToString();
                                Application.Current.Properties["SingleHardTies"] = user.SingleHardTies.ToString();
                                Application.Current.Properties["DoubleWins"] = user.DoubleWins.ToString();
                                Application.Current.Properties["DoubleLoses"] = user.DoubleLoses.ToString();
                                Application.Current.Properties["DoubleTies"] = user.DoubleTies.ToString();
                                Application.Current.Properties["EmailID"] = user.EmailID;
                                Application.Current.Properties["Password"] = user.Password;
                                Application.Current.Properties["ConfirmPassword"] = user.ConfirmPassword;
                                Application.Current.Properties["SingleEasyWinsCount"] = "0";
                                Application.Current.Properties["SingleEasyLosesCount"] = "0";
                                Application.Current.Properties["SingleEasyTiesCount"] = "0";
                                Application.Current.Properties["SingleHardWinsCount"] = "0";
                                Application.Current.Properties["SingleHardLosesCount"] = "0";
                                Application.Current.Properties["SingleHardTiesCount"] = "0";
                                Application.Current.Properties["DoubleWinsCount"] = "0";
                                Application.Current.Properties["DoubleLosesCount"] = "0";
                                Application.Current.Properties["DoubleTiesCount"] = "0";
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    overlay.IsVisible = false;
                                    Application.Current.MainPage= new NavigationPage(new HomePage());
                                });
                            }
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                mainError.Text = "Server Is Down. Try Again After Some Time";
                                mainError.IsVisible = true;
                            });
                        }
                    }
                    catch (Exception)
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainError.Text = "Check Your Internet Connection";
                            mainError.IsVisible = true;
                        });
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    overlay.IsVisible = false;
                });
            });
            overlay.IsVisible = false;
        }

        private void passwordEntry_Completed(object sender, EventArgs e)
        {
            loginButton_Clicked(null, null);
        }

        private void signUpButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUp();
        }

        private void userNameEntry_Focused(object sender, FocusEventArgs e)
        {
            mainError.IsVisible = false;
        }

        private void passwordEntry_Focused(object sender, FocusEventArgs e)
        {
            mainError.IsVisible = false;
        }
    }
}