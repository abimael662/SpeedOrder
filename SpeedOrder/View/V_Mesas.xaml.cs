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
    public partial class V_Mesas : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        private List<Mesa> _mesa = new List<Mesa>();
        public Mesa m;
        public V_Mesas()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Mesa>().Wait();
        }
        private async void RegisterTable_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_RegisterTables());
        }

        private async void Remover_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_Remover());
        }
    }
}