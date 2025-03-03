using SpeedOrder.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
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
