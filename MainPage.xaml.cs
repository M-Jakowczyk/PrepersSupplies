using PrepersSupplies.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrepersSupplies
{
    public partial class MainPage : ContentPage
    {
        // Używamy ObservableCollection, żeby UI odświeżało się samo
        public ObservableCollection<ProductItem> ScannedCodes { get; set; } = new();
        private readonly HttpClient _httpClient = new(); // Klient HTTP
        private string _filePath = Path.Combine(FileSystem.AppDataDirectory, "products.csv");
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadProducts(); // Wczytujemy dane na starcie
            
            Console.WriteLine("✅ MainPage zainicjalizowana");
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("📷 Otwieranie skanera...");
            
            // Otwórz stronę skanera jako okno modalne z callbackiem
            var scannerPage = new ScannerPage(async (code) => 
            {
                await OnBarcodeScanned(code);
            });
            
            await Navigation.PushModalAsync(scannerPage);
        }

        private async Task OnBarcodeScanned(string code)
        {
            Console.WriteLine($"📱 Otrzymano zeskanowany kod: {code}");

            // Sprawdzamy po kodzie kreskowym w naszej liście obiektów
            var alreadyExists = ScannedCodes.Any(x => x.Barcode == code);

            if (!alreadyExists)
            {
                await Dispatcher.DispatchAsync(async () =>
                {
                    // Sprawdź ponownie (double-check)
                    if (ScannedCodes.Any(x => x.Barcode == code)) 
                    {
                        LastScannedLabel.Text = $"🔄 Już zeskanowany: {code}";
                        LastScannedLabel.TextColor = Colors.Blue;
                        return;
                    }

                    // Wyświetl ostatnio zeskanowany kod
                    LastScannedLabel.Text = $"⏳ Przetwarzam: {code}";
                    LastScannedLabel.TextColor = Colors.Orange;

                    // 1. Dodajemy produkt tymczasowy
                    var newItem = new ProductItem { Barcode = code, Name = "Ładowanie..." };
                    ScannedCodes.Insert(0, newItem); // Dodaj na początek listy
                    Console.WriteLine($"✅ Dodano produkt do listy: {code}");

                    // Zapisujemy od razu (żeby kod nie zginął w razie błędu API)
                    SaveProducts();

                    // 2. Pobieramy nazwę z API
                    string productName = await GetProductName(code);

                    // 3. Aktualizujemy obiekt - INotifyPropertyChanged automatycznie odświeży UI
                    if (!string.IsNullOrEmpty(productName) && productName != "Nieznany produkt")
                    {
                        newItem.Name = productName;
                        Console.WriteLine($"✅ Znaleziono produkt: {productName}");
                        LastScannedLabel.Text = $"✅ Dodano: {productName}";
                        LastScannedLabel.TextColor = Colors.Green;
                        SaveProducts(); // Zapisujemy zaktualizowaną nazwę
                    }
                    else
                    {
                        newItem.Name = "Nieznany produkt";
                        Console.WriteLine($"⚠️ Nie znaleziono produktu dla kodu: {code}");
                        LastScannedLabel.Text = $"⚠️ Nieznany produkt: {code}";
                        LastScannedLabel.TextColor = Colors.Red;
                        SaveProducts();
                    }
                });
            }
            else
            {
                Console.WriteLine($"⚠️ Kod już istnieje w liście: {code}");
                await Dispatcher.DispatchAsync(() =>
                {
                    LastScannedLabel.Text = $"🔄 Już zeskanowany: {code}";
                    LastScannedLabel.TextColor = Colors.Blue;
                });
            }
        }

        private void LoadProducts()
        {
            if (!File.Exists(_filePath)) 
            {
                Console.WriteLine("❌ Plik CSV nie istnieje jeszcze");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(_filePath);
                Console.WriteLine($"📂 Wczytano {lines.Length} linii z pliku");
                
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var item = ProductItem.FromCsvLine(line);
                    if (item != null) ScannedCodes.Add(item);
                }
                
                Console.WriteLine($"✅ Wczytano {ScannedCodes.Count} produktów");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Błąd wczytywania produktów: {ex.Message}");
                LastScannedLabel.Text = $"Błąd wczytywania: {ex.Message}";
                LastScannedLabel.TextColor = Colors.Red;
            }
        }

        private void SaveProducts()
        {
            try
            {
                // Nadpisujemy plik całą listą
                var lines = ScannedCodes.Select(x => x.ToCsvLine());
                File.WriteAllLines(_filePath, lines);
                Console.WriteLine("💾 Produkty zapisane");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Błąd zapisywania produktów: {ex.Message}");
            }
        }

        private async Task<string> GetProductName(string barcode)
        {
            try
            {
                var url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
                Console.WriteLine($"🌐 Zapytanie API: {url}");
                
                var response = await _httpClient.GetStringAsync(url);
                
                var data = JsonSerializer.Deserialize<ProductResponse>(response, _jsonOptions);

                if (data?.Status == 1 && !string.IsNullOrEmpty(data?.Product?.ProductName))
                {
                    Console.WriteLine($"✅ API odpowiedź: {data.Product.ProductName}");
                    return data.Product.ProductName;
                }
                else
                {
                    Console.WriteLine($"⚠️ API: Kod nie znaleziony w bazie (status: {data?.Status})");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Błąd HTTP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"❌ Błąd deserializacji JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Błąd API: {ex.Message}");
            }
            return "Nieznany produkt";
        }
    }
}
