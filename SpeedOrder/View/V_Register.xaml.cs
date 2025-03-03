using SQLite;
using SpeedOrder.Models;
using SpeedOrder.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Register : ContentPage
    {
        Meseros _m;
        private SQLiteAsyncConnection conexion;
        public V_Register()
        {
            InitializeComponent();
            conexion = DependencyService.Get<ConexionSQL>().GetConnection();
        }

        private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombre.Text) ||
                string.IsNullOrEmpty(TxtAPaterno.Text) ||
                string.IsNullOrEmpty(TxtAMaterno.Text) ||
                string.IsNullOrEmpty(TxtEdad.Text) ||
                string.IsNullOrEmpty(TxtPassword.Text) ||
                string.IsNullOrEmpty(TxtEmail.Text))
            {
                await PopupNavigation.Instance.PushAsync(new V_AlertRegister());
                return;
            }
            var m = new Meseros
            {
                Nombre = TxtNombre.Text,
                Ape_paterno = TxtAPaterno.Text,
                Ape_materno = TxtAMaterno.Text,
                Edad = Convert.ToInt16(TxtEdad.Text),
                Password = TxtPassword.Text,
                Email = TxtEmail.Text,
            };

            await conexion.CreateTableAsync<Meseros>();
            await conexion.InsertAsync(m);
            await Navigation.PushAsync(new V_Tabulador(m));
        }
    }
}