using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tester.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tester
{


    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("sound.mp3");
            player.Loop = true;
            player.Volume = 0.1;
            if (Application.Current.Properties.ContainsKey("UserName"))
            {
                if ((Application.Current.Properties["UserName"] as string) == "null")
                {
                    MainPage = new Login();
                }
                else
                {

                    MainPage = new NavigationPage(new HomePage());
                }
            }
            else
            {
                Application.Current.Properties["UserName"] = "null";
                MainPage = new Login();
            }
        }


        protected override void OnStart()
        {
            if (Application.Current.Properties.ContainsKey("ApplicationPrimaryColor"))
            {
                string primaryColor = Application.Current.Properties["ApplicationPrimaryColor"] as string;
                string primaryDarkColor = Application.Current.Properties["ApplicationPrimaryDarkColor"] as string;
                string textColor = Application.Current.Properties["ApplicationTextColor"] as string;
                App.Current.Resources["ApplicationPrimaryColor"] = Color.FromHex(primaryColor);
                App.Current.Resources["ApplicationPrimaryDarkColor"] = Color.FromHex(primaryDarkColor);
                App.Current.Resources["ApplicationTextColor"] = Color.FromHex(textColor);
                DependencyService.Get<IStatusBarColor>().SetStatusBarColor();
            }
            else
            {
                Application.Current.Properties["ApplicationPrimaryColor"] = "009587";
                Application.Current.Properties["ApplicationPrimaryDarkColor"] = "00786A";
                Application.Current.Properties["ApplicationTextColor"] = "FFFFFF";
            }
            Application.Current.Properties["temp1401"] = "False";
            if(Application.Current.Properties.ContainsKey("AudioMuted"))
            {
                if(Application.Current.Properties["AudioMuted"] as string == "False")
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Play();
                }
            }
            else
            {
                Application.Current.Properties["AudioMuted"] = "False";
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Play();
            }
            
        }

        protected override void OnSleep()
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            if (player.IsPlaying)
                player.Pause();
        }

        protected override void OnResume()
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            if (!player.IsPlaying && Application.Current.Properties["AudioMuted"] as string=="False")
                player.Play();
        }
    }
}
