using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var temp = (Image)sender;
            if(temp.StyleId=="0")
            {
                Device.OpenUri(new Uri("https://www.sudeshkumar.me"));
            }
            else if(temp.StyleId=="1")
            {
                Device.OpenUri(new Uri("https://www.facebook.com/sudesh1611"));
            }
            else
            {
                Device.OpenUri(new Uri("https://github.com/sudesh1611"));
            }
        }
    }
}