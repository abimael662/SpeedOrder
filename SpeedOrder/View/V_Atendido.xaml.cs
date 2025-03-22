using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Crypto.Digests;
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

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Atendido : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        private List<Platillo> _platillo = new List<Platillo>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public List<Foto> FotoList = ListFotos.Datos();
        private List<Foto> _fotos = new List<Foto>();
        public Gestion g;
        public Meseros _m;
        public Orden o;
        public Mesa ms;
        public Ticket t;
        public Atender a;
        public Platillo_Orden po;
        public V_Atendido()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Platillo>().Wait();
            _db.CreateTableAsync<Tipo_Menu>().Wait();
            _db.CreateTableAsync<Gestion>().Wait();
            _db.CreateTableAsync<Meseros>().Wait();
            _db.CreateTableAsync<Orden>().Wait();
            _db.CreateTableAsync<Mesa>().Wait();
            _db.CreateTableAsync<Ticket>().Wait();
            _db.CreateTableAsync<Atender>().Wait();
            _db.CreateTableAsync<Platillo_Orden>().Wait();
        }
        protected async override void OnAppearing()
        {
            var Registros = await _db.Table<Platillo>().ToListAsync();
            _platillo = Registros.ToList();
            ListaPlatillos.ItemsSource = _platillo;
            base.OnAppearing();
        }

        public void ActualizarImagen()
        {
            var platillo = MenuList.FirstOrDefault(m => m.Tipo == "Comidas" || m.Tipo == "Desayunos" || m.Tipo == "Cenas" || m.Tipo == "Bebidas" || m.Tipo == "Postres");

            if (platillo != null)
            {
                var foto = FotoList.FirstOrDefault(f => f.Name == platillo.Tipo);

                if (foto != null)
                {
                    var img = ImageSource.FromFile(foto.Photo);
                    ListaPlatillos.ItemsSource = (System.Collections.IEnumerable)img;
                }
            }
        }
        private async void PDF_Clicked(object sender, EventArgs e)
        {
            float width = 80 * 2.835f;
            float height = 200 * 2.835f;
            var customSize = new iTextSharp.text.Rectangle(width, height);

            float total = 0;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(customSize);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                var imagen = ImageSource.FromFile("Resources/drawable/MAELDEVS.png");

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagen.ToString());
                img.BorderWidth = 0;
                img.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                float percentage = 0.0f;
                percentage = 150 / img.Width;
                img.ScalePercent(percentage * 100);

                document.Add(img);

                /*string imagePath = @"C:\Users\abima\source\repos\SpeedOrder\SpeedOrder.Android\Resources\drawable\MAELDEVS.png";
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(imagePath);
                imagen.BorderWidth = 0;
                imagen.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                float percentage = 0.0f;
                percentage = 150 / imagen.Width;
                imagen.ScalePercent(percentage * 100);

                document.Add(imagen);*/

                iTextSharp.text.Font font = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 6);

                Paragraph title = new Paragraph("TICKET DE COMPRA")
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };

                document.Add(title);

                string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                document.Add(new Paragraph($"Fecha de Creación: {creationDate}", font));

                document.Add(new Paragraph($"Mesa: ", font));
                document.Add(new Paragraph($"Mesero: \n{TxtMesero.Text} \nCliente: \n{TxtCliente.Text}", font));
                document.Add(new Paragraph($"Comida: \n$25 Soda\n$250 Sopa", font));
                document.Add(new Paragraph($"Total: {total}", font));

                document.Add(new Paragraph("Gracias por su compra", font));

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
    }
}