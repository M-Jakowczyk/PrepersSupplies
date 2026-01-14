# 📦 Przepływ aplikacji Preppers Supplies

## Schemat operacji

```
1. SKANOWANIE KODU KRESKOWEGO
   └─> ScannerPage (kamera ZXing)
       └─> Pobranie nazwy produktu z API
           └─> ProductDetailsPage (NOWE - formularz szczegółów)

2. FORMULARZ SZCZEGÓŁÓW PRODUKTU
   └─> Dodawanie rekordów przydatności:
       ├─ Data przydatności (YYYY-MM-DD)
       ├─ Ilość produktu (liczba całkowita)
       └─ Guziki ➕ (dodaj) i 🗑️ (usuń)

3. ZAPIS I WYŚWIETLANIE
   └─> MainPage (lista produktów):
       ├─ Nazwa produktu
       ├─ Najbliższa data przydatności (red)
       ├─ Całkowita ilość (suma wszystkich)
       └─ Kod kreskowy (gray)
```

## Pliki zmienione/utworzone

### ✅ Nowe pliki:
- `ProductDetailsPage.xaml` - Formularz do dodawania dat przydatności i ilości
- `ProductDetailsPage.xaml.cs` - Logika formularza

### ✅ Zmienione pliki:
- `Models/ProductItem.cs` - Dodano `ExpiryRecord` i obsługę kolekcji
- `MainPage.xaml.cs` - Integracja z formularzem
- `MainPage.xaml` - Nowy wygląd listy produktów
- `ScannerPage.xaml.cs` - Już zawiera `MainThread.BeginInvokeOnMainThread()`

## Model danych

### ExpiryRecord
```csharp
public class ExpiryRecord
{
    public DateTime ExpiryDate { get; set; }  // Data przydatności
    public int Quantity { get; set; }         // Ilość
}
```

### ProductItem (zaktualizowany)
```csharp
public class ProductItem
{
    public string Barcode { get; set; }
    public string Name { get; set; }
    public ObservableCollection<ExpiryRecord> ExpiryRecords { get; set; }
    
    // Obliczane automatycznie:
    public DateTime? NearestExpiryDate { get; }  // Najbliższa data
    public int TotalQuantity { get; }             // Suma ilości
    public string DisplayText { get; }            // Formatowana nazwa
}
```

## Format CSV
```
Barcode;ProductName;Date1:Qty1,Date2:Qty2,...
123456789;Mleko;2025-01-20:2,2025-02-10:3
987654321;Chleb;2025-01-15:1
```

## Funkcjonalności

### 📷 Skanowanie
1. Naciśnij "📷 SKANUJ KOD KRESKOWY"
2. Aparat skanuje kod
3. Pobierana jest nazwa z Open Food Facts API
4. ✅ Automatycznie otwiera się formularz

### ➕ Dodawanie szczegółów
1. Wpisz datę przydatności (YYYY-MM-DD)
2. Wpisz ilość
3. Naciśnij "➕ Dodaj datę przydatności" aby dodać więcej
4. Naciśnij "🗑️" aby usunąć rekord
5. Naciśnij "✅ Zapisz produkt"

### 📋 Wyświetlanie listy
- **Nazwa**: Np. "Mleko"
- **📅 Data**: Najbliższa data przydatności (czerwony kolor)
- **📦 Ilość**: Suma wszystkich produktów ze wszystkimi datami (zielony kolor)
- **Kod**: Na szarym tle

## Obsługa błędów
- Brak rekordów: Alert "Dodaj przynajmniej jeden rekord..."
- Wszystkie ilości = 0: Alerty są filtrowane przed zapisem
- Duplikaty: Pozwala edytować istniejący produkt

## Walidacja
- Data przydatności: Format YYYY-MM-DD
- Ilość: Liczba całkowita > 0 (rekordy z 0 są usuwane)
- Minimum jeden rekord przed zapisem
