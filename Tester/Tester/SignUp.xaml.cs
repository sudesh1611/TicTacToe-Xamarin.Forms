using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
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

        private void nameEntry_Completed(object sender, EventArgs e)
        {
            userNameEntry.Focus();
        }

        private void userNameEntry_Completed(object sender, EventArgs e)
        {
            emailEntry.Focus();
        }

        private void emailEntry_Completed(object sender, EventArgs e)
        {
            emailError.IsVisible = false;
            if (string.IsNullOrEmpty(emailEntry.Text))
            {
                emailError.Text = "Email ID Can't Be Empty";
                emailError.IsVisible = true;
                emailEntry.Focus();
            }
            else
            {
                var regex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
                var match = Regex.Match(emailEntry.Text, regex, RegexOptions.IgnoreCase);
                if(match.Success)
                {
                    passwordEntry.Focus();
                }
                else
                {
                    emailError.Text = "Enter A Valid Email ID";
                    emailError.IsVisible = true;
                    emailEntry.Focus();
                }
            }
        }

        private void passwordEntry_Completed(object sender, EventArgs e)
        {
            confirmPasswordEntry.Focus();
        }

        private void confirmPasswordEntry_Completed(object sender, EventArgs e)
        {
            confirmPasswordError.IsVisible = false;
            if (string.IsNullOrEmpty(passwordEntry.Text))
            {
                confirmPasswordError.Text = "Enter A Valid Password";
                confirmPasswordEntry.Text = "";
                confirmPasswordError.IsVisible = true;
            }
            else
            {
                if(string.IsNullOrEmpty(confirmPasswordEntry.Text))
                {
                    confirmPasswordError.Text = "Passwords Don't Match";
                    confirmPasswordError.IsVisible = true;
                }
                else if(confirmPasswordEntry.Text!=passwordEntry.Text)
                {
                    confirmPasswordError.Text = "Passwords Don't Match";
                    confirmPasswordError.IsVisible = true;
                }
                else
                {
                    signUpButton_Clicked(null, null);
                }
            }
        }

        private async void signUpButton_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = true;
            mainError.IsVisible = false;
            string Name = nameEntry.Text;
            string UserName = userNameEntry.Text;
            string EmailID = emailEntry.Text;
            string Password = passwordEntry.Text;
            string ConfirmPassword = confirmPasswordEntry.Text;
            if(string.IsNullOrEmpty(Name))
            {
                nameEntry.Text = "";
                mainError.Text = "Name Can't Be Empty";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            else if(Name.Length>15)
            {
                nameEntry.Text = "";
                mainError.Text = "Name Length Shouldn't Exceed 15";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            if (string.IsNullOrEmpty(UserName))
            {
                userNameEntry.Text = "";
                mainError.Text = "Username Can't Be Empty";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            else if (UserName.Length > 15)
            {
                userNameEntry.Text = "";
                mainError.Text = "Username Length Shouldn't Exceed 15";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                passwordEntry.Text = "";
                mainError.Text = "Password Can't Be Empty";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            else if (Password.Length > 15)
            {
                passwordEntry.Text = "";
                mainError.Text = "Password Length Shouldn't Exceed 15";
                mainError.IsVisible = true;
                overlay.IsVisible = false;
                return;
            }
            UserName = UserName.ToLower();
            EmailID = EmailID.ToLower();
            await Task.Run(async () =>
            {
                string url = "http://tictactoe.sudeshkumar.me/Android/SignUpUser";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("UserName", UserName), new KeyValuePair<string, string>("FullName", Name), new KeyValuePair<string, string>("EmailID", EmailID), new KeyValuePair<string, string>("Password", Password) });
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                        HttpResponseMessage response = await getResponse;
                        if (response.IsSuccessStatusCode)
                        {
                            var myContent = await response.Content.ReadAsStringAsync();
                            if (myContent == "UserNameExists")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    userNameEntry.Text = "";
                                    mainError.Text = "Username Isn't Available";
                                    mainError.IsVisible = true;
                                });
                            }
                            else if (myContent == "EmailIDExists")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    emailEntry.Text = "";
                                    mainError.Text = "Email ID Already Registered";
                                    mainError.IsVisible = true;
                                });
                            }
                            else
                            {
                                Application.Current.Properties["UserName"] = UserName;
                                Application.Current.Properties["FullName"] = Name;
                                Application.Current.Properties["ID"] = "1";
                                Application.Current.Properties["SingleEasyWins"] = "0";
                                Application.Current.Properties["SingleEasyLoses"] = "0";
                                Application.Current.Properties["SingleEasyTies"] = "0";
                                Application.Current.Properties["SingleHardWins"] = "0";
                                Application.Current.Properties["SingleHardLoses"] = "0";
                                Application.Current.Properties["SingleHardTies"] = "0";
                                Application.Current.Properties["DoubleWins"] = "0";
                                Application.Current.Properties["DoubleLoses"] = "0";
                                Application.Current.Properties["DoubleTies"] = "0";
                                Application.Current.Properties["EmailID"] = EmailID;
                                Application.Current.Properties["Password"] = Password;
                                Application.Current.Properties["ConfirmPassword"] = ConfirmPassword;
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
                                    Application.Current.MainPage = new NavigationPage(new HomePage());
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

        private void loginButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}