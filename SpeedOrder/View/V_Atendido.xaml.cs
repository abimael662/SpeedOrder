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
	public partial class V_Atendido : ContentPage
	{
		public readonly SQLiteAsyncConnection _db;
		public Gestion g;
        public Meseros _m;
		public Orden o;
        public Mesa ms;
		public Ticket t;
		public Atender a;
		public PLatillo_Orden po;
        public V_Atendido ()
		{
			InitializeComponent ();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Gestion>().Wait();
            _db.CreateTableAsync<Meseros>().Wait();
            _db.CreateTableAsync<Orden>().Wait();
            _db.CreateTableAsync<Mesa>().Wait();
            _db.CreateTableAsync<Ticket>().Wait();
            _db.CreateTableAsync<Atender>().Wait();
            _db.CreateTableAsync<PLatillo_Orden>().Wait();
            //TxtMesero.Text = m.Nombre + " " + m.Ape_paterno + " " + m.Ape_materno;
        }/*
        protected async override void OnAppearing()
        {
            var mesero = await _db.Table<Meseros>().ToListAsync();
            var mesa = await _db.Table<Mesa>().ToListAsync();
            var orden = await _db.Table<Orden>().ToListAsync();
            var ticket = await _db.Table<Ticket>().ToListAsync();
            var atender = await _db.Table<Atender>().ToListAsync();
            var gestion = await _db.Table<Gestion>().ToListAsync();
            var platillo_orden = await _db.Table<PLatillo_Orden>().ToListAsync();
            var mesero1 = mesero.FirstOrDefault(m => m.Id_Mesero == 1);
            var mesa1 = mesa.FirstOrDefault(m => m.Id_Mesa == 1);
            var orden1 = orden.FirstOrDefault(m => m.Id_Orden == 1);
            var ticket1 = ticket.FirstOrDefault(m => m.Id_Ticket == 1);
            var atender1 = atender.FirstOrDefault(m => m.Id_Atender == 1);
            var gestion1 = gestion.FirstOrDefault(m => m.Id_Gestion == 1);
            var platillo_orden1 = platillo_orden.FirstOrDefault(m => m.Id_Orden == 1);
            if (mesero1 != null)
            {
                Mesero.Text = mesero1.Nombre;
            }
            if (mesa1 != null)
            {
                Mesa.Text = mesa1.Tamano;
            }
            if (orden1 != null)
            {
                Orden.Text = orden1.Nombre_Cliente;
            }
            if (ticket1 != null)
            {
                Ticket.Text = ticket1.Id_Ticket.ToString();
            }
            if (atender1 != null)
            {
                Atender.Text = atender1.Id_Atender.ToString();
            }
            if (gestion1 != null)
            {
                Gestion.Text = gestion1.Id_Gestion.ToString();
            }
            if (platillo_orden1 != null)
            {
                Platillo_Orden.Text = platillo_orden1.Id_Orden.ToString();
            }
            base.OnAppearing();
        }*/
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_m != null)
                TxtMesero.Text = $"{_m.Nombre} {_m.Ape_paterno} {_m.Ape_materno}";
        }
    }
}