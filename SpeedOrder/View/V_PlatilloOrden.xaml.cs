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
        public Orden _o;
        public Mesa _ms;
        public Ticket _t;
        public Platillo _p;
        public Atender _a;
        public Platillo_Orden po;
        public V_PlatilloOrden (Platillo p)
		{
			InitializeComponent ();
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
            _p = p;
            TxtIdPlatillo.Text = p.Id_Platillo.ToString();
        }
        private async void BtnRegistrar_Clicked(object sender, System.EventArgs e)
		{
            po = new Platillo_Orden
            {
                Id_Orden = Convert.ToInt32(TxtIdOrden.Text),
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