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
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_EliminarPlatilloOrden : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Platillo_Orden _platOrden;
        int platillo;

        public V_EliminarPlatilloOrden(int idPlatillo)
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            TxtIdPlatilloOrden.Text = idPlatillo.ToString();
            platillo = idPlatillo;
        }

        private async void BtnEliminarClicked(object sender, EventArgs e)
        {
            _platOrden = await _db.Table<Platillo_Orden>().FirstOrDefaultAsync(po => po.Id_Platillo_Orden == Convert.ToInt32(TxtIdPlatilloOrden.Text));
            if (_platOrden != null)
            {
                await _db.DeleteAsync(_platOrden);
                await DisplayAlert("Éxito", "Platillo eliminado exitosamente", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se encontró el platillo a eliminar.", "OK");
            }
        }

        private async void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            var plaorden = await _db.Table<Platillo_Orden>().FirstOrDefaultAsync(a => a.Id_Platillo == platillo);
            if (plaorden != null)
            {
                int cantidad;
                if (int.TryParse(TxtCantidad.Text, out cantidad) && cantidad > 0)
                {
                    plaorden.Cantidad = cantidad;
                    await _db.UpdateAsync(plaorden);
                    await DisplayAlert("Éxito", "Cantidad actualizada exitosamente", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Por favor, ingrese una cantidad válida.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se encontró el platillo a actualizar.", "OK");
            }
        }

        private async void BtnCerrar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}