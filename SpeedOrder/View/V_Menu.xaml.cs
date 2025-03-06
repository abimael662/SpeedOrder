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
    public partial class V_Menu : ContentPage
    {
        //public Meseros _m;
        public V_Menu()
        {
            InitializeComponent();
            //_m = m;
        }

        private void Desayunos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Desayunos());
        }

        private void Comidas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Comidas());
        }

        private void Cenas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Cenas());
        }

        private void Bebidas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Bebidas());
        }

        private void Postres_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Postres());
        }

        private void Todo_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Platillos());
        }

        private void BtnI_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new V_Atendido(_m));
            Navigation.PushAsync(new V_Atendido());
        }

        private void Todos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Platillos());
        }
    }
}