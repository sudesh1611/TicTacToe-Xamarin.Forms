using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace Tester.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", NoHistory = true, MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashScreen : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentBundle)
        {
            base.OnCreate(savedInstanceState, persistentBundle);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(this, typeof(MainActivity)));
        }

        public override void OnBackPressed()
        {

        }
    }
        
}