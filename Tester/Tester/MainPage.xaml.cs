using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tester
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var image = (ImageCircle.Forms.Plugin.Abstractions.CircleImage)sender;
            Color color = image.FillColor;
            App.Current.Resources["ApplicationPrimaryColor"] = color;
            string finalColor = "E54919";
            if (color == Color.FromHex("FE5622"))
            {
                finalColor = "E54919";
                Application.Current.Properties["ApplicationPrimaryColor"] = "FE5622";
            }
            else if (color == Color.FromHex("2195F2"))
            {
                finalColor = "1975D1";
                Application.Current.Properties["ApplicationPrimaryColor"] = "2195F2";
            }
            else if (color == Color.FromHex("009587"))
            {
                finalColor = "00786A";
                Application.Current.Properties["ApplicationPrimaryColor"] = "009587";
            }
            else if (color == Color.FromHex("F34236"))
            {
                finalColor = "D22F2F";
                Application.Current.Properties["ApplicationPrimaryColor"] = "F34236";
            }
            else if (color == Color.FromHex("E81E62"))
            {
                finalColor = "C1185A";
                Application.Current.Properties["ApplicationPrimaryColor"] = "E81E62";
            }
            else if (color == Color.FromHex("CCDB39"))
            {
                finalColor = "AEB32B";
                Application.Current.Properties["ApplicationPrimaryColor"] = "CCDB39";
            }
            else if (color == Color.FromHex("9B27AF"))
            {
                finalColor = "7A1FA1";
                Application.Current.Properties["ApplicationPrimaryColor"] = "9B27AF";
            }
            else if (color == Color.FromHex("663AB6"))
            {
                finalColor = "502DA7";
                Application.Current.Properties["ApplicationPrimaryColor"] = "663AB6";
            }
            else if (color == Color.FromHex("3E50B4"))
            {
                finalColor = "303F9E";
                Application.Current.Properties["ApplicationPrimaryColor"] = "3E50B4";
            }
            else if (color == Color.FromHex("4BAE4F"))
            {
                finalColor = "378D3C";
                Application.Current.Properties["ApplicationPrimaryColor"] = "4BAE4F";
            }
            else if (color == Color.FromHex("5F7C8A"))
            {
                finalColor = "445963";
                Application.Current.Properties["ApplicationPrimaryColor"] = "5F7C8A";
            }
            else if (color == Color.FromHex("111111"))
            {
                finalColor = "000000";
                Application.Current.Properties["ApplicationPrimaryColor"] = "000000";
            }

            App.Current.Resources["ApplicationPrimaryDarkColor"] = Color.FromHex(finalColor);
            Application.Current.Properties["ApplicationPrimaryDarkColor"] = finalColor;
            Application.Current.Properties["ApplicationTextColor"] = "FFFFFF";
            Application.Current.SavePropertiesAsync();
            DependencyService.Get<IStatusBarColor>().SetStatusBarColor();
        }
    }
}
