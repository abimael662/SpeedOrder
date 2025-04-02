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
    public partial class V_RegisterTables : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Meseros _mesero;
        public Mesa m;
        public Ticket t;
        public Orden o;
        public Atender a;

        public V_RegisterTables()
        {
            InitializeComponent();
            BindingContext = App.ViewModelGlobal;
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Mesa>().Wait();
            _db.CreateTableAsync<Orden>().Wait();
            _db.CreateTableAsync<Ticket>().Wait();
            _db.CreateTableAsync<Atender>().Wait();
            _db.CreateTableAsync<Meseros>().Wait();
            TxtTam.ItemsSource = Listas();
            TxtMesa.ItemsSource = Tipo();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var correo = App.ViewModelGlobal.Correo;

            if (string.IsNullOrEmpty(correo))
            {
                await DisplayAlert("Error", "No se ha proporcionado correo.", "OK");
                return;
            }

            _mesero = await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo);
            if (_mesero == null)
            {
                await DisplayAlert("Error", "El mesero no existe en la base de datos.", "OK");
                return;
            }
        }

        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (TxtTam.SelectedIndex == -1 || TxtMesa.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Selecciona una opción", "OK");
                return;
            }

            m = new Mesa
            {
                Tamano = TxtTam.SelectedItem?.ToString(),
                Tipo = TxtMesa.SelectedItem?.ToString()
            };

            await _db.InsertAsync(m);
            m = await _db.Table<Mesa>().OrderByDescending(x => x.Id_Mesa).FirstOrDefaultAsync();

            o = new Orden
            {
                Fecha = DateTime.Now,
                Nombre_Cliente = TxtCliente.Text,
                Subtotal = 0,
                Total = 0
            };

            await _db.InsertAsync(o);
            o = await _db.Table<Orden>().OrderByDescending(x => x.Id_Orden).FirstOrDefaultAsync();

            t = new Ticket
            {
                Id_Orden = o.Id_Orden,
                Id_Mesa = m.Id_Mesa
            };

            await _db.InsertAsync(t);

            a = new Atender
            {
                Id_Mesa = m.Id_Mesa,
                Id_Mesero = _mesero.Id_Mesero
            };

            await _db.InsertAsync(a);

            await DisplayAlert("Éxito", "Mesa registrada correctamente", "OK");
            await PopupNavigation.Instance.PopAsync();
        }

        private async void BtnCerrar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public List<string> Listas()
        {
            return new List<string> { "Grande", "Mediana", "Pequeño" };
        }

        public List<string> Tipo()
        {
            return new List<string> { "Cuadrada", "Circular" };
        }
    }
}