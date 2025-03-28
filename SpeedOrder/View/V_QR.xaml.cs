using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.IO;
using Xamarin.Forms;
using QRCoder;
using System;

namespace SpeedOrder.View
{
    public partial class V_QR : PopupPage
    {
        public V_QR(string qrData)
        {
            InitializeComponent();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            var imageSource = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            QRImage.Source = imageSource;
        }

        private async void Cerrar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}