using SpeedOrder.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Tabulador : TabbedPage
    {
        private int id_Mesero;

        //Meseros _m;
        public V_Tabulador()
        {
            InitializeComponent();
            //InitializeComponent();
            /*  _m = m;
              V_Inicio vInicioPage = new V_Inicio(_m);
              this.Children.Add(vInicioPage);
              this.Children.Add(new V_Menu(_m));
              this.Children.Add(new V_Mesas());*/
        }
        /*
        public V_Tabulador(int id_Mesero)
        {
            this.id_Mesero = id_Mesero;
            ConfigurarPestañas();
        }

        private void ConfigurarPestañas()
        {
            this.Children.Add(new V_Inicio(id_Mesero));
            this.Children.Add(new V_Menu(id_Mesero));
            this.Children.Add(new V_Mesas());
        }*/


        /*
        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new V_Login());
        }*/
    }
}