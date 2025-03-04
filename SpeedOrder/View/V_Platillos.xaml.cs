using SpeedOrder.Tables;
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

namespace SpeedOrder.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Platillos : ContentPage
	{
		public readonly SQLiteAsyncConnection _db;
		public ObservableCollection<Platillo> TPlatillos;
        private List<Platillo> _platillo = new List<Platillo>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public V_Platillos ()
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
        /*
        public async void ActualizarImagen()
        {
            var platillo = MenuList.FirstOrDefault(m => m.Tipo == "Comidas" || m.Tipo == "Desayunos" || m.Tipo == "Cenas" || m.Tipo == "Bebidas" || m.Tipo == "Postres");

            if (platillo != null) {
                var registros = await _db.Table<Tipo_Menu>().Where(tm => tm.Id_Menu == platillo.Id_Menu).ToListAsync();
                var Ids = registros.Select(r => r.Id_Platillo).ToList();
                _platillo = await _db.Table<Platillo>().Where(p => Ids.Contains(p.Id_Platillo)).ToListAsync();
                if (platillo.Tipo == "Comidas")
                {
                    ImgMesas.Source = ImageSource.FromFile("Comidas.png");

                }
                else if (platillo.Tipo == "Desayunos")
                {
                    ImgMesas.Source = ImageSource.FromFile("Comidas.png");
                }
                else if (platillo.Tipo == "Cenas")
                {
                    ImgMesas.Source = ImageSource.FromFile("Comidas.png");
                }
                else if (platillo.Tipo == "Bebidas")
                {
                    ImgMesas.Source = ImageSource.FromFile("Comidas.png");
                }
                else
                {
                    ImgMesas.Source = ImageSource.FromFile("Comidas.png");
                }
            }            
            ListaPlatillos = TPlatillos;
        }*/
    }
}
/*
 *             List<string> Fotos = new List<string>
            {
                "Comidas.png",
                "Desayuno.png",
                "Cenas.png",
                "Bebidas.png",
                "Postres.png"
            };

            var index = Fotos.IndexOf(platillo.Tipo);
            if (platillo != null)
            {
                var imagen = (Image)ListaPlatillos.FindByName("ImagenPlatillo");

                if (imagen != null)
                {
                    var registros = await _db.Table<Tipo_Menu>().Where(tm => tm.Id_Menu == platillo.Id_Menu).ToListAsync();
                    var Ids = registros.Select(r => r.Id_Platillo).ToList();
                    _platillo = await _db.Table<Platillo>().Where(p => Ids.Contains(p.Id_Platillo)).ToListAsync();

                    var imagenes = new Dictionary<string, string>
            {
                { "Comidas", "Comidas.png" },
                { "Desayunos", "Desayuno.png" },
                { "Cenas", "Cenas.png" },
                { "Bebidas", "Bebidas.png" },
                { "Postres", "Postres.png" }
            };

                    if (imagenes.ContainsKey(platillo.Tipo))
                    {
                        imagen.Source = ImageSource.FromResource(imagenes[platillo.Tipo]);
                    }
                }
            }

        public async void ActualizarImagen()
        {
            var platillo = MenuList.FirstOrDefault(m => m.Tipo == "Comidas" || m.Tipo == "Desayunos" || m.Tipo == "Cenas" || m.Tipo == "Bebidas" || m.Tipo == "Postres");

            if (platillo != null)
            {
                var imagen = (Image)ListaPlatillos.FindByName("ImagenPlatillo");

                if (imagen != null)
                {
                    var registros = await _db.Table<Tipo_Menu>().Where(tm => tm.Id_Menu == platillo.Id_Menu).ToListAsync();
                    var Ids = registros.Select(r => r.Id_Platillo).ToList();
                    _platillo = await _db.Table<Platillo>().Where(p => Ids.Contains(p.Id_Platillo)).ToListAsync();
                    if (platillo.Tipo == "Comidas")
                    {
                        imagen.Source = ImageSource.FromResource("Comidas.png");
                    }
                    else if (platillo.Tipo == "Desayunos")
                    {
                        imagen.Source = "Desayuno.png";
                    }
                    else if (platillo.Tipo == "Cenas")
                    {
                        imagen.Source = "Cenas.png";
                    }
                    else if (platillo.Tipo == "Bebidas")
                    {
                        imagen.Source = ImageSource.FromResource("Bebidas.png");
                    }
                    else
                    {
                        imagen.Source = "Postres.png";
                    }
                }
            }
        }
        public void Imagenes()
        {
            var imagenes = new List<string>
            {
                "Bebidas.png",
                "Desayuno.png",
                "Comidas.png",
                "Cenas.png",
                "Postres.png"
            };
        }*/
