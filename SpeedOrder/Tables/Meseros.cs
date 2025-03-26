using SQLite;
using System;

namespace SpeedOrder.Tables
{
    public class Meseros
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Mesero { get; set; }
        [MaxLength(20)]
        public string Nombre { get; set; }
        [MaxLength(20)]
        public string Ape_paterno { get; set; }
        [MaxLength(20)]
        public string Ape_materno { get; set; }
        public int Edad { get; set; }
        //public DateTime Edad { get; set; }
        [MaxLength(10)]
        public string Password { get; set; }
        [Unique, MaxLength(50)]
        public string Email { get; set; }
    }

    public class Platillo
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Platillo { get; set; }
        [MaxLength(20)]
        public string Nombre_Platillo { get; set; }
        public Double Precio_Platillo { get; set; }
        //Extras
        public string Photo { get; set; }
        public bool Disponible { get; set; }
    }

    public class Tipo_Menu
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Tipo_Menu { get; set; }
        public int Id_Platillo {  get; set; }
        public int Id_Menu { get; set; }
    }
   
    public class Menu
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Menu { get; set; }
        [MaxLength(15)]
        public string Tipo {  get; set; }
    }

    public class Gestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Gestion { get; set; }
        public int Id_Menu { get; set; }
        public int Id_Mesero { get; set; }
    }

    public class Atender
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Atender { get; set; }
        public int Id_Mesero { get; set; }
        public int Id_Mesa { get; set; }
    }
    public class Mesa
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Mesa { get; set; }
        [MaxLength(20)]
        public string Tamano { get; set; }
        //Prueba
        public string Tipo { get; set; }
    }
    public class Ticket
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Ticket { get; set; }
        public int Id_Mesa { get;set; }
        public int Id_Orden { get;set; }
    }

    public class Orden
    {
        //[PrimaryKey, AutoIncrement]
        public int Id_Orden { get; set; }
        [MaxLength(10)]
        public string Nombre_Cliente { get; set; }
        public DateTime Fecha {  get; set; }
        public Decimal Total {  get; set; }
        public Decimal Subtotal { get; set; }
    }

    public class Platillo_Orden 
    {
        [PrimaryKey, AutoIncrement]
        public int Id_Platillo_Orden { get; set; }
        [AutoIncrement]
        public int Id_Orden { get; set; }
        public int Id_Platillo { get; set; }
        public int Cantidad { get; set; }
    }
    /*
    public class Foto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }*/
}