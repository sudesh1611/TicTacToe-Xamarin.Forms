using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaySingleEasy : ContentPage
    {
        public static double PageHeight;
        float x, y;
        bool IfFailure = false;
        List<string> CanvasElements = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
        bool IfWorking = false, buttonCondition = true;
        public GameArenaViewModel GameViewModel;
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = ((Color)App.Current.Resources["ApplicationPrimaryColor"]).ToSKColor(),
            StrokeWidth = 20,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };
        public PlaySingleEasy()
        {

            PageHeight = Application.Current.MainPage.Height / 2;

            GameViewModel = new GameArenaViewModel
            {
                IsLoading = true
            };
            this.BindingContext = GameViewModel;
            InitializeComponent();
            InitializeGameArena();
            GameViewModel.IsLoading = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                    //if (counter >= 3)
                    //{
                    //    MainGrid.AnchorX = 0;
                    //    MainGrid.RotationY = 0;
                    //    MainGrid.RotateYTo(360, 1000, animationType);
                    //    MainGrid.AnchorX = 0.5;
                    //    counter = 0;
                    //    return true;
                    //}
                    //if (counter >= 100)
                    //{
                    //    j.AnchorX = 0;
                    //    j.RotationY = 0;
                    //    j.RotateYTo(-360, 1000, animationType);
                    //    j.AnchorX = 0.5;
                    //}
                    //if (counter >= 1)
                    //{
                    //    k.AnchorX = 0;
                    //    k.RotationY = 0;
                    //    k.RotateYTo(360, 1000, animationType);
                    //    k.AnchorX = 0.5;
                    //}
                    //if (counter >= 2)
                    {
                        ScoreGrid.AnchorX = 0;
                        ScoreGrid.RotationY = 0;
                        ScoreGrid.RotateYTo(360, 1000, animationType);
                        ScoreGrid.AnchorX = 0.5;
                    }
                    //counter++;
                    return true;
                }
                else
                {
                    //if (buttonCondition == true)
                    //{
                    //    TurnButtonLayout.TranslationY = 0;
                    //    TurnButtonLayout.TranslateTo(0, -20, 2000, Easing.BounceOut);
                    //    buttonCondition = false;
                    //}
                    //else
                    //{
                    //    TurnButtonLayout.TranslationY = -20;
                    //    TurnButtonLayout.TranslateTo(0, 0, 2000, Easing.BounceOut);
                    //    buttonCondition = true;
                    //}

                }
                return true;
            });
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Exit Game?", "Are you sure you want to quit this game?", "Yes", "No");
                if (result)
                    await this.Navigation.PopAsync();
            });

            return true;
        }

        private void InitializeGameArena()
        {

            GameViewModel.PlayerName = Application.Current.Properties["UserName"] as string;
            GameViewModel.OpponentName = "Alice";
            GameViewModel.PlayerSymbol = 'X';
            GameViewModel.OpponentSymbol = 'O';
            GameViewModel.PlayerWins = long.Parse(Application.Current.Properties["SingleEasyWins"] as string);
            GameViewModel.PlayerLoses = long.Parse(Application.Current.Properties["SingleEasyLoses"] as string);
            GameViewModel.PlayerTies = long.Parse(Application.Current.Properties["SingleEasyTies"] as string);
            GameViewModel.OpponentWins = long.Parse(Application.Current.Properties["SingleEasyLoses"] as string);
            GameViewModel.OpponentLoses = long.Parse(Application.Current.Properties["SingleEasyWins"] as string);
            GameViewModel.OpponentTies = long.Parse(Application.Current.Properties["SingleEasyTies"] as string);
            GameViewModel.IsGameOver = false;
            GameViewModel.IsPlayerTurn = true;
            GameViewModel.Filled = 0;
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
            await Task.Delay(3000);
            string abc = "";
            SKCanvasView tempCanvasView;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameViewModel.ArenaMatrix[i, j] == ' ')
                    {
                        GameViewModel.ArenaMatrix[i, j] = 'O';
                        var x = GameOver();
                        if (x == 10)
                        {
                            GameViewModel.ArenaMatrix[i, j] = ' ';
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
                        GameViewModel.ArenaMatrix[i, j] = ' ';
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameViewModel.ArenaMatrix[i, j] == ' ')
                    {
                        GameViewModel.ArenaMatrix[i, j] = 'X';
                        var x = GameOver();
                        if (x == -10)
                        {
                            GameViewModel.ArenaMatrix[i, j] = ' ';
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
                        GameViewModel.ArenaMatrix[i, j] = ' ';
                    }
                }
            }

            {
                if (GameViewModel.ArenaMatrix[0, 0] == 'O' && GameViewModel.ArenaMatrix[0, 1] == ' ' && GameViewModel.ArenaMatrix[0, 2] == ' ')
                {
                    abc = "01";
                }
                else if (GameViewModel.ArenaMatrix[0, 0] == 'O' && GameViewModel.ArenaMatrix[1, 0] == ' ' && GameViewModel.ArenaMatrix[2, 0] == ' ')
                {
                    abc = "10";
                }
                else if (GameViewModel.ArenaMatrix[0, 0] == 'O' && GameViewModel.ArenaMatrix[1, 1] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "11";
                }
                else if (GameViewModel.ArenaMatrix[0, 1] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[0, 2] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[0, 1] == 'O' && GameViewModel.ArenaMatrix[1, 1] == ' ' && GameViewModel.ArenaMatrix[2, 1] == ' ')
                {
                    abc = "11";
                }
                else if (GameViewModel.ArenaMatrix[0, 2] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[0, 1] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[0, 2] == 'O' && GameViewModel.ArenaMatrix[1, 1] == ' ' && GameViewModel.ArenaMatrix[2, 0] == ' ')
                {
                    abc = "11";
                }
                else if (GameViewModel.ArenaMatrix[0, 2] == 'O' && GameViewModel.ArenaMatrix[1, 2] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "12";
                }
                else if (GameViewModel.ArenaMatrix[1, 0] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[2, 0] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[1, 0] == 'O' && GameViewModel.ArenaMatrix[1, 1] == ' ' && GameViewModel.ArenaMatrix[1, 2] == ' ')
                {
                    abc = "11";
                }
                else if (GameViewModel.ArenaMatrix[1, 1] == 'O' && GameViewModel.ArenaMatrix[1, 0] == ' ' && GameViewModel.ArenaMatrix[1, 2] == ' ')
                {
                    abc = "10";
                }
                else if (GameViewModel.ArenaMatrix[1, 1] == 'O' && GameViewModel.ArenaMatrix[0, 1] == ' ' && GameViewModel.ArenaMatrix[2, 1] == ' ')
                {
                    abc = "01";
                }
                else if (GameViewModel.ArenaMatrix[1, 1] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[1, 1] == 'O' && GameViewModel.ArenaMatrix[0, 2] == ' ' && GameViewModel.ArenaMatrix[2, 0] == ' ')
                {
                    abc = "02";
                }
                else if (GameViewModel.ArenaMatrix[1, 2] == 'O' && GameViewModel.ArenaMatrix[1, 0] == ' ' && GameViewModel.ArenaMatrix[1, 1] == ' ')
                {
                    abc = "10";
                }
                else if (GameViewModel.ArenaMatrix[1, 2] == 'O' && GameViewModel.ArenaMatrix[0, 2] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "02";
                }
                else if (GameViewModel.ArenaMatrix[2, 0] == 'O' && GameViewModel.ArenaMatrix[2, 1] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "21";
                }
                else if (GameViewModel.ArenaMatrix[2, 0] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[1, 0] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[2, 0] == 'O' && GameViewModel.ArenaMatrix[1, 1] == ' ' && GameViewModel.ArenaMatrix[0, 2] == ' ')
                {
                    abc = "02";
                }
                else if (GameViewModel.ArenaMatrix[2, 1] == 'O' && GameViewModel.ArenaMatrix[0, 1] == ' ' && GameViewModel.ArenaMatrix[1, 1] == ' ')
                {
                    abc = "01";
                }
                else if (GameViewModel.ArenaMatrix[2, 1] == 'O' && GameViewModel.ArenaMatrix[2, 0] == ' ' && GameViewModel.ArenaMatrix[2, 2] == ' ')
                {
                    abc = "20";
                }
                else if (GameViewModel.ArenaMatrix[2, 2] == 'O' && GameViewModel.ArenaMatrix[2, 0] == ' ' && GameViewModel.ArenaMatrix[2, 1] == ' ')
                {
                    abc = "20";
                }
                else if (GameViewModel.ArenaMatrix[2, 2] == 'O' && GameViewModel.ArenaMatrix[0, 0] == ' ' && GameViewModel.ArenaMatrix[1, 1] == ' ')
                {
                    abc = "00";
                }
                else if (GameViewModel.ArenaMatrix[2, 2] == 'O' && GameViewModel.ArenaMatrix[0, 2] == ' ' && GameViewModel.ArenaMatrix[1, 2] == ' ')
                {
                    abc = "02";
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bool shouldBreak = false;
                        for (int j = 0; j < 3; j++)
                        {
                            if (GameViewModel.ArenaMatrix[i, j] == ' ')
                            {
                                abc = i.ToString() + j.ToString();
                                shouldBreak = true;
                                break;
                            }
                        }
                        if (shouldBreak)
                        {
                            break;
                        }
                    }
                }
            }
            foreach (var item in CanvasElements)
            {
                tempCanvasView = this.FindByName<SKCanvasView>(item);
                if (tempCanvasView.StyleId == abc)
                {
                    await PrintArenaAsync(tempCanvasView);
                    return;
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
            if (GameViewModel.IsGameOver)
            {
                if (GameViewModel.GameResult == 'P')
                {
                    GameViewModel.PlayerWins++;
                    GameViewModel.OpponentLoses++;
                    Application.Current.Properties["SingleEasyWinsCount"] = ((long.Parse(Application.Current.Properties["SingleEasyWinsCount"] as string)) + 1).ToString();
                    Application.Current.Properties["SingleEasyWins"] = ((long.Parse(Application.Current.Properties["SingleEasyWins"] as string)) + 1).ToString();
                    DisplayAlert("Game Over", GameViewModel.PlayerName + " won!!", "Okay");
                }
                else if (GameViewModel.GameResult == 'O')
                {
                    GameViewModel.OpponentWins++;
                    GameViewModel.PlayerLoses++;
                    Application.Current.Properties["SingleEasyLosesCount"] = ((long.Parse(Application.Current.Properties["SingleEasyLosesCount"] as string)) + 1).ToString();
                    Application.Current.Properties["SingleEasyLoses"] = ((long.Parse(Application.Current.Properties["SingleEasyLoses"] as string)) + 1).ToString();
                    DisplayAlert("Game Over", GameViewModel.OpponentName + " won!!", "Okay");
                }
                else
                {
                    GameViewModel.PlayerTies++;
                    GameViewModel.OpponentTies++;
                    Application.Current.Properties["SingleEasyTiesCount"] = ((long.Parse(Application.Current.Properties["SingleEasyTiesCount"] as string)) + 1).ToString();
                    Application.Current.Properties["SingleEasyTies"] = ((long.Parse(Application.Current.Properties["SingleEasyTies"] as string)) + 1).ToString();
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
                                Application.Current.Properties["SingleEasyWinsCount"] = "0";
                                Application.Current.Properties["SingleEasyLosesCount"] = "0";
                                Application.Current.Properties["SingleEasyTiesCount"] = "0";
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            GameViewModel = new GameArenaViewModel
            {
                IsLoading = true
            };
            InitializeGameArena();
            foreach (var item in CanvasElements)
            {
                this.FindByName<SKCanvasView>(item).InvalidateSurface();
            }
            GameViewModel.IsLoading = false;
            this.BindingContext = GameViewModel;
        }

        private int GameOver()
        {
            if (GameViewModel.ArenaMatrix[0, 0] == GameViewModel.ArenaMatrix[0, 1] && GameViewModel.ArenaMatrix[0, 1] == GameViewModel.ArenaMatrix[0, 2] && GameViewModel.ArenaMatrix[0, 1] != ' ')
                return GameViewModel.ArenaMatrix[0, 1] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[1, 0] == GameViewModel.ArenaMatrix[1, 1] && GameViewModel.ArenaMatrix[1, 1] == GameViewModel.ArenaMatrix[1, 2] && GameViewModel.ArenaMatrix[1, 0] != ' ')
                return GameViewModel.ArenaMatrix[1, 0] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[2, 0] == GameViewModel.ArenaMatrix[2, 1] && GameViewModel.ArenaMatrix[2, 1] == GameViewModel.ArenaMatrix[2, 2] && GameViewModel.ArenaMatrix[2, 1] != ' ')
                return GameViewModel.ArenaMatrix[2, 1] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[0, 0] == GameViewModel.ArenaMatrix[1, 0] && GameViewModel.ArenaMatrix[1, 0] == GameViewModel.ArenaMatrix[2, 0] && GameViewModel.ArenaMatrix[0, 0] != ' ')
                return GameViewModel.ArenaMatrix[0, 0] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[0, 1] == GameViewModel.ArenaMatrix[1, 1] && GameViewModel.ArenaMatrix[1, 1] == GameViewModel.ArenaMatrix[2, 1] && GameViewModel.ArenaMatrix[0, 1] != ' ')
                return GameViewModel.ArenaMatrix[0, 1] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[0, 2] == GameViewModel.ArenaMatrix[1, 2] && GameViewModel.ArenaMatrix[1, 2] == GameViewModel.ArenaMatrix[2, 2] && GameViewModel.ArenaMatrix[0, 2] != ' ')
                return GameViewModel.ArenaMatrix[0, 2] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[0, 0] == GameViewModel.ArenaMatrix[1, 1] && GameViewModel.ArenaMatrix[1, 1] == GameViewModel.ArenaMatrix[2, 2] && GameViewModel.ArenaMatrix[0, 0] != ' ')
                return GameViewModel.ArenaMatrix[0, 0] == 'O' ? 10 : -10;

            if (GameViewModel.ArenaMatrix[0, 2] == GameViewModel.ArenaMatrix[1, 1] && GameViewModel.ArenaMatrix[1, 1] == GameViewModel.ArenaMatrix[2, 0] && GameViewModel.ArenaMatrix[0, 2] != ' ')
                return GameViewModel.ArenaMatrix[0, 2] == 'O' ? 10 : -10;

            return -1;
        }



    }
}