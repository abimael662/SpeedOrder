using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class V_AlertLogin : PopupPage
	{
		public V_AlertLogin ()
		{
			InitializeComponent ();
		}
        private async void BtnCerrar_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(); // Cierra el popup y nos regresa a la ventana que lo abrio
        }
    }
}