using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin;
using Xamarin.Forms;
using Tester;
using Tester.Droid;
using Xamarin.Forms.Platform.Android;

[assembly:Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace Tester.Droid
{
    [Activity(Label = "Tester", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation =ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,IStatusBarColor
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();
            LoadApplication(new App());
            if(Build.VERSION.SdkInt>=BuildVersionCodes.Lollipop)
            {
                var statusbarColor = (Color)App.Current.Resources["ApplicationPrimaryDarkColor"];
                this.Window.SetStatusBarColor(statusbarColor.ToAndroid());
            }
        }

        public void SetStatusBarColor()
        {
            {
                if(Build.VERSION.SdkInt>=BuildVersionCodes.Lollipop)
                {
                    var statusbarColor = (Color)App.Current.Resources["ApplicationPrimaryDarkColor"];
                    (Forms.Context as Activity).Window.SetStatusBarColor(statusbarColor.ToAndroid());
                }
            }
        }
    }

}

