using PrepersSupplies.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrepersSupplies.Pages
{
    public partial class MainPage : ContentPage
    {
        // Używamy ObservableCollection, żeby UI odświeżało się samo
        public ObservableCollection<ProductItem> ScannedCodes { get; set; } = new();
        
        // Przechowujemy wszystkie produkty dla filtrowania
        private List<ProductItem> _allProducts = new();
        
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
            var existingProduct = _allProducts.FirstOrDefault(x => x.Barcode == code);

            if (existingProduct == null)
            {
                await Dispatcher.DispatchAsync(async () =>
                {
                    // Wyświetl ostatnio zeskanowany kod
                    LastScannedLabel.Text = $"⏳ Przetwarzam: {code}";
                    LastScannedLabel.TextColor = Colors.Orange;

                    // 1. Dodajemy produkt tymczasowy
                    var newItem = new ProductItem { Barcode = code, Name = "Ładowanie..." };
                    ScannedCodes.Insert(0, newItem);
                    _allProducts.Insert(0, newItem);
                    Console.WriteLine($"✅ Dodano produkt do listy: {code}");

                    // Zapisujemy od razu
                    SaveProducts();

                    // 2. Pobieramy nazwę z API
                    string productName = await GetProductName(code);

                    // 3. Aktualizujemy obiekt
                    if (!string.IsNullOrEmpty(productName) && productName != "Nieznany produkt")
                    {
                        newItem.Name = productName;
                        Console.WriteLine($"✅ Znaleziono produkt: {productName}");
                        LastScannedLabel.Text = $"✅ {productName}";
                        LastScannedLabel.TextColor = Colors.Green;
                    }
                    else
                    {
                        newItem.Name = "Nieznany produkt";
                        Console.WriteLine($"⚠️ Nie znaleziono produktu dla kodu: {code}");
                        LastScannedLabel.Text = $"⚠️ Nieznany produkt";
                        LastScannedLabel.TextColor = Colors.Red;
                    }

                    SaveProducts();

                    // 4. Otwórz formularz szczegółów produktu
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var detailsPage = new ProductDetailsPage(newItem, (updatedProduct) =>
                        {
                            Console.WriteLine($"💾 Produkt zaktualizowany: {updatedProduct.Name}");
                            SaveProducts();
                            LastScannedLabel.Text = $"✅ Zapisano: {updatedProduct.Name}";
                            LastScannedLabel.TextColor = Colors.Green;
                        });

                        await Navigation.PushModalAsync(detailsPage);
                    });
                });
            }
            else
            {
                Console.WriteLine($"ℹ️ Produkt już istnieje: {existingProduct.Name}");
                await Dispatcher.DispatchAsync(async () =>
                {
                    LastScannedLabel.Text = $"ℹ️ Produkt już istnieje";
                    LastScannedLabel.TextColor = Colors.Blue;

                    // Otwórz formularz aby edytować istniejący produkt
                    var detailsPage = new ProductDetailsPage(existingProduct, (updatedProduct) =>
                    {
                        Console.WriteLine($"💾 Produkt zaktualizowany: {updatedProduct.Name}");
                        SaveProducts();
                        RefreshFilteredList();
                    });

                    await Navigation.PushModalAsync(detailsPage);
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
                    if (item != null)
                    {
                        ScannedCodes.Add(item);
                        _allProducts.Add(item);
                    }
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

        private async void OnEditProductClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ProductItem product)
            {
                Console.WriteLine($"✏️ Otwieranie edycji produktu: {product.Name}");
                
                var detailsPage = new ProductDetailsPage(product, (updatedProduct) =>
                {
                    Console.WriteLine($"💾 Produkt zaktualizowany: {updatedProduct.Name}");
                    SaveProducts();
                    RefreshFilteredList(); // Odświeżamy listę filtrowaną
                });

                await Navigation.PushModalAsync(detailsPage);
            }
        }

        private async void OnDeleteProductClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ProductItem product)
            {
                Console.WriteLine($"🗑️ Usuwanie produktu: {product.Name}");
                
                bool result = await DisplayAlert("Potwierdź usunięcie", 
                    $"Czy na pewno chcesz usunąć produkt \"{product.Name}\"?", 
                    "Usuń", "Anuluj");

                if (result)
                {
                    ScannedCodes.Remove(product);
                    _allProducts.Remove(product);
                    SaveProducts();
                    LastScannedLabel.Text = $"✅ Usunięto: {product.Name}";
                    LastScannedLabel.TextColor = Colors.Green;
                    Console.WriteLine($"✅ Produkt usunięty: {product.Name}");
                }
            }
        }

        // Wyszukiwanie produktów
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilteredList();
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            RefreshFilteredList();
        }

        // Pokaż wszystkie produkty
        private void OnShowAllClicked(object sender, EventArgs e)
        {
            SearchBar.Text = "";
            RefreshFilteredList();
            Console.WriteLine("🔄 Pokazuję wszystkie produkty");
        }

        // Pokaż produkty ważne w ciągu 7 dni
        private void OnShowExpiringSoonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("📅 Filtrowanie: produkty ważne w 7 dni");
            
            var now = DateTime.Now;
            var sevenDaysLater = now.AddDays(7);
            
            ScannedCodes.Clear();
            
            foreach (var product in _allProducts
                .Where(p => p.NearestExpiryDate.HasValue && 
                            //p.NearestExpiryDate >= now && 
                            p.NearestExpiryDate <= sevenDaysLater)
                .OrderBy(p => p.NearestExpiryDate))
            {
                ScannedCodes.Add(product);
            }

            LastScannedLabel.Text = $"📅 Produkty ważne w ciągu 7 dni: {ScannedCodes.Count}";
            LastScannedLabel.TextColor = Colors.Orange;
            Console.WriteLine($"✅ Znaleziono {ScannedCodes.Count} produktów");
        }

        // Odświeżanie listy filtrowanej na podstawie wyszukiwania
        private void RefreshFilteredList()
        {
            var searchText = SearchBar?.Text?.ToLower() ?? "";
            
            ScannedCodes.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                // Pokaż wszystko
                foreach (var product in _allProducts)
                {
                    ScannedCodes.Add(product);
                }
                Console.WriteLine($"🔄 Pokazuję wszystkie {_allProducts.Count} produkty");
            }
            else
            {
                // Filtruj po nazwie lub kodzie
                var filtered = _allProducts
                    .Where(p => p.Name.ToLower().Contains(searchText) || 
                                p.Barcode.ToLower().Contains(searchText))
                    .ToList();

                foreach (var product in filtered)
                {
                    ScannedCodes.Add(product);
                }

                LastScannedLabel.Text = $"🔍 Znaleziono: {filtered.Count} produktów";
                LastScannedLabel.TextColor = Colors.Blue;
                Console.WriteLine($"🔍 Znaleziono {filtered.Count} produktów dla: '{searchText}'");
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
