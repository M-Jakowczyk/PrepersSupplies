using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace PrepersSupplies.Models
{
    // Rekord przydatności (data i ilość)
    public class ExpiryRecord : INotifyPropertyChanged
    {
        private DateTime _expiryDate;
        private int _quantity;

        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                if (_expiryDate != value)
                {
                    _expiryDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ExpiryDateString
        {
            get => ExpiryDate.ToString("yyyy-MM-dd");
            set
            {
                if (DateTime.TryParse(value, out var date))
                {
                    ExpiryDate = date;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProductItem : INotifyPropertyChanged
    {
        private string _barcode = "";
        private string _name = "";

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        // Kolekcja rekordów przydatności
        public ObservableCollection<ExpiryRecord> ExpiryRecords { get; set; } = new();

        // Najbliższa data przydatności
        public DateTime? NearestExpiryDate
        {
            get
            {
                if (ExpiryRecords.Count == 0) return null;
                return ExpiryRecords.MinBy(x => x.ExpiryDate)?.ExpiryDate;
            }
        }

        // Całkowita ilość wszystkich produktów
        public int TotalQuantity
        {
            get => ExpiryRecords.Sum(x => x.Quantity);
        }

        // Wyświetlanie na liście: Nazwa | Najbliższa data | Ilość
        public string DisplayText
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return $"Szukam... ({Barcode})";

                var nearestDate = NearestExpiryDate;
                var dateStr = nearestDate?.ToString("yyyy-MM-dd") ?? "Brak";
                var total = TotalQuantity;

                return $"{Name} | {dateStr} | Ilość: {total}";
            }
        }

        // CSV format: kod;nazwa;data1:ilość1,data2:ilość2,...
        public string ToCsvLine()
        {
            var expiryPart = string.Join("," , ExpiryRecords.OrderBy(x => x.ExpiryDate).Select(x => $"{x.ExpiryDate:yyyy-MM-dd}:{x.Quantity}"));

            return $"{Barcode};{Name};{expiryPart}";
        }

        // Metoda do tworzenia obiektu z linii CSV
        public static ProductItem? FromCsvLine(string line)
        {
            var parts = line.Split(';');
            if (parts.Length < 2) return null;

            var item = new ProductItem { Barcode = parts[0], Name = parts[1] };

            // Wczytaj rekordy przydatności jeśli istnieją
            if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
            {
                var expiryParts = parts[2].Split(',');
                foreach (var expiryPart in expiryParts)
                {
                    var expiryBits = expiryPart.Split(':');
                    if (expiryBits.Length == 2 &&
                        DateTime.TryParse(expiryBits[0], out var date) &&
                        int.TryParse(expiryBits[1], out var qty))
                    {
                        item.ExpiryRecords.Add(new ExpiryRecord { ExpiryDate = date, Quantity = qty });
                    }
                }
            }

            return item;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
