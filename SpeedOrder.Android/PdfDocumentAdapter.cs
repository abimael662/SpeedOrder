using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Java.IO;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedOrder.Droid
{
    public class PdfDocumentAdapter : PrintDocumentAdapter
    {
        private readonly byte[] _pdfData;

        public PdfDocumentAdapter(Context context, byte[] pdfData)
        {
            _pdfData = pdfData;
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            try
            {
                // Abre el archivo de destino para escribir el PDF
                var outputStream = new FileOutputStream(destination.FileDescriptor);
                outputStream.Write(_pdfData, 0, _pdfData.Length);
                outputStream.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error al escribir el PDF: {ex.Message}");
            }
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            // Define la información básica de la página para la impresión
            var builder = new PrintDocumentInfo.Builder("Ticket")
                .SetContentType(PrintContentType.Document)
                .SetPageCount(PrintDocumentInfo.PageCountUnknown)
                .Build();
            callback.OnLayoutFinished(builder, true);
        }
    }
}
