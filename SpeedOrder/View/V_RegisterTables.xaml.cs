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
        public Mesa m;
        public V_RegisterTables ()
		{
			InitializeComponent ();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Mesa>().Wait();
            TxtTam.ItemsSource = Listas();
            TxtMesa.ItemsSource = Tipo();
        }
        private async void BtnRegistrar_Clicked(object sender, System.EventArgs e)
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
            await DisplayAlert("Éxito", "Mesa registrada correctamente", "OK");
            await PopupNavigation.Instance.PopAsync();
        }
        private async void BtnCerrar_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public List<string> Listas()
        {
            return new List<string>
            {
                "Grande",
                "Mediana",
                "Pequeño"
            };
        }
        public List<string> Tipo()
        {
            return new List<string>
            {
                "Cuadrada",
                "Circular"
            };
        }
    }
}