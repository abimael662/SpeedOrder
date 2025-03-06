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

        Page page { get; }

        public async Task Ver()
        {
            await page.Navigation.PushAsync(new V_Inicio());
        }

        public ICommand ComandoVer => new Command<Meseros>(async (m) => await Ver());
    }
}
