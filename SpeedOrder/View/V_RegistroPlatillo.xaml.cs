using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace SpeedOrder.View
{
    public partial class V_RegistroPlatillo : PopupPage
    {
        public readonly SQLiteAsyncConnection _db;
        public Platillo p;
        public Tipo_Menu tm;
        public V_RegistroPlatillo()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();

            List<Tables.Menu> lista = Menus.Datos();
            lista.Insert(0, new Tables.Menu { Id_Menu = 0, Tipo = "-- Tipo de Comida --" });

            TipoComida.ItemsSource = lista;
            TipoComida.ItemDisplayBinding = new Binding("Tipo");
            TipoComida.SelectedIndex = 0;
        }
        private async void BtnRegistrar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtPlatillo.Text) || string.IsNullOrWhiteSpace(TxtPrecio.Text))
                {
                    await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                    return;
                }

                if (TipoComida.SelectedIndex <= 0) // Mejor validación
                {
                    await DisplayAlert("Error", "Selecciona un tipo de comida válido", "OK");
                    return;
                }

                if (!double.TryParse(TxtPrecio.Text, out double precio) || precio <= 0)
                {
                    await DisplayAlert("Error", "Ingresa un precio válido (número positivo)", "OK");
                    return;
                }

                var menuSeleccionado = (Tables.Menu)TipoComida.SelectedItem;

                // Mapeo más seguro
                var categorias = new Dictionary<int, string>
        {
            {1, "Bebidas"},
            {2, "Desayuno"},
            {3, "Comidas"},
            {4, "Cenas"},
            {5, "Postres"}
        };
                if (!categorias.TryGetValue(menuSeleccionado.Id_Menu, out string categoria))
                {
                    await DisplayAlert("Error", "Tipo de comida no válido", "OK");
                    return;
                }

                p = new Platillo
                {
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = precio,
                    Disponible = TxtDisponible.IsChecked,
                    Photo = categoria
                };

                await _db.InsertAsync(p);

                var platilloInsertado = await _db.Table<Platillo>()
                                               .OrderByDescending(x => x.Id_Platillo)
                                               .FirstOrDefaultAsync();

                if (platilloInsertado == null)
                {
                    await DisplayAlert("Error", "Hubo un problema al insertar el platillo", "OK");
                    return;
                }

                tm = new Tipo_Menu
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Id_Menu = menuSeleccionado.Id_Menu
                };

                await _db.InsertAsync(tm);
                await DisplayAlert("Éxito", "Platillo registrado correctamente", "OK");
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
        private async void BtnCerrar_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAsync();
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}