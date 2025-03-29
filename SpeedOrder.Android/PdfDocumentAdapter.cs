using Android.Content;
using Android.OS;
using Android.Print;
using Android.Print.Pdf;
using Java.IO;
using System;
using System.IO;

public class PdfDocumentAdapter : PrintDocumentAdapter
{
    private string filePath;
    private Context context;

    public PdfDocumentAdapter(Context context, string filePath)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes,
        CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
    {
        try
        {
            var pdi = new PrintDocumentInfo.Builder("document.pdf")
                .SetContentType(PrintContentType.Document)
                .SetPageCount(PrintDocumentInfo.PageCountUnknown)
                .Build();

            callback.OnLayoutFinished(pdi, true);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error en OnLayout: {ex.Message}");
            callback.OnLayoutFailed(ex.Message);
        }
    }

    public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination,
        CancellationSignal cancellationSignal, WriteResultCallback callback)
    {
        try
        {
            using (var input = new FileInputStream(filePath))
            using (var output = new FileOutputStream(destination.FileDescriptor))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = input.Read(buffer)) != -1)
                {
                    output.Write(buffer, 0, bytesRead);
                }
            }

            callback.OnWriteFinished(new[] { PageRange.AllPages });
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error en OnWrite: {ex.Message}");
            callback.OnWriteFailed(ex.Message);
        }
    }
}