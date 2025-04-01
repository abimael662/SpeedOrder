using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Ajustes : ContentPage
    {
        private bool isDarkTheme = false;

        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;
        public V_Ajustes()
        {
            InitializeComponent();
            BindingContext = App.ViewModelGlobal;
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var correo = App.ViewModelGlobal.Correo;

            if (!string.IsNullOrEmpty(correo))
            {
                _mesero = (await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo));

                if (_mesero != null)
                {
                    TxtNombre.Text = $"{_mesero.Nombre} {_mesero.Ape_paterno} {_mesero.Ape_materno}";
                }
                else
                {
                    TxtNombre.Text = "Correo no encontrado. Verifica tus credenciales.";
                }
            }
            else
            {
                TxtNombre.Text = "No se ha proporcionado correo.";
            }
        }
        private async void TxtEdit_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_EditarPerfil());
        }

        private async void TxtDelete_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_EliminarCuenta());
        }
        private void OnThemeToggle(object sender, EventArgs e)
        {
            isDarkTheme = !isDarkTheme;

            if (isDarkTheme)
            {
                Application.Current.Resources["PageTheme"] = Application.Current.Resources["DarkTheme"];
                Application.Current.Resources["LabelTheme"] = Application.Current.Resources["DarkLabel"];
                Application.Current.Resources["ButtonTheme"] = Application.Current.Resources["DarkButton"];
                Application.Current.UserAppTheme = OSAppTheme.Dark;
                Preferences.Set("UserTheme", "Dark");  // ✅ Guarda el tema oscuro
            }
            else
            {
                Application.Current.Resources["PageTheme"] = Application.Current.Resources["LightTheme"];
                Application.Current.Resources["LabelTheme"] = Application.Current.Resources["LightLabel"];
                Application.Current.Resources["ButtonTheme"] = Application.Current.Resources["LightButton"];
                Application.Current.UserAppTheme = OSAppTheme.Light;
                Preferences.Set("UserTheme", "Light"); // ✅ Guarda el tema claro
            }
        }
    }
}