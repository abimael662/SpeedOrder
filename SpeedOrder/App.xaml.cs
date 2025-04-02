using SpeedOrder.View;
using SpeedOrder.ViewModel;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder
{
    public partial class App : Application
    {
        public static LoginViewModel ViewModelGlobal { get; private set; }
        public App()
        {
            InitializeComponent();
            ViewModelGlobal = new LoginViewModel();
            SetTheme();
            MainPage = new NavigationPage(new View.V_Login());
        }
        private void SetTheme()
        {
            // Obtiene el tema guardado o usa el tema del sistema si no hay ninguno guardado
            string savedTheme = Preferences.Get("UserTheme", "Default");

            if (savedTheme == "Default")
            {
                OSAppTheme systemTheme = Application.Current.RequestedTheme;
                savedTheme = systemTheme == OSAppTheme.Dark ? "Dark" : "Light";
            }

            // Aplica el tema
            if (savedTheme == "Dark")
            {
                Application.Current.Resources["PageTheme"] = Application.Current.Resources["DarkTheme"];
                Application.Current.Resources["LabelTheme"] = Application.Current.Resources["DarkLabel"];
                Application.Current.Resources["ButtonTheme"] = Application.Current.Resources["DarkButton"];
                Application.Current.UserAppTheme = OSAppTheme.Dark;
                Preferences.Set("UserTheme", "Dark");
            }
            else
            {
                Application.Current.Resources["PageTheme"] = Application.Current.Resources["LightTheme"];
                Application.Current.Resources["LabelTheme"] = Application.Current.Resources["LightLabel"];
                Application.Current.Resources["ButtonTheme"] = Application.Current.Resources["LightButton"];
                Application.Current.UserAppTheme = OSAppTheme.Light;
                Preferences.Set("UserTheme", "Light");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
