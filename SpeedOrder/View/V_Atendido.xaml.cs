using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Crypto.Digests;
using QRCoder;
using Rg.Plugins.Popup.Services;
using SpeedOrder.Models;
using SpeedOrder.Tables;
using SQLite;
using System;
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
        public Orden o;
        public Mesa ms;
        public Ticket t;
        public Atender a;
        public Platillo_Orden po;
        private Platillo PlatilloSeleccionado;
        private float total = 0;
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
            else
            {
                TxtMesero.Text = "No se ha proporcionado correo.";
            }
            base.OnAppearing();
        }
        private async void PDF_Clicked(object sender, EventArgs e)
        {
            float width = 80 * 2.835f;
            float height = 200 * 2.835f;
            var customSize = new iTextSharp.text.Rectangle(width, height);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(customSize);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                iTextSharp.text.Font font = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 6);
                iTextSharp.text.Font sub = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 4);

                Paragraph title = new Paragraph("TICKET DE COMPRA")
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };
                /*

                string rutaImagen = DependencyService.Get<IFileService>().ObtenerRutaImagen("MAELDEVS");

                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(rutaImagen);
                imagen.BorderWidth = 0;
                imagen.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                float percentage = 150 / imagen.Width;
                imagen.ScalePercent(percentage * 100);
                document.Add(imagen);
                */

                document.Add(title);
                Paragraph spe = new Paragraph("SPEED ORDER", sub);
                spe.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                document.Add(spe);

                string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                document.Add(new Paragraph($"Fecha y Hora: {creationDate}", font));
                document.Add(new Paragraph($"No.Ticket: ", font));

                document.Add(new Paragraph($"Mesa: {IdMesa}", font));
                document.Add(new Paragraph($"Mesero: \n{TxtMesero.Text} \nCliente: \n{TxtCliente.Text}", font));
                foreach (var platillo in _platillo)
                {
                    nombre = platillo.Nombre_Platillo;
                    precio = platillo.Precio_Platillo;
                    document.Add(new Paragraph($"{nombre} - ${precio}", font));
                    total += Convert.ToInt32(platillo.Precio_Platillo);
                }

                document.Add(new Paragraph($"Total: ${total}", font));
                Paragraph paragraph = new Paragraph("Gracias por su compra", font);
                paragraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                document.Add(paragraph);

                document.Close();
                writer.Close();

                byte[] pdfBytes = memoryStream.ToArray();

                await PrintPDF(pdfBytes);
            }
        }
        private async Task PrintPDF(byte[] pdfData)
        {
            try
            {
                DependencyService.Get<IPrintService>().PrintPDF(pdfData);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Impresión", "Documento enviado a la impresora", "OK");
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

        private async void QR_Clicked(object sender, EventArgs e)
        {
            string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string mesero = TxtMesero.Text;
            string cliente = TxtCliente.Text;

            string cadena = $"Fecha: {creationDate}\nMesa: {IdMesa}\nMesero: {mesero}\nCliente: {cliente}\nTotal: ${total}\nGracias por su compra";

            await PopupNavigation.Instance.PushAsync(new V_QR(cadena));
            /*
            string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string mesero = TxtMesero.Text;
            string cliente = TxtCliente.Text;
            string comida = nombre;
            string cadena = $"Fecha de Creación: {creationDate}\nMesa: {IdMesa} \nMesero: {mesero}\nCliente: {cliente}\nComida: \n{comida}\nTotal: {total}\nGracias por su compra";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(cadena, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            var stream = new MemoryStream(qrCodeBytes);
            //imagenQR.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));*/
        }
    }
}