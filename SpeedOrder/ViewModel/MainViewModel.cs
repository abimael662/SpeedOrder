using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SpeedOrder.ViewModel
{
    public class Item
    {
        public string Tipo { get; set; }

        public string Imagen
        {
            get
            {
                if (Tipo == "Comidas")
                    return "Comidas.png";
                else if (Tipo == "Desayunos")
                    return "Desayuno.png";
                else if (Tipo == "Cenas")
                    return "Cenas.png";
                else if (Tipo == "Bebidas")
                    return "Bebidas.png";
                else if (Tipo == "Postres")
                    return "Postres.png";
                else
                    return "default.png";
            }
        }
    }
    public class MainViewModel
    {
        public ObservableCollection<Item> Items { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<Item>
        {
            new Item { Tipo = "Comidas" },
            new Item { Tipo = "Desayunos" },
            new Item { Tipo = "Cenas" },
            new Item { Tipo = "Bebidas" },
            new Item { Tipo = "Postres" }
        };
        }
    }
}