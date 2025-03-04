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

            // Pequeña espera para asegurarse de que la mesa se haya guardado en la BD antes de refrescar
            await Task.Delay(500);
            Mesas();
        }

        private async void Remover_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_Remover());
        }

        public async void Mesas()
        {
            _mesa = await _db.Table<Mesa>().ToListAsync();

            // Limpiar el Grid antes de agregar las nuevas mesas
            Device.BeginInvokeOnMainThread(() => Canvas.Children.Clear());

            int columnas = 2; // Número de mesas por fila
            int filas = (_mesa.Count + columnas - 1) / columnas; // Calcula cuántas filas se necesitan

            Canvas.RowDefinitions.Clear();
            Canvas.ColumnDefinitions.Clear();

            for (int i = 0; i < filas; i++)
                Canvas.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < columnas; i++)
                Canvas.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            int index = 0;
            foreach (var mesa in _mesa)
            {
                Image mesaImage = new Image
                {
                    Source = "Redonda.png",
                    HeightRequest = 50,
                    WidthRequest = 50,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                int fila = index / columnas;
                int columna = index % columnas;

                // Agregar imagen al Grid en la posición correspondiente
                Device.BeginInvokeOnMainThread(() => Canvas.Children.Add(mesaImage, columna, fila));

                index++;
            }
        }

        /*
        public async void Mesas()
        {
            _mesa = await _db.Table<Mesa>().ToListAsync();

            if (_mesa.Count > 0 )
            {
                ImgMesa.Source = "Redonda.png";
            }
        }*/
    }
}