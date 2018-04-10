using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tester.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayMultiplayerOffline : ContentPage
    {
        public static double PageHeight;
        float x, y;

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

        public PlayMultiplayerOffline()
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

        //protected override bool OnBackButtonPressed()
        //{
        //    //Task BackPressedTask = new Task(async () =>
        //    //{
        //    //    bool x=await 
        //    //    if (x)
        //    //        base.OnBackButtonPressed();
        //    //});
        //    return false;
        //}

        private void InitializeGameArena()
        {

            GameViewModel.PlayerName = Application.Current.Properties["TempPlayerOne"] as string;
            GameViewModel.OpponentName = Application.Current.Properties["TempPlayerTwo"] as string;
            GameViewModel.PlayerSymbol = 'X';
            GameViewModel.OpponentSymbol = 'O';
            GameViewModel.PlayerWins = long.Parse(Application.Current.Properties["temp1"] as string);
            GameViewModel.PlayerLoses = long.Parse(Application.Current.Properties["temp2"] as string);
            GameViewModel.PlayerTies = long.Parse(Application.Current.Properties["temp3"] as string);
            GameViewModel.OpponentWins = GameViewModel.PlayerLoses;
            GameViewModel.OpponentLoses = GameViewModel.PlayerWins;
            GameViewModel.OpponentTies = GameViewModel.PlayerTies;
            GameViewModel.IsGameOver = false;
            GameViewModel.Filled = 0;
            PlayerButton.Text = GameViewModel.PlayerName + "'s Turn";
            OpponentButton.Text = GameViewModel.OpponentName +"'s Turn";
            if (Application.Current.Properties["TempPlayerTurn"] as string == "First")
            {
                GameViewModel.IsPlayerTurn = true;
            }
            else
            {
                GameViewModel.IsPlayerTurn = false;
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (IfWorking == false && GameViewModel.IsGameOver == false)
            {
                IfWorking = true;
                var ArenaCell = (SKCanvasView)sender;
                int[] index = new int[2] { int.Parse(ArenaCell.StyleId[0].ToString()), int.Parse(ArenaCell.StyleId[1].ToString()) };
                if (GameViewModel.ArenaMatrix[index[0], index[1]] != ' ')
                {
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
                        Application.Current.Properties["temp1"] = GameViewModel.PlayerWins.ToString();
                        DisplayAlert("Game Over", GameViewModel.PlayerName + " won!!", "Okay");
                    }
                    else if (GameViewModel.GameResult == 'O')
                    {
                        GameViewModel.PlayerLoses++;
                        GameViewModel.OpponentWins++;
                        Application.Current.Properties["temp2"] = GameViewModel.PlayerLoses.ToString();
                        DisplayAlert("Game Over", GameViewModel.OpponentName + " won!!", "Okay");
                    }
                    else
                    {
                        GameViewModel.PlayerTies++;
                        GameViewModel.OpponentTies++;
                        Application.Current.Properties["temp3"] = GameViewModel.PlayerTies.ToString();
                        DisplayAlert("Game Over", " It's a tie!!", "Okay");
                    }
                }

                IfWorking = false;
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
    }
}