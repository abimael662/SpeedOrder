using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Org.BouncyCastle.Crypto.Digests;
using SpeedOrder.Models;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Atendido : ContentPage
    {
        public readonly SQLiteAsyncConnection _db;
        public ObservableCollection<Platillo> TPlatillos;
        //public ObservableCollection<Foto> TFoto;
        private List<Platillo> _platillo = new List<Platillo>();
        public List<Tables.Menu> MenuList = Menus.Datos();
        public List<Foto> FotoList = ListFotos.Datos();
        private List<Foto> _fotos = new List<Foto>();
        //public List<Foto> Fot = Fotos.Datos();*/
        public Gestion g;
        public Meseros _m;
        public Orden o;
        public Mesa ms;
        public Ticket t;
        public Atender a;
        public Platillo_Orden po;
        //public V_Atendido(Meseros m)
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
            /*_m = m;
            TxtMesero.Text = $"{m.Nombre} {m.Ape_paterno} {m.Ape_materno}" ?? "Sin Nombre";*/
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
                    //ListaPlatillos.ItemsSource = _platillo.Concat(_fotos).ToList();
                    //ListaPlatillos.ItemsSource = _fotos;
                }
            }
        }
        private async void PDF_Clicked(object sender, EventArgs e)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                var nombreMesero = TxtMesero.Text ?? "Sin Nombre";
                var nombreCliente = TxtCliente.Text ?? "Sin Cliente";

                document.Add(new Paragraph("Atendido por: " + nombreMesero));
                document.Add(new Paragraph("Cliente: " + nombreCliente));
                document.Add(new Paragraph("Platillos:"));

                // Aquí puedes agregar los platillos a la lista si los tienes
                /*
                foreach (var platillo in ListaPlatillos.ItemsSource)
                {
                    var nombrePlatillo = platillo.Nombre_Platillo;
                    var precioPlatillo = platillo.Precio_Platillo.ToString("C");
                    document.Add(new Paragraph($"{nombrePlatillo} - {precioPlatillo}"));
                }
                */

                document.Close();

                await PrintPDF(memoryStream);
            }
        }

        private async Task PrintPDF(MemoryStream memoryStream)
        {
            byte[] pdfData = memoryStream.ToArray();

            var pdfPrinter = DependencyService.Get<IPdfPrinter>();
            if (pdfPrinter != null)
            {
                await pdfPrinter.PrinterPDF(pdfData);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se encontró un servicio de impresión.", "OK");
            }
        }
    }
}