using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using SpeedOrder.Droid;
using SpeedOrder.Models;

[assembly: Dependency(typeof(FileService_Android))]
namespace SpeedOrder.Droid
{
    public class FileService_Android : IFileService
    {
        
        public string ObtenerRutaImagen(string nombreArchivo)
        {

            string rutaDestino = Path.Combine(FileSystem.CacheDirectory, nombreArchivo);

            if (!File.Exists(rutaDestino))
            {
                int id = Android.App.Application.Context.Resources.GetIdentifier(Path.GetFileNameWithoutExtension(nombreArchivo),"drawable",Android.App.Application.Context.PackageName);
                //Android.App.Application.Context.Resources.GetDrawable(Path.GetFileName(nombreArchivo,rutaDestino));

                if (id == 0)
                    throw new FileNotFoundException("No se encontró la imagen en drawable", nombreArchivo);

                using (var inputStream = Android.App.Application.Context.Resources.OpenRawResource(id))
                using (var outputStream = File.Create(rutaDestino))
                {   
                    inputStream.CopyTo(outputStream);
                }
            }

            return rutaDestino;
        }
    }
}