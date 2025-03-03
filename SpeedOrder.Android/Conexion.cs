using Android.Runtime;
using SQLite;
using SpeedOrder.Models;
using System.IO;
using Xamarin.Forms;
using SpeedOrder.Droid;

[assembly: Dependency(typeof(Conexion))]

namespace SpeedOrder.Droid
{
    public class Conexion : ConexionSQL
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "SpeedOrder.db3");
            var db = new SQLiteAsyncConnection(path);

            return db;
        }
    }
}