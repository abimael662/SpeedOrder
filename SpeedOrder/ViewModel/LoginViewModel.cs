using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpeedOrder.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _correo = "";
        public string Correo
        {
            get => _correo;
            set
            {
                if (_correo != value)
                {
                    _correo = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string nombre = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

        public LoginViewModel() { }
    }
}

/*using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpeedOrder.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public readonly SQLiteAsyncConnection _db;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _mandar = "";
        private string _recibir = "";
        public ICommand MandarCorreo { get; private set; }
        public ICommand RecibirCorreo { get; private set; }
        public string Mandar
        {
            get => _mandar;
            set
            {
                if (_mandar != value)
                {
                    _mandar = value;
                    OnPropertyChanged();
                    Recibir = _mandar;
                }
            }
        }
        public string Recibir
        {
            get => _recibir;
            private set
            {
                if (_recibir != value)
                {
                    _recibir = value;
                    OnPropertyChanged();
                }
            }
        }
        private void OnPropertyChanged([CallerMemberNameAttribute] string nombre = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

        public LoginViewModel()
        {
            MandarCorreo = new Command<string>((key) => Mandar += key);
        }
    }
}*/