using System;
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
        public string ObtenerRutaImagen()
        {
            // Obtén la ID del recurso de la imagen
            int id = Resource.Drawable.MAELDEVS;

            // Verifica si se encontró el recurso
            if (id == 0)
                throw new FileNotFoundException("No se encontró la imagen en drawable");

            // Ruta temporal en el directorio de caché
            string rutaDestino = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "MAELDEVS.png");

            // Copiar la imagen desde los recursos a la ruta de destino
            using (var inputStream = Android.App.Application.Context.Resources.OpenRawResource(id))
            using (var outputStream = new FileStream(rutaDestino, FileMode.Create))
            {
                inputStream.CopyTo(outputStream);
            }

            // Devuelve la ruta donde se encuentra la imagen copiada
            return rutaDestino;
        }
    }
}