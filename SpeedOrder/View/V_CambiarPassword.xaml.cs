using Rg.Plugins.Popup.Pages;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_CambiarPassword : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;
        public V_CambiarPassword()
        {
            InitializeComponent();
            BindingContext = App.ViewModelGlobal;
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            var correo = App.ViewModelGlobal.Correo;
            _mesero = (await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo));

            if (string.IsNullOrWhiteSpace(TxtConfirmar.Text) || string.IsNullOrWhiteSpace(TxtNueva.Text))
            {
                await DisplayAlert("Error", "Por favor ingresa una contraseña válida.", "OK");
                return;
            }
            if (_mesero.Password == TxtConfirmar.Text)
            {
                var mesero = new Meseros
                {
                    Id_Mesero = _mesero.Id_Mesero,
                    Nombre = _mesero.Nombre,
                    Ape_paterno = _mesero.Ape_paterno,
                    Ape_materno = _mesero.Ape_materno,
                    Edad = _mesero.Edad,
                    Email = _mesero.Email,
                    Password = TxtNueva.Text
                };
                await _db.UpdateAsync(mesero);
                await DisplayAlert("Éxito", "Contraseña actualizada correctamente.", "OK");
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "La contraseña actual es incorrecta.", "OK");
            }
        }

        private void VerPassword_Clicked(object sender, EventArgs e)
        {
            TxtConfirmar.IsPassword = !TxtConfirmar.IsPassword;
            if (TxtConfirmar.IsPassword)
                VerPassword.Source = "esconder";
            else
                VerPassword.Source = "abierto";
        }

        private void VerNueva_Clicked(object sender, EventArgs e)
        {
            TxtNueva.IsPassword = !TxtNueva.IsPassword;
            if (TxtNueva.IsPassword)
                VerNueva.Source = "esconder";
            else
                VerNueva.Source = "abierto";
        }
    }
}