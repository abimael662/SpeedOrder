using Rg.Plugins.Popup.Pages;
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
    public partial class V_Mesas : ContentPage
    {
        private double scale = 1;
        public readonly SQLiteAsyncConnection _db;
        private List<Mesa> _mesa = new List<Mesa>();
        public Mesa m;
        public V_Mesas()
        {
            InitializeComponent();
            var rutaBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SpeedOrder.db3");
            _db = new SQLiteAsyncConnection(rutaBD);
            _db.CreateTableAsync<Mesa>().Wait();
        }
        private async void RegisterTable_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_RegisterTables());
            Mesas();
        }
        private async void Remover_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new V_Remover());
        }
        public async void Mesas()
        {
            _mesa = await _db.Table<Mesa>().ToListAsync();

            Canvas.Children.Clear();

            foreach (var mesa in _mesa)
            {
                string mesaName = "Mesa " + mesa.Id_Mesa;

                if (!Canvas.Children.Any(view => view.AutomationId == mesaName))
                {
                    var label = new Label
                    {
                        Text = mesa.Id_Mesa.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 12,
                        FontAttributes = FontAttributes.Bold
                    };

                    var boxView = new Frame
                    {
                        BackgroundColor = mesa.Tipo == "Circular" ? Color.Red : Color.Blue,
                        WidthRequest = mesa.Tamano == "Grande" ? 100 : mesa.Tamano == "Mediana" ? 50 : 25,
                        HeightRequest = mesa.Tamano == "Grande" ? 100 : mesa.Tamano == "Mediana" ? 50 : 25,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        AutomationId = mesaName,
                        Content = label
                    };
                    /*
                    var boxView = new BoxView
                    {
                        Color = mesa.Tipo == "Circular" ? Color.Red : Color.Blue,
                        WidthRequest = 100,
                        HeightRequest = 100,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        AutomationId = mesaName
                    };
                    */
                    if (mesa.Tipo == "Circular")
                        boxView.CornerRadius = 100;
                    else
                        boxView.CornerRadius = 0;

                    Canvas.Children.Add(boxView);

                    var panGesture = new PanGestureRecognizer();
                    panGesture.PanUpdated += (s, args) => OnPanUpdated(s, args);
                    boxView.GestureRecognizers.Add(panGesture);

                    var pinchGesture = new PinchGestureRecognizer();
                    pinchGesture.PinchUpdated += (s, args) => OnPinchUpdated(s, args);
                    boxView.GestureRecognizers.Add(pinchGesture);

                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += async (s, args) =>
                    {
                        await Navigation.PushAsync(new V_Atendido(mesa.Id_Mesa));
                    };
                    boxView.GestureRecognizers.Add(tapGesture);
                }
            }
        }
        private void OnPanUpdated(object sender, PanUpdatedEventArgs args)
        {
            if (sender is Frame boxView)
            {
                switch (args.StatusType)
                {
                    case GestureStatus.Running:
                        double gridWidth = Canvas.Width;
                        double gridHeight = Canvas.Height;
                        double boxWidth = boxView.WidthRequest * boxView.Scale;
                        double boxHeight = boxView.HeightRequest * boxView.Scale;

                        double newX = Math.Max(0, Math.Min(boxView.TranslationX + args.TotalX, gridWidth - boxWidth));
                        double newY = Math.Max(0, Math.Min(boxView.TranslationY + args.TotalY, gridHeight - boxHeight));

                        boxView.TranslationX = newX;
                        boxView.TranslationY = newY;
                        break;
                }
            }
        }
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs args)
        {
            if (sender is Frame boxView)
            {
                if (args.Status == GestureStatus.Running)
                {
                    double newScale = Math.Max(0.5, Math.Min(scale * args.Scale, 2.5));
                    boxView.Scale = newScale;
                }
                else if (args.Status == GestureStatus.Completed)
                {
                    scale = boxView.Scale;
                }
            }
        }
        /*
        protected async override void OnAppearing()
        {
            var mesa = await _db.Table<Mesa>().ToListAsync();
            base.OnAppearing();
        }*/
    }
}