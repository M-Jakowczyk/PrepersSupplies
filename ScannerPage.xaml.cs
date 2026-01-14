using ZXing.Net.Maui;

namespace PrepersSupplies
{
    public partial class ScannerPage : ContentPage
    {
        private bool _isTorchOn = false;
        private readonly Action<string> _onBarcodeScanned;

        public ScannerPage(Action<string> onBarcodeScanned)
        {
            InitializeComponent();
            _onBarcodeScanned = onBarcodeScanned;
            
            // Konfiguracja opcji skanera
            barcodeReader.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false // Skanujemy pojedyncze kody
            };

            Console.WriteLine("✅ ScannerPage zainicjalizowana");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            barcodeReader.IsDetecting = true;
            Console.WriteLine("📷 Skaner aktywowany");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            barcodeReader.IsDetecting = false;
            barcodeReader.IsTorchOn = false;
            Console.WriteLine("📷 Skaner dezaktywowany");
        }

        private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            var firstResult = e.Results.FirstOrDefault();
            if (firstResult is null)
            {
                Console.WriteLine("⚠️ Nie wykryto kodu kreskowego");
                return;
            }

            var code = firstResult.Value;
            Console.WriteLine($"📱 Zeskanowano kod: {code}");

            // Zatrzymaj skanowanie
            barcodeReader.IsDetecting = false;

            // Aktualizuj status
            await Dispatcher.DispatchAsync(() =>
            {
                StatusLabel.Text = $"✅ Zeskanowano: {code}";
                StatusLabel.TextColor = Colors.Green;
            });

            // Wibracja jako feedback
            try
            {
                if (Vibration.IsSupported)
                {
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(200));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Błąd wibracji: {ex.Message}");
            }

            // Czekaj chwilę, żeby użytkownik widział komunikat
            await Task.Delay(500);

            // Zamknij okno modalne i wywoła callback na głównym wątku
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Zamknij okno modalne
                await Navigation.PopModalAsync();
                
                // Wywołaj callback z kodem
                _onBarcodeScanned?.Invoke(code);
            });
        }

        private void OnToggleTorch(object sender, EventArgs e)
        {
            _isTorchOn = !_isTorchOn;
            barcodeReader.IsTorchOn = _isTorchOn;
            ToggleTorchBtn.Text = _isTorchOn ? "🔦 Wyłącz latarkę" : "🔦 Włącz latarkę";
            Console.WriteLine($"Latarka {(_isTorchOn ? "włączona" : "wyłączona")}");
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            Console.WriteLine("❌ Anulowano skanowanie");
            await Navigation.PopModalAsync();
        }
    }
}
