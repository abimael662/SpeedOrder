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
        public V_Tabulador()
        {
            InitializeComponent();
            BindingContext = App.ViewModelGlobal;
        }
    }
}