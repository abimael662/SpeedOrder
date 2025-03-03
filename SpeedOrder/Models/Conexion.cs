using SQLite;

namespace SpeedOrder.Models
{
    public interface ConexionSQL
    {
        SQLiteAsyncConnection GetConnection();
    }
}
