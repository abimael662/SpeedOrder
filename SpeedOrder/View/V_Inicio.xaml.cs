using SpeedOrder.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Inicio : ContentPage
    {
        //public Meseros _m;
        public V_Inicio()
        {
            InitializeComponent();
            /*_m = m;
            datos.Text = "Bienvenid@ " + m.Nombre + " " + m.Ape_paterno + " " + m.Ape_materno + " a Speed Order \n" + "¡Listo para comenzar!";*/
        }
    }
}