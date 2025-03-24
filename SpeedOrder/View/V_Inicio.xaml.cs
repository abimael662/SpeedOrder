using SpeedOrder.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SpeedOrder.ViewModel.LoginViewModel;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Inicio : ContentPage
    {
        private int id_Mesero;

        //public Meseros _m;
        /*
        public V_Inicio(Meseros m)
        {
            InitializeComponent();
            //_m = m;
            datos.Text = "Bienvenid@ " + m.Nombre + " " + m.Ape_paterno + " " + m.Ape_materno + " a Speed Order \n" +
                "¡Listo para comenzar!";
        }*/
        public V_Inicio()
        {
            InitializeComponent();
            /*var mesero = BindingContext as Meseros;

            if (mesero != null)
            {
                datos.Text = $"Bienvenid@ {mesero.Nombre} {mesero.Ape_paterno} {mesero.Ape_materno} a Speed Order \n" +
                             "¡Listo para comenzar!";
            }
            else
            {
                datos.Text = "Bienvenid@ a Speed Order \n" +
                             "¡Listo para comenzar!";
            }*/
        }
    }
}