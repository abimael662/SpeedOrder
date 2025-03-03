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
		public Platillo_Orden po;
        public V_Atendido (Meseros m)
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
            _db.CreateTableAsync<Platillo_Orden>().Wait();
            _m = m;
            TxtMesero.Text = $"{m.Nombre} {m.Ape_paterno} {m.Ape_materno}" ?? "Sin Nombre";
        }
    }
}