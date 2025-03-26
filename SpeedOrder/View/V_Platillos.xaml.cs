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
        }/*
        public void ActualizarImagen()
        {
            var menu = MenuList.FirstOrDefault(m => m.Tipo == "Comidas" || m.Tipo == "Desayunos" || m.Tipo == "Cenas" || m.Tipo == "Bebidas" || m.Tipo == "Postres");
            foreach (var platillo in _platillo)
            {
                var foto = FotoList.FirstOrDefault(f => f.Name == menu.Tipo);
                if (foto != null)
                {
                    platillo.Photo = foto.Photo;
                }
            }
            ListaPlatillos.ItemsSource = null;
            ListaPlatillos.ItemsSource = _platillo;
        }*/
    }
}