using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_PlatilloOrden : PopupPage
	{
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        private List<Mesa> _mesa = new List<Mesa>();
        private List<Orden> _orden = new List<Orden>();
        private List<Platillo> _platillo = new List<Platillo>();
        public Gestion _g;
        public Meseros _m;
        public Orden o;
        public Mesa _ms;
        public Ticket _t;
        public Platillo _p;
        public Atender _a;
        public Platillo_Orden po;
        int ordennum;
        public V_PlatilloOrden (int idPlatillo)
		{
			InitializeComponent ();
            BindingContext = App.ViewModelGlobal;
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();
            _db.CreateTableAsync<Gestion>().Wait();
            _db.CreateTableAsync<Meseros>().Wait();
            _db.CreateTableAsync<Orden>().Wait();
            _db.CreateTableAsync<Mesa>().Wait();
            _db.CreateTableAsync<Ticket>().Wait();
            _db.CreateTableAsync<Atender>().Wait();
            _db.CreateTableAsync<Platillo_Orden>().Wait();
            TxtIdPlatillo.Text = idPlatillo.ToString();
            CargarMesas();
        }
        private async void CargarMesas()
        {
            try
            {
                var mesas = await _db.Table<Mesa>().ToListAsync();
                TxtIdMenu.ItemsSource = mesas.Select(m => m.Id_Mesa.ToString()).ToList();
                var correo = App.ViewModelGlobal.Correo;
                _m = await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo);
                var atender = (await _db.Table<Atender>().FirstOrDefaultAsync(a => a.Id_Mesero == _m.Id_Mesero));
                var ticket = (await _db.Table<Ticket>().FirstOrDefaultAsync(a => a.Id_Mesa == atender.Id_Mesa));
                var orden = (await _db.Table<Orden>().FirstOrDefaultAsync(a => a.Id_Orden == ticket.Id_Orden));
                ordennum = orden.Id_Orden;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar las mesas: {ex.Message}", "OK");
            }
        }
        private async void BtnRegistrar_Clicked(object sender, System.EventArgs e)
		{
            po = new Platillo_Orden
            {
                Id_Orden = Convert.ToInt32(ordennum),
                Id_Platillo = Convert.ToInt32(TxtIdPlatillo.Text),
                Cantidad = Convert.ToInt32(TxtCantidad.Text)
            };
            await _db.InsertAsync(po);
            await DisplayAlert("Registro", "Platillo registrado", "OK");
            await PopupNavigation.Instance.PopAsync();
        }
        private async void BtnCerrar_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}