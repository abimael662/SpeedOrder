using Rg.Plugins.Popup.Services;
using SpeedOrder.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedOrder.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Login : ContentPage
    {
        public V_Login()
        {
            InitializeComponent();
        }
        private async void Btn_Button_Clicked(object sender, EventArgs e)
        {
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            var db = new SQLiteConnection(rutaBD);
            db.CreateTable<Meseros>();
            var email = TxtEmail.Text;
            var contra = TxtPassword.Text;
            IEnumerable<Meseros> resultado = Consulta(db, email, contra);


            if (resultado.Count() > 0)
            {
                var users = (Meseros)resultado.FirstOrDefault();
                //await Navigation.PushAsync(new V_Inicio());
                await Navigation.PushAsync(new V_Tabulador(users));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new V_AlertLogin());
                //await DisplayAlert("Error", "Email o Contraseña Incorrectas", "Ok");
            }
        }
        public static IEnumerable<Meseros> Consulta(SQLiteConnection db, string email, string contra)
        {
            return db.Query<Meseros>("SELECT * FROM Meseros WHERE Email = ? AND Password = ?", email, contra);
        }

        private async void Registrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new V_Register());
        }

        private void TxtEmail_Focused(object sender, FocusEventArgs e)
        {
            LblEmail.TranslationY = 0;
            LblEmail.FontSize = 14;
            LblEmail.TextColor = Color.FromHex("#C44C89");
        }

        private void TxtEmail_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                LblEmail.TranslationY = 20;
                LblEmail.FontSize = 18;
                LblEmail.TextColor = Color.FromHex("#C44C89");
            }
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                LblEmail.TranslationY = 0;
                LblEmail.FontSize = 14;
                LblEmail.TextColor = Color.FromHex("#C44C89");
            }
        }

        private void TxtPassword_Focused(object sender, FocusEventArgs e)
        {
            LblPassword.TranslationY = 0;
            LblPassword.FontSize = 14;
            LblPassword.TextColor = Color.FromHex("#C44C89");
        }

        private void TxtPassword_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                LblPassword.TranslationY = 20;
                LblPassword.FontSize = 18;
                LblPassword.TextColor = Color.FromHex("#C44C89");
            }
        }

        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                LblPassword.TranslationY = 0;
                LblPassword.FontSize = 14;
                LblPassword.TextColor = Color.FromHex("#C44C89");
            }
        }
    }
}