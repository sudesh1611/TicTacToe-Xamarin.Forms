using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tester.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : TabbedPage
	{
		public HomePage ()
		{
			InitializeComponent ();
            var temp = ToolbarItems.FirstOrDefault(m => m.Name == "Audio");
            if (Application.Current.Properties["AudioMuted"] as string == "True")
            {
                temp.Icon = "ic_volume_off_white_36dp.png";
            }
            else
            {
                temp.Icon = "ic_volume_up_white_36dp.png";
            }
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings());
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private void Audio_Clicked(object sender, EventArgs e)
        {
            var temp=ToolbarItems.FirstOrDefault(m => m.Name == "Audio");
            if(Application.Current.Properties["AudioMuted"] as string =="True")
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Play();
                temp.Icon = "ic_volume_up_white_36dp.png";
                Application.Current.Properties["AudioMuted"] = "False";
            }
            else
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Pause();
                temp.Icon = "ic_volume_off_white_36dp.png";
                Application.Current.Properties["AudioMuted"] = "True";
            }
        }
    }
}