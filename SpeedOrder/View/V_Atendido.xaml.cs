using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Crypto.Digests;
using QRCoder;
using Rg.Plugins.Popup.Services;
using SpeedOrder.Models;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Internals.Profile;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Atendido : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        private List<Platillo> _platillo = new List<Platillo>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public Gestion g;
        public Meseros _mesero;
        private List<Orden> _orden = new List<Orden>();
        public Orden o;
        public Mesa ms;
        public Ticket t;
        public Atender a;
        //private List<Platillo_Orden> _porden = new List<Platillo_Orden>();
        public Platillo_Orden po;
        private Platillo PlatilloSeleccionado;
        private float total = 0;
        private decimal finaltotal = 0;
        private string nombre;
        private double precio = 0;
        int IdMesa;
        public V_Atendido(int Id_Mesa)
        {
            InitializeComponent();
            BindingContext = App.ViewModelGlobal;
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();
            _db.CreateTableAsync<Gestion>().Wait();
            _db.CreateTableAsync<Meseros>().Wait();
            _db.CreateTableAsync<Orden>().Wait();
            _db.CreateTableAsync<Ticket>().Wait();
            _db.CreateTableAsync<Atender>().Wait();
            _db.CreateTableAsync<Platillo_Orden>().Wait();
            IdMesa = Id_Mesa;
        }
        protected async override void OnAppearing()
        {
            var Registros = await _db.Table<Platillo>().ToListAsync();
            _platillo = Registros.ToList();
            ListaPlatillos.ItemsSource = _platillo;
            var correo = App.ViewModelGlobal.Correo;

            if (!string.IsNullOrEmpty(correo))
            {
                _mesero = (await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo));

                if (_mesero != null)
                {
                    TxtMesero.Text = $"{_mesero.Nombre} {_mesero.Ape_paterno} {_mesero.Ape_materno}";
                }
            }
            ObtenerOrden();
            base.OnAppearing();
        }
        private async void PDF_Clicked(object sender, EventArgs e)
        {
            // Resetear el total para que no se acumule entre tickets
            total = 0;

            // Definir el tamaño personalizado de la página
            float width = 80 * 2.835f; // Convertir 80 mm a puntos
            float height = 200 * 2.835f; // Convertir 200 mm a puntos
            var customSize = new iTextSharp.text.Rectangle(width, height);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(customSize);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Definir fuentes
                iTextSharp.text.Font font = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 6);
                iTextSharp.text.Font sub = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 4);
                iTextSharp.text.Font totalFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);  // Fuente más grande para el total

                // Agregar una línea separadora que abarque todo el ancho de la página
                PdfPTable separatorTable = new PdfPTable(1); // Solo una columna para abarcar todo el ancho
                separatorTable.WidthPercentage = 100; // Hacer que ocupe el 100% del ancho de la página

                // Crear una celda con un borde para representar la línea
                PdfPCell separatorCell = new PdfPCell(new Phrase(""))
                {
                    BorderWidthTop = 1f, // Configurar el borde superior para hacer la línea
                    BorderWidthBottom = 0f, // No mostrar el borde inferior
                    BorderWidthLeft = 0f, // No mostrar los bordes laterales
                    BorderWidthRight = 0f, // No mostrar los bordes laterales
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER, // Alineación centrada
                    VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                };

                // Agregar la celda a la tabla
                separatorTable.AddCell(separatorCell);

                // Título del ticket
                Paragraph title = new Paragraph("TICKET DE COMPRA")
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };
                document.Add(title);

                // Obtener la ruta de la imagen
                string rutaImagen = DependencyService.Get<IFileService>().ObtenerRutaImagen();
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(rutaImagen);

                imagen.BorderWidth = 0;
                imagen.Alignment = iTextSharp.text.Element.ALIGN_CENTER | iTextSharp.text.Element.ALIGN_TOP;

                float percentage = 100 / imagen.Width;
                imagen.ScalePercent(percentage * 100);

                // Establecer la opacidad de la imagen
                PdfContentByte cb = writer.DirectContentUnder;
                PdfGState state = new PdfGState();
                state.FillOpacity = 0.2f; // Ajustar opacidad
                cb.SetGState(state);

                imagen.SetAbsolutePosition(60f, 400f);  // Establecer la posición
                                                        // Agregar la imagen al fondo del documento con opacidad
                cb.AddImage(imagen);

                // Agregar el título y la información
                Paragraph spe = new Paragraph("SPEED ORDER", sub)
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };
                document.Add(spe);

                string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                document.Add(new Paragraph($"Fecha y Hora: {creationDate} \n", font));
                
                PdfPTable tableInicio = new PdfPTable(2); 
                tableInicio.WidthPercentage = 100;

                PdfPCell ticket = new PdfPCell(new Phrase("No.Ticket: ", font))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                };

                PdfPCell mesa = new PdfPCell(new Phrase($"Mesa: {IdMesa}", font))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER, // Sin borde
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT, // Alinear a la derecha
                    VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                };

                // Agregar los encabezados a la tabla
                tableInicio.AddCell(ticket);
                tableInicio.AddCell(mesa);

                document.Add(tableInicio);

                document.Add(separatorTable);

                // Asegurarse de que el cliente tenga un valor asignado en TxtCliente
                string cliente = string.IsNullOrEmpty(TxtCliente.Text) ? "Cliente no especificado" : TxtCliente.Text;
                document.Add(new Paragraph($"Mesero: {TxtMesero.Text} \nCliente: {cliente} \n\n", font));

                document.Add(separatorTable);
                // Crear la tabla para mostrar Producto y Precio
                PdfPTable table = new PdfPTable(2); // Dos columnas: una para Producto y otra para Precio
                table.WidthPercentage = 100; // Ancho completo de la página

                // Configurar las celdas para Producto y Precio
                PdfPCell cellProducto = new PdfPCell(new Phrase("Producto", font)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT };
                PdfPCell cellPrecio = new PdfPCell(new Phrase("Precio", font)) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT };

                // Agregar los encabezados a la tabla
                table.AddCell(cellProducto);
                table.AddCell(cellPrecio);

                // Agregar los platillos a la tabla
                foreach (var platillo in _platillo)
                {
                    nombre = platillo.Nombre_Platillo;
                    precio = platillo.Precio_Platillo;

                    // Agregar las celdas para el platillo
                    table.AddCell(new PdfPCell(new Phrase(nombre, font))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER, // Sin borde
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT, // Alinear a la izquierda
                        VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                    });
                    table.AddCell(new PdfPCell(new Phrase($"${precio:F2}", font)) {
                        Border = iTextSharp.text.Rectangle.NO_BORDER, // Sin borde
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                    });

                        // Sumar al total
                        total += Convert.ToInt32(platillo.Precio_Platillo);
                }

                // Agregar la tabla al documento
                document.Add(table);

                // Agregar la tabla con la línea al documento
                document.Add(separatorTable);

                PdfPTable totalTable = new PdfPTable(2); // Dos columnas: una para TOTAL y otra para el resultado
                totalTable.WidthPercentage = 100; // Ancho completo de la página

                // Configurar las celdas para TOTAL y el total numérico
                PdfPCell cellTotalLabel = new PdfPCell(new Phrase("TOTAL:", totalFont))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER, // Sin borde
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT, // Alinear a la izquierda
                    VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                };

                PdfPCell cellTotalValue = new PdfPCell(new Phrase($"${total:F2}", totalFont))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER, // Sin borde
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT, // Alinear a la derecha
                    VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                };

                // Agregar las celdas a la tabla del total
                totalTable.AddCell(cellTotalLabel);
                totalTable.AddCell(cellTotalValue);

                document.Add(totalTable);

                // Agregar un mensaje final
                Paragraph paragraph = new Paragraph("Gracias por su compra", font)
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };
                document.Add(paragraph);

                // Cerrar el documento
                document.Close();
                writer.Close();

                byte[] pdfBytes = memoryStream.ToArray();

                // Imprimir el PDF generado
                await PrintPDF(pdfBytes);
            }
        }
        private async Task PrintPDF(byte[] pdfData)
        {
            try
            {
                DependencyService.Get<IPrintService>().PrintPDF(pdfData);
            }
            catch (Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", $"Error al imprimir: {ex.Message}", "OK");
            }
        }

        private void ListaPlatillos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            PlatilloSeleccionado = e.SelectedItem as Platillo;
            ListaPlatillos.SelectedItem = null;
        }
        private async void TxtCantidad_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is int idPlatillo)
            {
                await PopupNavigation.Instance.PushAsync(new V_PlatilloOrden(idPlatillo));
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener el ID del platillo.", "OK");
            }
        }
        public async void Orden()
        {
            if (Propina.IsChecked)
            {
                finaltotal = Convert.ToDecimal(total) * 1.15m;
            }
            else
            {
                finaltotal = Convert.ToDecimal(total);
            }

            o = new Orden
            {
                Fecha = DateTime.Now,
                Nombre_Cliente = TxtCliente.Text,
                Subtotal = Convert.ToDecimal(total),
                Total = finaltotal,
            };
            await _db.InsertAsync(o);
        }
        public async void ObtenerOrden()
        {
            var registro = await _db.Table<Orden>().ToListAsync();

            if (registro.Any())
            {
                var ultimaOrden = registro.Last();

                po = await _db.Table<Platillo_Orden>().FirstOrDefaultAsync(m => m.Id_Orden == ultimaOrden.Id_Orden);
            }
        }
        private async void QR_Clicked(object sender, EventArgs e)
        {
            string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string mesero = TxtMesero.Text;
            string cliente = TxtCliente.Text;

            string cadena = $"Fecha: {creationDate}\nMesa: {IdMesa}\nMesero: {mesero}\nCliente: {cliente}\nTotal: ${total}\nGracias por su compra";

            await PopupNavigation.Instance.PushAsync(new V_QR(cadena));
        }
    }
}