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
using Xamarin.Essentials;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Register : ContentPage
    {
        // Creamos la conexion a la base de datos
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
            // Validamos que los campos no esten vacios
            if (string.IsNullOrEmpty(TxtNombre.Text) || string.IsNullOrEmpty(TxtAPaterno.Text) ||
                string.IsNullOrEmpty(TxtAMaterno.Text) || string.IsNullOrEmpty(TxtEdad.Text) ||
                string.IsNullOrEmpty(TxtPassword.Text) || string.IsNullOrEmpty(TxtEmail.Text))
            {
                await PopupNavigation.Instance.PushAsync(new V_AlertRegister());
                return;
            }

            // Verificamos que el correo sea valido
            string verificar = TxtEmail.Text;

            // Verificamos si el correo ya esta registrado
            if (await Comprovar(verificar))
            {
                await DisplayAlert("Error", "El correo ya esta registrado", "Aceptar");
                return;
            }
            // Primero, limpiamos los valores y los guardamos en variables
            string nombre = TxtNombre.Text.Trim();
            string apePaterno = TxtAPaterno.Text.Trim();
            string apeMaterno = TxtAMaterno.Text.Trim();
            string edad = TxtEdad.Text.Trim();
            string password = TxtPassword.Text.Trim();
            string email = TxtEmail.Text.Trim();

            // Convertimos y asignamos los valores al objeto
            var m = new Meseros
            {
                Nombre = nombre,
                Ape_paterno = apePaterno,
                Ape_materno = apeMaterno,
                Edad = Convert.ToInt16(edad),
                Password = password,
                Email = email,
            };

            // Insertamos el objeto en la base de datos
            await conexion.CreateTableAsync<Meseros>();
            await conexion.InsertAsync(m);
            //await Navigation.PushAsync(new V_Tabulador(m));
            await Navigation.PushAsync(new V_Login());
            // Limpiamos los campos llamando al metodo Limpiar
            Limpiar();
        }
        private async Task<bool> Comprovar(string email)
        {
            // Buscamos si el correo ya esta registrado
            var mesero = await conexion.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == email);
            return mesero != null;
        }
        public void Limpiar()
        {
            // Limpiamos los campos
            TxtNombre.Text = "";
            TxtAPaterno.Text = "";
            TxtAMaterno.Text = "";
            TxtEdad.Text = "";
            TxtPassword.Text = "";
            TxtEmail.Text = "";
        }
    }
}