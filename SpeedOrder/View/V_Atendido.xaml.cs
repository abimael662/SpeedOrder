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
        private List<Platillo_Orden> _platilloOr = new List<Platillo_Orden>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public Gestion g;
        public Meseros _mesero;
        private List<Orden> _orden = new List<Orden>();
        public Orden o;
        public Mesa ms;
        public Ticket t;
        public Atender a;
        public Platillo_Orden po;
        private float total = 0;
        private decimal finaltotal = 0;
        private string nombre;
        private double precio = 0;
        int IdMesa;
        string nomcliente;
        int numtiket;
        int cantidad;
        float subtotal;
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
            base.OnAppearing();
            try
            {
                var correo = App.ViewModelGlobal.Correo;

                if (!string.IsNullOrEmpty(correo))
                {
                    _mesero = await _db.Table<Meseros>().FirstOrDefaultAsync(m => m.Email == correo);
                    ObtenerOrden();
                    if (_mesero != null)
                    {
                        TxtMesero.Text = $"{_mesero.Nombre} {_mesero.Ape_paterno} {_mesero.Ape_materno}";
                        var atender = (await _db.Table<Atender>().FirstOrDefaultAsync(a => a.Id_Mesero == _mesero.Id_Mesero));
                        var ticket = (await _db.Table<Ticket>().FirstOrDefaultAsync(a => a.Id_Mesa == atender.Id_Mesa));
                        var orden = (await _db.Table<Orden>().FirstOrDefaultAsync(a => a.Id_Orden == ticket.Id_Orden));
                        if (orden != null)
                        {
                            TxtCliente.Text = $"{orden.Nombre_Cliente}";
                            nomcliente = orden.Nombre_Cliente;
                            numtiket = ticket.Id_Ticket;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
            }
        }

        public async void ObtenerOrden()
        {
            try
            {
                if (_mesero == null)
                {
                    await DisplayAlert("Error", "No se encontró el mesero.", "OK");
                    return;
                }

                var atender = await _db.Table<Atender>().FirstOrDefaultAsync(a => a.Id_Mesero == _mesero.Id_Mesero);
                if (atender == null)
                {
                    await DisplayAlert("Error", "No se encontró la mesa atendida.", "OK");
                    return;
                }

                var ticket = await _db.Table<Ticket>().FirstOrDefaultAsync(a => a.Id_Mesa == atender.Id_Mesa);
                if (ticket == null)
                {
                    await DisplayAlert("Error", "No se encontró el ticket.", "OK");
                    return;
                }

                var ordenPlatillos = await _db.Table<Platillo_Orden>().Where(po => po.Id_Orden == ticket.Id_Orden).ToListAsync();
                if (ordenPlatillos == null || !ordenPlatillos.Any())
                {
                    await DisplayAlert("Error", "No hay platillos en la orden.", "OK");
                    return;
                }

                var platillos = new List<Platillo>();
                subtotal = 0;

                foreach (var po in ordenPlatillos)
                {
                    var platillo = await _db.Table<Platillo>().FirstOrDefaultAsync(p => p.Id_Platillo == po.Id_Platillo);
                    if (platillo != null)
                        platillos.Add(platillo);

                    subtotal += (float)(platillo.Precio_Platillo * po.Cantidad);
                }

                _platillo = platillos;
                ListaPlatillos.ItemsSource = null;  // Resetear lista
                ListaPlatillos.ItemsSource = _platillo;
                CalcularTotales();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al obtener orden: {ex.Message}", "OK");
            }
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

                PdfPCell ticket = new PdfPCell(new Phrase($"No.Ticket:{numtiket} ", font))
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
                document.Add(new Paragraph($"Mesero: {TxtMesero.Text} \nCliente: {nomcliente} \n\n", font));

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
                foreach (var po in await _db.Table<Platillo_Orden>().Where(po => po.Id_Orden == numtiket).ToListAsync())
                {
                    var platillo = await _db.Table<Platillo>().FirstOrDefaultAsync(p => p.Id_Platillo == po.Id_Platillo);
                    if (platillo != null)
                    {
                        nombre = platillo.Nombre_Platillo;
                        precio = platillo.Precio_Platillo;
                        cantidad = po.Cantidad;  // Obtener la cantidad del platillo ordenado
                        subtotal = (float)(precio * cantidad); // Calcular el subtotal por platillo

                        // Agregar platillo y su subtotal al PDF
                        table.AddCell(new PdfPCell(new Phrase($"{nombre} (x{cantidad})", font))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                            VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                        });
                        table.AddCell(new PdfPCell(new Phrase($"${subtotal:F2}", font))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
                        });

                        total += subtotal;  // ✅ Ahora sumamos correctamente la cantidad * precio
                    }
                }

                // Agregar la tabla al documento
                document.Add(table);

                // Agregar la tabla con la línea al documento
                document.Add(separatorTable);

                // Agregar subtotal al ticket
                PdfPTable subtotalTable = new PdfPTable(2);
                subtotalTable.WidthPercentage = 100;
                subtotalTable.AddCell(new PdfPCell(new Phrase("Subtotal:", totalFont))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                });
                subtotalTable.AddCell(new PdfPCell(new Phrase($"${total:F2}", totalFont)) // Usamos 'total' que ya tiene la suma de los subtotales
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                });
                document.Add(subtotalTable);

                // Agregar propina si está activada
                if (Propina.IsChecked)
                {
                    decimal propina = (decimal)total * 0.15m; // Propina del 15%
                    PdfPTable propinaTable = new PdfPTable(2);
                    propinaTable.WidthPercentage = 100;
                    propinaTable.AddCell(new PdfPCell(new Phrase("Propina (15%):", totalFont))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                    });
                    propinaTable.AddCell(new PdfPCell(new Phrase($"${propina:F2}", totalFont))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                    });
                    document.Add(propinaTable);
                }

                // Calcular el total final (subtotal + propina, si aplica)
                finaltotal = (decimal)total;
                if (Propina.IsChecked)
                {
                    finaltotal += (decimal)total * 0.15m; // Agregar la propina al total final
                }

                // Agregar total final
                PdfPTable totalTable = new PdfPTable(2);
                totalTable.WidthPercentage = 100;
                totalTable.AddCell(new PdfPCell(new Phrase("TOTAL:", totalFont))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                });
                totalTable.AddCell(new PdfPCell(new Phrase($"${finaltotal:F2}", totalFont))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                });
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
        private async void TxtCantidad_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is int idPlatillo)
            {
                var atender = await _db.Table<Atender>().FirstOrDefaultAsync(a => a.Id_Mesero == _mesero.Id_Mesero);
                if (atender == null)
                {
                    await DisplayAlert("Error", "No se encontró el mesero asociado.", "OK");
                    return;
                }

                var ticket = await _db.Table<Ticket>().FirstOrDefaultAsync(a => a.Id_Mesa == atender.Id_Mesa);
                if (ticket == null)
                {
                    await DisplayAlert("Error", "No se encontró el ticket para esta mesa.", "OK");
                    return;
                }

                var orden = await _db.Table<Orden>().FirstOrDefaultAsync(a => a.Id_Orden == ticket.Id_Orden);
                if (orden == null)
                {
                    await DisplayAlert("Error", "No se encontró la orden asociada a este ticket.", "OK");
                    return;
                }

                var platillorden = await _db.Table<Platillo_Orden>().FirstOrDefaultAsync(a => a.Id_Orden == ticket.Id_Orden && a.Id_Platillo == idPlatillo);
                if (platillorden == null)
                {
                    await DisplayAlert("Error", "No se encontró el platillo en esta orden.", "OK");
                    return;
                }

                int pla = platillorden.Id_Platillo_Orden;
                await PopupNavigation.Instance.PushAsync(new V_EliminarPlatilloOrden(pla));
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener el ID del platillo.", "OK");
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
        private void CalcularTotales()
        {
            // Aplicar propina del 15% si el checkbox está marcado
            decimal propina = Propina.IsChecked ? (decimal)subtotal * 0.15m : 0m;

            // Calcular el total final
            finaltotal = (decimal)subtotal + propina;

            // Mostrar los valores en los Entries
            TxtSubtotal.Text = $"${subtotal:F2}";
            TxtTotal.Text = $"${finaltotal:F2}";
        }
        private void Propina_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CalcularTotales();
        }
    }
}