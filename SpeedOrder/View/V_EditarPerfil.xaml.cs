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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_EditarPerfil : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;
        public V_EditarPerfil()
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
                    TxtNombre.Text = _mesero.Nombre;
                    TxtAPaterno.Text = _mesero.Ape_paterno;
                    TxtAMaterno.Text = _mesero.Ape_materno;
                    TxtPassword.Text = _mesero.Password;
                    TxtEmail.Text = _mesero.Email;
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
        private async void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtAPaterno.Text) ||
                string.IsNullOrWhiteSpace(TxtAMaterno.Text) || string.IsNullOrWhiteSpace(TxtPassword.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
                return;
            }

            var mer = new Meseros
            {
                Id_Mesero = _mesero.Id_Mesero,
                Nombre = TxtNombre.Text,
                Ape_materno = TxtAMaterno.Text,
                Ape_paterno = TxtAPaterno.Text,
                Edad = _mesero.Edad,
                Password = TxtPassword.Text,
                Email = TxtEmail.Text
            };
            await _db.UpdateAsync(mer);
            await DisplayAlert("Exito", "Datos actualizados correctamente.", "OK");
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
        private async void BtnPassword_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_CambiarPassword());
        }
    }
}