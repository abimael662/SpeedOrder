using Android.Content;
using Android.Print;
using System.IO;
using Xamarin.Forms;
using Android.App;
using Android.PrintServices;
using SpeedOrder.Droid;
using SpeedOrder.Models;

[assembly: Dependency(typeof(PrintService_Android))]
namespace SpeedOrder.Droid
{
    public class PrintService_Android : IPrintService
    {
        public void PrintPDF(byte[] pdfData)
        {
            // Obtener el contexto de la actividad actual
            var activity = MainActivity.Instance;

            if (activity == null)
                return;

            var printManager = (PrintManager)activity.GetSystemService(Context.PrintService);

            string fileName = "temp_print.pdf";
            string filePath = Path.Combine(activity.CacheDir.AbsolutePath, fileName);

            // Guardar el PDF en un archivo temporal
            File.WriteAllBytes(filePath, pdfData);

            // Crear un PrintDocumentAdapter
            PrintDocumentAdapter pda = new PdfDocumentAdapter(activity, filePath);

            // Iniciar la impresión
            printManager.Print("Documento PDF", pda, null);
        }
    }
}
