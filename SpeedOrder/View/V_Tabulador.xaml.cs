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
    public partial class V_Tabulador : TabbedPage
    {
        Meseros _m;
        public V_Tabulador(Meseros m)
        {
            InitializeComponent();
            _m = m;
            V_Inicio vInicioPage = new V_Inicio(_m);
            this.Children.Add(vInicioPage);
            this.Children.Add(new V_Menu(_m));
            this.Children.Add(new V_Mesas());
        }
        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new V_Login());
        }
    }
}