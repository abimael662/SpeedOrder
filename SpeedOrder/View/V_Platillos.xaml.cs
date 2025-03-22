using iTextSharp.text;
using SpeedOrder.Tables;
using SpeedOrder.ViewModel;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.TableMapping;
using static Xamarin.Essentials.Permissions;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Platillos : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        private List<Platillo> _platillo = new List<Platillo>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public string Foto { get; set; }
        public V_Platillos()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();
        }
        
        protected async override void OnAppearing()
        {
            var Registros = await _db.Table<Platillo>().ToListAsync();
            _platillo = Registros.ToList();
            ListaPlatillos.ItemsSource = _platillo;
            base.OnAppearing();
        }
        
        public async void ActualizarImagen()
        {
            var platillo = MenuList.FirstOrDefault(m => m.Tipo == "Comidas" || m.Tipo == "Desayunos" || m.Tipo == "Cenas" || m.Tipo == "Bebidas" || m.Tipo == "Postres");

            if (platillo != null)
            {
                var registros = await _db.Table<Tipo_Menu>().Where(tm => tm.Id_Menu == platillo.Id_Menu).ToListAsync();
                var Ids = registros.Select(r => r.Id_Platillo).ToList();
                _platillo = await _db.Table<Platillo>().Where(p => Ids.Contains(p.Id_Platillo)).ToListAsync();

                if (platillo.Tipo == "Comidas")
                {
                    Foto = "Comidas";
                }
                else if (platillo.Tipo == "Desayunos")
                {
                    Foto = "Desayuno";
                }
                else if (platillo.Tipo == "Cenas")
                {
                    Foto = "Cenas";
                }
                else if (platillo.Tipo == "Bebidas")
                {
                    Foto = "Bebidas";
                }
                else
                {
                    Foto = "Postres";
                }
            }
            /*TPlatillos = new ObservableCollection<Platillo>(_platillo);
            ListaPlatillos.ItemsSource = TPlatillos;*/
        }
    }
}