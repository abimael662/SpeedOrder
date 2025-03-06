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
    public partial class V_MenuDesplegableFlyout : ContentPage
    {
        public ListView ListView;

        public V_MenuDesplegableFlyout()
        {
            InitializeComponent();

            BindingContext = new V_MenuDesplegableFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class V_MenuDesplegableFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<V_MenuDesplegableFlyoutMenuItem> MenuItems { get; set; }

            public V_MenuDesplegableFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<V_MenuDesplegableFlyoutMenuItem>(new[]
                {
                    new V_MenuDesplegableFlyoutMenuItem { Id = 0, Title = "Inicio", Icono = "Inicio", TargetType = typeof(V_Tabulador) },
                    new V_MenuDesplegableFlyoutMenuItem { Id = 1, Title = "Configuración", Icono = "Settings", TargetType = typeof(V_Ajustes)},
                    new V_MenuDesplegableFlyoutMenuItem { Id = 2, Title = "Salir", Icono = "Exit" }
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