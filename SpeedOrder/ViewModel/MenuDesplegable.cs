using SpeedOrder.Tables;
using SpeedOrder.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpeedOrder.ViewModel
{
    class MenuDesplegable
    {
        public List<Meseros> Datos { get; set; }

        //Page page { get; }

        /*public async Task Ver(Meseros m)
        {
            await page.Navigation.PushAsync(new V_Tabulador());
        }*/

        private async Task Ver(Meseros m)
        {
            var page = new V_Inicio();
            page.BindingContext = m;
            await page.Navigation.PushAsync(page);
        }

        public ICommand ComandoVer => new Command<Meseros>(async (m) => await Ver(m));
    }
}