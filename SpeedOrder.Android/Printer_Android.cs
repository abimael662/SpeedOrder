using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using SpeedOrder.Droid;
using SpeedOrder.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(Printer_Android))]

namespace SpeedOrder.Droid
{
    public class Printer_Android : IPdfPrinter
    {
        public async Task PrinterPDF(byte[] pdfData)
        {
            try
            {
                // Obtener el contexto de la Activity activa
                var context = Xamarin.Essentials.Platform.CurrentActivity;

                if (context == null)
                {
                    throw new Exception("No se pudo obtener el contexto de la actividad.");
                }

                // Obtiene el servicio de impresión
                var printManager = (PrintManager)context.GetSystemService(Context.PrintService);
                var printAdapter = new PdfDocumentAdapter(context, pdfData);

                // Inicia la impresión
                printManager.Print("Ticket", printAdapter, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al imprimir: {ex.Message}");
            }
        }
    }
}