using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_IceCreamFlyout : ContentPage
    {
        public ListView ListView;

        public V_IceCreamFlyout()
        {
            InitializeComponent();

            BindingContext = new V_IceCreamFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class V_IceCreamFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<V_IceCreamFlyoutMenuItem> MenuItems { get; set; }
            
            public V_IceCreamFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<V_IceCreamFlyoutMenuItem>(new[]
                {
                    new V_IceCreamFlyoutMenuItem { Id = 0, Title = "Inicio", Icono = "House", TargetType = typeof(V_Tabulador) },
                    new V_IceCreamFlyoutMenuItem { Id = 1, Title = "Ajustes", Icono = "Setting", TargetType = typeof(V_Ajustes) },
                    new V_IceCreamFlyoutMenuItem { Id = 2, Title = "Logout", Icono = "Exit", TargetType = typeof(V_Login) }
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}