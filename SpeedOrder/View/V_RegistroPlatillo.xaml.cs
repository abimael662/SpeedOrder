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
            if (string.IsNullOrWhiteSpace(TxtPlatillo.Text) || string.IsNullOrWhiteSpace(TxtPrecio.Text))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            if (TipoComida.SelectedIndex == 0)
            {
                await DisplayAlert("Error", "Selecciona un tipo de comida", "OK");
                return;
            }

            if (!double.TryParse(TxtPrecio.Text, out double precio))
            {
                await DisplayAlert("Error", "El precio debe ser un número válido", "OK");
                return;
            }

            var menuSeleccionado = (Tables.Menu)TipoComida.SelectedItem;

            string categoria;
            switch (menuSeleccionado.Id_Menu)
            {
                case 1:
                    categoria = "Bebidas";
                    break;
                case 2:
                    categoria = "Desayuno";
                    break;
                case 3:
                    categoria = "Comidas";
                    break;
                case 4:
                    categoria = "Cenas";
                    break;
                case 5:
                    categoria = "Postres";
                    break;
                default:
                    throw new InvalidOperationException("Tipo de comida no válido");
            }

            p = new Platillo
            {
                Nombre_Platillo = TxtPlatillo.Text,
                Precio_Platillo = precio,
                Disponible = TxtDisponible.IsChecked,
                Photo = categoria 
            };

            await _db.InsertAsync(p);

            // Obtener el último platillo insertado
            var platilloInsertado = await _db.Table<Platillo>().OrderByDescending(x => x.Id_Platillo).FirstOrDefaultAsync();

            if (platilloInsertado != null)
            {
                tm = new Tipo_Menu
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Id_Menu = menuSeleccionado.Id_Menu
                };

                // Insertar la relación entre el platillo y el tipo de menú
                await _db.InsertAsync(tm);

                await DisplayAlert("Éxito", "Platillo registrado correctamente", "OK");
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Hubo un problema al insertar el platillo", "OK");
            }
        }
        /*
        private async void BtnRegistrar_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPlatillo.Text) || string.IsNullOrWhiteSpace(TxtPrecio.Text))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            if (TipoComida.SelectedIndex == 0)
            {
                await DisplayAlert("Error", "Selecciona un tipo de comida", "OK");
                return;
            }

            var menuSeleccionado = (Tables.Menu)TipoComida.SelectedItem;

            p = new Platillo
            {
                Nombre_Platillo = TxtPlatillo.Text,
                Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                Disponible = TxtDisponible.IsChecked
            };

            await _db.InsertAsync(p);

            var platilloInsertado = await _db.Table<Platillo>().OrderByDescending(x => x.Id_Platillo).FirstOrDefaultAsync();

            if (platilloInsertado != null)
            {
                tm = new Tipo_Menu
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Id_Menu = menuSeleccionado.Id_Menu
                };

                await _db.InsertAsync(tm);
            }
            

            if (menuSeleccionado.Id_Menu == 1)
            {
                p = new Platillo
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                    Photo = "Bebidas",
                    Disponible = TxtDisponible.IsChecked
                };
                await _db.UpdateAsync(p);
            } else if (menuSeleccionado.Id_Menu == 2)
            {
                p = new Platillo
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                    Photo = "Desayunos",
                    Disponible = TxtDisponible.IsChecked
                };
                await _db.UpdateAsync(p);
            }
            else if (menuSeleccionado.Id_Menu == 3)
            {
                p = new Platillo
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                    Photo = "Comidas",
                    Disponible = TxtDisponible.IsChecked
                };
                await _db.UpdateAsync(p);
            }
            else if (menuSeleccionado.Id_Menu == 4)
            {
                p = new Platillo
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                    Photo = "Cenas",
                    Disponible = TxtDisponible.IsChecked
                };
                await _db.UpdateAsync(p);
            }
            else if (menuSeleccionado.Id_Menu == 5)
            {
                p = new Platillo
                {
                    Id_Platillo = platilloInsertado.Id_Platillo,
                    Nombre_Platillo = TxtPlatillo.Text,
                    Precio_Platillo = Convert.ToDouble(TxtPrecio.Text),
                    Photo = "Postres",
                    Disponible = TxtDisponible.IsChecked
                };
                await _db.UpdateAsync(p);
            }

            await DisplayAlert("Éxito", "Platillo registrado correctamente", "OK");
            await PopupNavigation.Instance.PopAsync();
        }*/
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