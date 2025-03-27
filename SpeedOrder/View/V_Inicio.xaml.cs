using SpeedOrder.Tables;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Inicio : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;

        public V_Inicio()
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
                    datos.Text = $"Bienvenid@ {_mesero.Nombre} {_mesero.Ape_paterno} {_mesero.Ape_materno} a Speed Order \n" +
                                 "¡Listo para comenzar!";
                }
                else
                {
                    datos.Text = "Correo no encontrado. Verifica tus credenciales.";
                }
            }
            else
            {
                datos.Text = "No se ha proporcionado correo.";
            }
        }
    }
}