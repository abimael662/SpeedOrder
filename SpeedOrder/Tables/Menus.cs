using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedOrder.Tables
{
    public class Menus
    {
        static public List<Menu> menus { get; set; }
        public static List<Menu> Datos()
        {
            menus = new List<Menu>
            {
                new Menu()
                {
                    Id_Menu = 1,
                    Tipo = "Bebidas"
                },
                new Menu()
                {
                    Id_Menu = 2,
                    Tipo = "Desayunos"
                },
                new Menu()
                {
                    Id_Menu = 3,
                    Tipo = "Comidas"
                },
                new Menu()
                {
                    Id_Menu = 4,
                    Tipo = "Cenas"
                },
                new Menu()
                {
                    Id_Menu = 5,
                    Tipo = "Postres"
                }
            };
            return menus;
        }
    }
}
