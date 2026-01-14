using PrepersSupplies.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrepersSupplies
{
    public partial class ProductDetailsPage : ContentPage
    {
        private ProductItem _product;
        private Action<ProductItem>? _onSave;

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

            _viewModel = new ProductDetailsViewModel(product);
            BindingContext = _viewModel;
            
            Console.WriteLine($"✅ ProductDetailsPage zainicjalizowana dla: {product.Name}");
        }

        // Przycisk zwiększenia ilości
        private void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            _viewModel.NewQuantity++;
            Console.WriteLine($"➕ Ilość zwiększona na: {_viewModel.NewQuantity}");
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

        // Potwierdzenie dodania rekordu
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("✅ Sukces", $"Zaktualizowano rekord\n{expiryDate:yyyy-MM-dd}: {existing.Quantity} szt.", "OK");
                });
            }
            else
            {
                _product.ExpiryRecords.Add(new ExpiryRecord { ExpiryDate = expiryDate, Quantity = quantity });
                Console.WriteLine($"➕ Dodano rekord: {expiryDate:yyyy-MM-dd} - {quantity} szt.");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("✅ Sukces", $"Dodano nowy rekord\n{expiryDate:yyyy-MM-dd}: {quantity} szt.", "OK");
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

                bool confirmed = await DisplayAlert(
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
                await DisplayAlert("Błąd", "Nazwa produktu nie może być pusta", "OK");
                return;
            }

            if (_product.ExpiryRecords.Count == 0)
            {
                await DisplayAlert("Błąd", "Dodaj co najmniej jeden rekord przydatności", "OK");
                return;
            }

            if (_product.TotalQuantity == 0)
            {
                await DisplayAlert("Błąd", "Całkowita ilość musi być większa od zera", "OK");
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
