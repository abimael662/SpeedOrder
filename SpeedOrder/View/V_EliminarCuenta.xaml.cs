using Rg.Plugins.Popup.Pages;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_EliminarCuenta : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;

        public V_EliminarCuenta()
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
                _mesero = await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo);

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

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            var correo = App.ViewModelGlobal.Correo;
            _mesero = await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo);

            if (_mesero == null)
            {
                await DisplayAlert("Error", "No se encontró la cuenta.", "OK");
                return;
            }

            if (_mesero.Password == TxtPassword.Text)
            {
                var result = await DisplayAlert("Eliminar cuenta", "¿Estás seguro de que deseas eliminar tu cuenta?", "Sí", "No");

                if (result) // Solo eliminar si el usuario presiona "Sí"
                {
                    await _db.DeleteAsync(_mesero);
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    Application.Current.MainPage = new NavigationPage(new V_Login());
                }
            }
            else
            {
                await DisplayAlert("Error", "Contraseña incorrecta. Inténtalo de nuevo.", "OK");
            }
        }
    }
}
