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
	public partial class V_Remover : PopupPage
	{
        public readonly SQLiteAsyncConnection _db;
        public Mesa m;
		public V_Remover ()
		{
			InitializeComponent ();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Mesa>().Wait();
            Buscador();
        }
        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {

            if (TxtBuscar.SelectedItem == null)
            {
                await DisplayAlert("Error", "Selecciona una mesa para eliminar", "OK");
                return;
            }

            int idSeleccionado = (int)TxtBuscar.SelectedItem;
            var mesa = await _db.Table<Mesa>().Where(m => m.Id_Mesa == idSeleccionado).FirstOrDefaultAsync();

            if (mesa != null)
            {
                await _db.DeleteAsync(mesa);
                await DisplayAlert("Éxito", "Mesa eliminada correctamente", "OK");

                TxtId.Text = "";
                TxtTam.Text = "";
                TxtBuscar.SelectedItem = null;
                Buscador();
            }
            else
            {
                await DisplayAlert("Error", "No se encontró la mesa seleccionada", "OK");
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async void Buscador()
        {
            var mesas = await _db.Table<Mesa>().ToListAsync();
            var listaIDs = mesas.Select(m => m.Id_Mesa).ToList();
            TxtBuscar.ItemsSource = listaIDs;
        }
        private async void TxtBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TxtBuscar.SelectedItem != null)
            {
                int idSeleccionado = (int)TxtBuscar.SelectedItem;
                var mesa = await _db.Table<Mesa>().Where(m => m.Id_Mesa == idSeleccionado).FirstOrDefaultAsync();

                if (mesa != null)
                {
                    TxtId.Text = mesa.Id_Mesa.ToString();
                    TxtTam.Text = mesa.Tamano;
                }
            }
        }
    }
}