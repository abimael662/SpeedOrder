using Rg.Plugins.Popup.Services;
using SpeedOrder.Models;
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

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Comidas : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        private List<Platillo> _platillo = new List<Platillo>();
        List<Tables.Menu> lista = Menus.Datos();
        public V_Comidas()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();
        }
        private async void Registrar_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_RegistroPlatillo());
        }
        protected async override void OnAppearing()
        {
            var comidas = lista.FirstOrDefault(m => m.Tipo == "Comidas");

            if (comidas != null)
            {
                var registros = await _db.Table<Tipo_Menu>().Where(tm => tm.Id_Menu == comidas.Id_Menu).ToListAsync();
                var Ids = registros.Select(r => r.Id_Platillo).ToList();
                _platillo = await _db.Table<Platillo>().Where(p => Ids.Contains(p.Id_Platillo)).ToListAsync();
                ListaComidas.ItemsSource = _platillo;
            }
            else
            {
                ListaComidas.ItemsSource = new List<Platillo>();
            }
            base.OnAppearing();
        }
        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                await CheckBoxHelper.HandleCheckBoxChangedAsync(checkBox, e, _db);
            }
        }
    }
}