using BarcodeScanner.Mobile;

namespace MauiApp1.src.Presentation.Views
{
    public partial class QrScannerPage : ContentPage
    {
        private readonly TaskCompletionSource<string?> _tcs;
        private bool _yaEscaneado = false;

        public QrScannerPage(TaskCompletionSource<string?> tcs)
        {
            InitializeComponent();
            _tcs = tcs;
        }

        // El evento se dispara en el hilo principal (BarcodeScanner lo marshalea)
        private void CameraView_OnDetected(object sender, OnDetectedEventArg e)
        {
            if (_yaEscaneado) return;

            var resultado = e.BarcodeResults?.FirstOrDefault();
            var valor = resultado?.DisplayValue;
            if (string.IsNullOrEmpty(valor)) return;

            _yaEscaneado = true;

            // Detener el escaneo para no disparar otra detección durante la transición
            cameraView.IsScanning = false;

            _tcs.TrySetResult(valor);
            MainThread.BeginInvokeOnMainThread(async () =>
                await Navigation.PopModalAsync());
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            _tcs.TrySetResult(null);
            await Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cameraView.IsScanning = false;
            _tcs.TrySetResult(null);
        }
    }
}
