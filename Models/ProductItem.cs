using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrepersSupplies.Models
{
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

        // To zobaczymy na liście w aplikacji
        public string DisplayText => string.IsNullOrEmpty(Name) ? $"Szukam... ({Barcode})" : $"{Name} ({Barcode})";

        // To zapiszemy do pliku (format: kod;nazwa)
        public string ToCsvLine() => $"{Barcode};{Name}";

        // Metoda do tworzenia obiektu z linii CSV
        public static ProductItem? FromCsvLine(string line)
        {
            var parts = line.Split(';');
            if (parts.Length < 2) return null;
            return new ProductItem { Barcode = parts[0], Name = parts[1] };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
