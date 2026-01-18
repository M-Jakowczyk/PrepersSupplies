using PrepersSupplies.Models;
using PrepersSupplies.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrepersSupplies
{
    public partial class ProductDetailsPage : ContentPage
    {
        private ProductItem _product;
        private Action<ProductItem>? _onSave;
        private OcrDateService _ocrDateService;

        // ViewModel dla bindowania
        public class ProductDetailsViewModel : INotifyPropertyChanged
        {
            private DateTime _newExpiryDate;
            private int _newQuantity = 1;

            public ProductItem Product { get; set; }

            public DateTime NewExpiryDate
            {
                get => _newExpiryDate;
                set
                {
                    if (_newExpiryDate != value)
                    {
                        _newExpiryDate = value;
                        OnPropertyChanged();
                    }
                }
            }

            public int NewQuantity
            {
                get => _newQuantity;
                set
                {
                    if (_newQuantity != value && value > 0)
                    {
                        _newQuantity = value;
                        OnPropertyChanged();
                    }
                }
            }

            public ProductDetailsViewModel(ProductItem product)
            {
                Product = product;
                // Domyślnie ustawiamy datę na dzisiaj
                _newExpiryDate = DateTime.Now.AddDays(7); // domyślnie +7 dni
                _newQuantity = 1;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ProductDetailsViewModel _viewModel;

        public ProductDetailsPage(ProductItem product, Action<ProductItem>? onSave = null)
        {
            InitializeComponent();
            _product = product;
            _onSave = onSave;
            _ocrDateService = new OcrDateService();

            _viewModel = new ProductDetailsViewModel(product);
            BindingContext = _viewModel;
            
            Console.WriteLine($"✅ ProductDetailsPage zainicjalizowana dla: {product.Name}");
        }

        // Przycisk zwiększenia ilości
        private void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            _viewModel.NewQuantity++;
            Console.WriteLine($"➕ Ilość zwięksona na: {_viewModel.NewQuantity}");
        }

        // Przycisk zmniejszenia ilości
        private void OnDecreaseQuantityClicked(object sender, EventArgs e)
        {
            if (_viewModel.NewQuantity > 1)
            {
                _viewModel.NewQuantity--;
                Console.WriteLine($"➖ Ilość zmniejszona na: {_viewModel.NewQuantity}");
            }
        }

        // Potwierdzenie dodania rekordu przydatności
        private void OnConfirmAddExpiryRecordClicked(object sender, EventArgs e)
        {
            Console.WriteLine("✅ Dodawanie nowego rekordu przydatności");

            var expiryDate = _viewModel.NewExpiryDate;
            var quantity = _viewModel.NewQuantity;

            // Sprawdzam czy data już istnieje (opcjonalnie mogę zezwolić duplikaty)
            var existing = _product.ExpiryRecords.FirstOrDefault(x => x.ExpiryDate.Date == expiryDate.Date);
            if (existing != null)
            {
                existing.Quantity += quantity;
                Console.WriteLine($"📝 Zaktualizowano rekord na {expiryDate:yyyy-MM-dd}, nowa ilość: {existing.Quantity}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlertAsync("✅ Sukces", $"Zaktualizowano rekord\n{expiryDate:yyyy-MM-dd}: {existing.Quantity} szt.", "OK");
                });
            }
            else
            {
                _product.ExpiryRecords.Add(new ExpiryRecord { ExpiryDate = expiryDate, Quantity = quantity });
                Console.WriteLine($"➕ Dodano rekord: {expiryDate:yyyy-MM-dd} - {quantity} szt.");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlertAsync("✅ Sukces", $"Dodano nowy rekord\n{expiryDate:yyyy-MM-dd}: {quantity} szt.", "OK");
                });
            }

            // Reset na następny rekord
            _viewModel.NewExpiryDate = DateTime.Now.AddDays(7);
            _viewModel.NewQuantity = 1;
        }

        // Usunięcie rekordu przydatności
        private async void OnDeleteExpiryRecordClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ExpiryRecord record)
            {
                Console.WriteLine($"🗑️ Usuwanie rekordu: {record.ExpiryDate:yyyy-MM-dd}");

                bool confirmed = await DisplayAlertAsync(
                    "Potwierdź usunięcie",
                    $"Czy chcesz usunąć rekord na {record.ExpiryDate:yyyy-MM-dd} ({record.Quantity} szt.)?",
                    "Usuń",
                    "Anuluj"
                );

                if (confirmed)
                {
                    _product.ExpiryRecords.Remove(record);
                    Console.WriteLine($"✅ Usunięto rekord: {record.ExpiryDate:yyyy-MM-dd}");
                }
            }
        }

        // Zmniejszenie ilości w rekordzie przydatności
        private async void OnDecreaseRecordQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ExpiryRecord record)
            {
                Console.WriteLine($"➖ Zmniejszanie ilości w rekordzie: {record.ExpiryDate:yyyy-MM-dd}");

                if (record.Quantity > 1)
                {
                    record.Quantity--;
                    Console.WriteLine($"✅ Nowa ilość: {record.Quantity}");
                }
                else
                {
                    // Jeśli ilość spadnie do 0, zapytaj czy usunąć rekord
                    bool confirmed = await DisplayAlertAsync(
                        "Usunąć rekord?",
                        $"Ilość spadnie do 0. Czy usunąć rekord na {record.ExpiryDate:yyyy-MM-dd}?",
                        "Usuń",
                        "Anuluj"
                    );

                    if (confirmed)
                    {
                        _product.ExpiryRecords.Remove(record);
                        Console.WriteLine($"✅ Usunięto rekord: {record.ExpiryDate:yyyy-MM-dd}");
                    }
                }
            }
        }

        // Zwiększenie ilości w rekordzie przydatności
        private void OnIncreaseRecordQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ExpiryRecord record)
            {
                Console.WriteLine($"➕ Zwiększanie ilości w rekordzie: {record.ExpiryDate:yyyy-MM-dd}");
                record.Quantity++;
                Console.WriteLine($"✅ Nowa ilość: {record.Quantity}");
            }
        }

        // Skanowanie daty przydatności przy pomocy OCR
        private async void OnScanDateClicked(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("📷 Rozpoczynam skanowanie daty...");

                // Sprawdź uprawnienia do aparatu
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                    if (cameraStatus != PermissionStatus.Granted)
                    {
                        await DisplayAlert("Błąd", "Brak uprawnień do aparatu", "OK");
                        return;
                    }
                }

                // Pokaż opcje: zrób zdjęcie lub wybierz z galerii
                var action = await DisplayActionSheet(
                    "Skanuj datę przydatności",
                    "Anuluj",
                    null,
                    "📷 Zrób zdjęcie",
                    "🖼️ Wybierz z galerii"
                );

                if (action == "Anuluj" || action == null)
                    return;

                FileResult? photo = null;

                if (action == "📷 Zrób zdjęcie")
                {
                    // Zrób zdjęcie
                    photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = "Zrób zdjęcie daty przydatności"
                    });
                }
                else if (action == "🖼️ Wybierz z galerii")
                {
                    // Wybierz zdjęcie z galerii
                    photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                    {
                        Title = "Wybierz zdjęcie daty przydatności"
                    });
                }

                if (photo == null)
                {
                    Console.WriteLine("❌ Nie wybrano zdjęcia");
                    return;
                }

                // Skopiuj zdjęcie do katalogu tymczasowego
                var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                Console.WriteLine($"✅ Zdjęcie zapisane: {newFile}");

                // Pokaż wskaźnik ładowania
                var loadingTask = DisplayAlert("Przetwarzanie", "Rozpoznawanie daty z zdjęcia...", "OK");

                // Rozpoznaj datę z OCR
                var (success, date, rawText) = await _ocrDateService.RecognizeDateFromImageAsync(newFile);

                // Zamknij wskaźnik ładowania
                try { await loadingTask; } catch { }

                if (success && date.HasValue)
                {
                    // Ustaw rozpoznaną datę
                    _viewModel.NewExpiryDate = date.Value;
                    
                    await DisplayAlert(
                        "✅ Sukces",
                        $"Rozpoznano datę: {date.Value:yyyy-MM-dd}\n\nMożesz ją zmienić ręcznie jeśli jest niepoprawna.",
                        "OK"
                    );
                    
                    Console.WriteLine($"✅ Ustawiono datę: {date.Value:yyyy-MM-dd}");
                }
                else
                {
                    // Nie udało się rozpoznać daty
                    var message = string.IsNullOrWhiteSpace(rawText)
                        ? "Nie udało się rozpoznać tekstu na zdjęciu."
                        : $"Nie znaleziono daty w rozpoznanym tekście:\n\n{rawText.Substring(0, Math.Min(200, rawText.Length))}...";

                    await DisplayAlert(
                        "⚠️ Nie rozpoznano daty",
                        $"{message}\n\nWpisz datę ręcznie.",
                        "OK"
                    );
                    
                    Console.WriteLine("⚠️ Nie rozpoznano daty");
                }

                // Usuń tymczasowy plik
                try { File.Delete(newFile); } catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Błąd podczas skanowania: {ex.Message}");
                await DisplayAlert(
                    "❌ Błąd",
                    $"Wystąpił błąd podczas skanowania:\n{ex.Message}\n\nWpisz datę ręcznie.",
                    "OK"
                );
            }
        }

        // Anulowanie edycji
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            Console.WriteLine("❌ Anulowanie edycji");
            await Navigation.PopModalAsync();
        }

        // Zapis zmian
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            Console.WriteLine($"✅ Zapisuję produkt: {_product.Name}");

            // Validacja
            if (string.IsNullOrWhiteSpace(_product.Name))
            {
                await DisplayAlertAsync("Błąd", "Nazwa produktu nie może być pusta", "OK");
                return;
            }

            if (_product.ExpiryRecords.Count == 0)
            {
                await DisplayAlertAsync("Błąd", "Dodaj co najmniej jeden rekord przydatności", "OK");
                return;
            }

            if (_product.TotalQuantity == 0)
            {
                await DisplayAlertAsync("Błąd", "Całkowita ilość musi być większa od zera", "OK");
                return;
            }

            // Callback do rodzica
            _onSave?.Invoke(_product);

            // Powrót
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }
    }
}

