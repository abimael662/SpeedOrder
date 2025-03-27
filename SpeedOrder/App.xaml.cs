using SpeedOrder.View;
using SpeedOrder.ViewModel;
using System;
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

            //MainPage = new MainPage();
            ViewModelGlobal = new LoginViewModel();
            MainPage = new NavigationPage(new View.V_Login());
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
