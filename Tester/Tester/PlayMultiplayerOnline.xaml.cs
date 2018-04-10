using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tester.Models;
using Tester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayMultiplayerOnline : ContentPage
    {
        public static double PageHeight;
        float x, y;
        bool IfFailure = false;
        List<string> CanvasElements = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
        bool IfWorking = false, buttonCondition = true;
        public GameArenaViewModel GameViewModel;
        public GameDataClass OnlineGameData;
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = ((Color)App.Current.Resources["ApplicationPrimaryColor"]).ToSKColor(),
            StrokeWidth = 20,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };

        public PlayMultiplayerOnline()
        {
            PageHeight = Application.Current.MainPage.Height / 2;

            GameViewModel = new GameArenaViewModel
            {
                IsLoading = true
            };
            this.BindingContext = GameViewModel;
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            InitializeGameArena();
            GameViewModel.IsLoading = false;
        }

        private void InitializeGameArena()
        {

            GameViewModel.PlayerName = Application.Current.Properties["UserName"] as string;
            GameViewModel.PlayerWins = long.Parse(Application.Current.Properties["DoubleWins"] as string);
            GameViewModel.PlayerLoses = long.Parse(Application.Current.Properties["DoubleLoses"] as string);
            GameViewModel.PlayerTies = long.Parse(Application.Current.Properties["DoubleTies"] as string);
            GameViewModel.OpponentWins = 0;
            GameViewModel.OpponentLoses = 0;
            GameViewModel.OpponentTies = 0;
            GameViewModel.IsGameOver = false;
            GameViewModel.Filled = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadingIndicatorText.Text = "Loading Game Data";
            quitButton.IsVisible = false;
            overlay.IsVisible = true;
            Easing animationType = Easing.Linear;
            Random randomNumber = new Random();
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                if (GameViewModel.IsPlayerTurn == false)
                {
                    switch (randomNumber.Next(3))
                    {
                        case 0:
                            animationType = Easing.SpringOut;
                            break;
                        case 1:
                            animationType = Easing.Linear;
                            break;
                        case 2:
                            animationType = Easing.CubicOut;
                            break;
                        case 3:
                            animationType = Easing.SinInOut;
                            break;
                        default:
                            animationType = Easing.BounceOut;
                            break;
                    }
                    {
                        ScoreGrid.AnchorX = 0;
                        ScoreGrid.RotationY = 0;
                        ScoreGrid.RotateYTo(360, 1000, animationType);
                        ScoreGrid.AnchorX = 0.5;
                    }
                    return true;
                }
                else
                {
                    

                }
                return true;
            });
            Task.Run(() =>
            {
                ReturnMyData();
            });
        }

        private async void quitButton_Clicked(object sender, EventArgs e)
        {
            if (quitButton.Text != "Play")
            {
                await Navigation.PopAsync(true);
                return;
            }
            overlay.IsVisible = false;
            if (OnlineGameData.Turn == "Mine")
            {
                GameViewModel.IsPlayerTurn = true;
            }
            else
            {
                GameViewModel.IsPlayerTurn = false;
                IfWorking = true;
                await ComputerTurnAsync();
                IfWorking = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Exit Game?", "Are you sure you want to quit this game?", "Yes", "No");
                if (result)
                {
                    await Task.Run(async () =>
                    {
                        await IQuit();
                    });
                    await this.Navigation.PopAsync();
                }

            });

            return true;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (IfWorking == false && GameViewModel.IsGameOver == false && GameViewModel.IsPlayerTurn == true)
            {
                IfWorking = true;
                var ArenaCell = (SKCanvasView)sender;
                IfFailure = false;
                await PrintArenaAsync(ArenaCell);
                if (IfFailure == true)
                    return;
                IfWorking = false;
                if (GameViewModel.IsGameOver == false)
                {
                    IfWorking = true;
                    await ComputerTurnAsync();
                    IfWorking = false;
                }
            }
        }

        private async Task ComputerTurnAsync()
        {
            SKCanvasView tempCanvasView;
            while (OnlineGameData.Turn != "Mine" && OnlineGameData.IfQuit!="True")
            {
                await CheckStatus();
            }
            if (OnlineGameData.IfQuit == "True")
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Snap!", "Opponent Quitted", "Okay", " ");
                    if (result)
                        await this.Navigation.PopAsync();
                });
            }
            int abc = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameViewModel.ArenaMatrix[i, j] == ' ' && OnlineGameData.IfClicked[abc] != 0)
                    {
                        foreach (var item in CanvasElements)
                        {
                            tempCanvasView = this.FindByName<SKCanvasView>(item);
                            if (tempCanvasView.StyleId == i.ToString() + j.ToString())
                            {
                                await PrintArenaAsync(tempCanvasView);
                                return;
                            }
                        }
                    }
                    abc++;
                }
            }
        }

        private async Task PrintArenaAsync(SKCanvasView ArenaCell)
        {
            int[] index = new int[2] { int.Parse(ArenaCell.StyleId[0].ToString()), int.Parse(ArenaCell.StyleId[1].ToString()) };
            if (GameViewModel.ArenaMatrix[index[0], index[1]] != ' ')
            {
                IfFailure = true;
                IfWorking = false;
                return;
            }
            ArenaCell.AnchorX = 0;
            ArenaCell.RotationY = 0;
            ArenaCell.RotateYTo(360, 500, Easing.Linear);
            ArenaCell.AnchorX = 0.5;
            TurnButtonLayout.TranslationY = 0;
            GameViewModel.UpdateGameArena(index);
            ArenaCell.InvalidateSurface();
            if(GameViewModel.IsPlayerTurn==false)
            {
                int abc = 0;
                for(int i=0;i<3;i++)
                {
                    for(int j=0;j<3;j++)
                    {
                        if(index[0]==i && index[1]==j)
                        {
                            OnlineGameData.IfClicked[abc] = 1;
                            OnlineGameData.SymbolPresent[abc] = GameViewModel.PlayerSymbol.ToString();
                            OnlineGameData.Turn = "Opponent";
                        }
                        abc++;
                    }
                }
                await Task.Run(async () =>
                {
                    await UpdateStatus();
                });
            }
            if (GameViewModel.IsGameOver)
            {
                if (GameViewModel.GameResult == 'P')
                {
                    GameViewModel.PlayerWins++;
                    GameViewModel.OpponentLoses++;
                    Application.Current.Properties["DoubleWinsCount"] = ((long.Parse(Application.Current.Properties["DoubleWinsCount"] as string)) + 1).ToString();
                    Application.Current.Properties["DoubleWins"] = ((long.Parse(Application.Current.Properties["DoubleWins"] as string)) + 1).ToString();
                    DisplayAlert("Game Over", GameViewModel.PlayerName + " won!!", "Okay");
                }
                else if (GameViewModel.GameResult == 'O')
                {
                    GameViewModel.OpponentWins++;
                    GameViewModel.PlayerLoses++;
                    Application.Current.Properties["DoubleLosesCount"] = ((long.Parse(Application.Current.Properties["DoubleLosesCount"] as string)) + 1).ToString();
                    Application.Current.Properties["DoubleLoses"] = ((long.Parse(Application.Current.Properties["DoubleLoses"] as string)) + 1).ToString();
                    DisplayAlert("Game Over", GameViewModel.OpponentName + " won!!", "Okay");
                }
                else
                {
                    GameViewModel.PlayerTies++;
                    GameViewModel.OpponentTies++;
                    Application.Current.Properties["DoubleTiesCount"] = ((long.Parse(Application.Current.Properties["DoubleTiesCount"] as string)) + 1).ToString();
                    Application.Current.Properties["DoubleTies"] = ((long.Parse(Application.Current.Properties["DoubleTies"] as string)) + 1).ToString();
                    DisplayAlert("Game Over", " It's a tie!!", "Okay");
                }
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
                                Application.Current.Properties["DoubleWinsCount"] = "0";
                                Application.Current.Properties["DoubleLosesCount"] = "0";
                                Application.Current.Properties["DoubleTiesCount"] = "0";
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
            }
        }

        private void OnCanvasViewPaintSurfaceAsync(object sender, SKPaintSurfaceEventArgs e)
        {

            var ArenaCell = (SKCanvasView)sender;
            int[] index = new int[2] { int.Parse(ArenaCell.StyleId[0].ToString()), int.Parse(ArenaCell.StyleId[1].ToString()) };

            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            if (ArenaCell.StyleId == "98")
            {
                if (GameViewModel.PlayerSymbol == 'X')
                {
                    x = info.Width;
                    y = info.Height;
                    canvas.DrawLine(40, 40, x - 40, y - 40, paint);
                    canvas.DrawLine(40, y - 40, x - 40, 40, paint);
                }
                else
                {
                    x = info.Width;
                    canvas.DrawCircle(info.Width / 2, info.Height / 2, (x / 2) - 30, paint);
                }
                return;
            }
            else if (ArenaCell.StyleId == "99")
            {
                if (GameViewModel.OpponentSymbol == 'X')
                {
                    x = info.Width;
                    y = info.Height;
                    canvas.DrawLine(40, 40, x - 40, y - 40, paint);
                    canvas.DrawLine(40, y - 40, x - 40, 40, paint);
                }
                else
                {
                    x = info.Width;
                    canvas.DrawCircle(info.Width / 2, info.Height / 2, (x / 2) - 30, paint);
                }
                return;
            }
            else if (GameViewModel.ArenaMatrix[index[0], index[1]] == ' ')
            {
                return;
            }
            else if (GameViewModel.ArenaMatrix[index[0], index[1]] == 'X')
            {
                x = info.Width;
                y = info.Height;
                canvas.DrawLine(35, 35, x - 35, y - 35, paint);
                canvas.DrawLine(35, y - 35, x - 35, 35, paint);
            }
            else
            {
                x = info.Width;
                int i = 35;
                canvas.DrawCircle(info.Width / 2, info.Height / 2, (x / 2) - i, paint);

            }


        }

        private async void Reset_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }

        private async void ReturnMyData()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/ReturnMyData";
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
                        if (myContent == "False")
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                LoadingIndicator.IsVisible = false;
                                LoadingIndicatorText.Text = "Can't Connect To Server";
                                quitButton.Text = "Go Back";
                                quitButton.IsVisible = true;
                            });
                        }
                        else
                        {
                            OnlineGameData = JsonConvert.DeserializeObject<GameDataClass>(myContent);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                GameViewModel.OpponentName = OnlineGameData.OpponentName;
                                GameViewModel.PlayerSymbol = OnlineGameData.MySymbol[0];
                                if(GameViewModel.PlayerSymbol=='X')
                                {
                                    GameViewModel.OpponentSymbol = 'O';
                                }
                                else
                                {
                                    GameViewModel.OpponentSymbol = 'X';
                                }
                                j.InvalidateSurface();
                                k.InvalidateSurface();
                                OpponentNameLabel.Text = GameViewModel.OpponentName + "'s Turn";
                                LoadingIndicator.IsVisible = false;
                                LoadingIndicatorText.Text = "";
                                quitButton.Text = "Play";
                                quitButton.IsVisible = true;
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LoadingIndicatorText.Text = "Can't Connect To Server";
                            quitButton.Text = "Go Back";
                            quitButton.IsVisible = true;
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LoadingIndicatorText.Text = "Can't Connect To Server";
                        quitButton.Text = "Go Back";
                        quitButton.IsVisible = true;
                    });
                    return;
                }
            }
        }

        private async Task DeleteQuitted()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/DeleteQuitted";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("id", Application.Current.Properties["ID"] as string) });
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
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                            if (result)
                                await this.Navigation.PopAsync();
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                        if (result)
                            await this.Navigation.PopAsync();
                    });
                    return;
                }
            }
        }

        private async Task UpdateStatus()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/UpdateStatus";
            var updatedJson = JsonConvert.SerializeObject(OnlineGameData);
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("updatedJson", updatedJson) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent != "Okay")
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                                if (result)
                                    await this.Navigation.PopAsync();
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                            if (result)
                                await this.Navigation.PopAsync();
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                        if (result)
                            await this.Navigation.PopAsync();
                    });
                    return;
                }
            }
        }

        private async Task CheckStatus()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/CheckStatus";
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
                        OnlineGameData = JsonConvert.DeserializeObject<GameDataClass>(myContent);
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                            if (result)
                                await this.Navigation.PopAsync();
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                        if (result)
                            await this.Navigation.PopAsync();
                    });
                    return;
                }
            }
        }

        private async Task IQuit()
        {
            string url = "http://tictactoe.sudeshkumar.me/Android/IQuit";
            HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("id", Application.Current.Properties["ID"] as string) });
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                    HttpResponseMessage response = await getResponse;
                    if (response.IsSuccessStatusCode)
                    {
                        var myContent = await response.Content.ReadAsStringAsync();
                        if (myContent != "Deleted")
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                                if (result)
                                    await this.Navigation.PopAsync();
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                            if (result)
                                await this.Navigation.PopAsync();
                        });
                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Errr!!", "Connection With Server Lost", "Okay", " ");
                        if (result)
                            await this.Navigation.PopAsync();
                    });
                    return;
                }
            }
        }

        private async Task UpdateStatsMob()
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
                        Application.Current.Properties["DoubleWinsCount"] = "0";
                        Application.Current.Properties["DoubleLosesCount"] = "0";
                        Application.Current.Properties["DoubleTiesCount"] = "0";
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}