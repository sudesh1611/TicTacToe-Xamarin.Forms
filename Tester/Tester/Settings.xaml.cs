using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public class SettingName
        {
            public string SettingIcons { get; set; }
            public string Setting { get; set; }
        }
        ObservableCollection<SettingName> settings = new ObservableCollection<SettingName>();
        public Settings()
        {
            InitializeComponent();
            this.SettingsItem.ItemsSource = settings;
            settings.Add(new SettingName { Setting = "Account", SettingIcons = "ic_account_circle_black_36dp.png" });
            settings.Add(new SettingName { Setting = "Theme", SettingIcons = "ic_invert_colors_black_36dp.png" });
            settings.Add(new SettingName { Setting = "About", SettingIcons = "ic_help_black_36dp.png" });

            SettingsItem.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }

        private void ListViewItemSelected(object sender, EventArgs e)
        {
            var parent = (StackLayout)sender;
            var label = (Label)parent.Children[1];
            if(label.Text=="Account")
            {
                Navigation.PushAsync(new Account());
            }
            else if(label.Text=="Theme")
            {
                Navigation.PushAsync(new MainPage());
            }
            else if(label.Text=="About")
            {
                Navigation.PushAsync(new About());
            }
        }
    }
}