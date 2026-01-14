# 📚 Pełny Przewodnik CRUD - Preppers Supplies

## 🎯 Operacje CRUD (Create, Read, Update, Delete)

### **C - CREATE (Tworzenie)**

#### Metoda 1: Skanowanie kodu kreskowego
1. Naciśnij przycisk **"📷 SKANUJ KOD KRESKOWY"**
2. Najedź aparatem na kod kreskowy
3. Po automatycznym zeskanowaniu:
   - Aplikacja pobiera nazwę z API OpenFoodFacts
   - Otwiera się formularz **ProductDetailsPage**
   - Dodaj co najmniej 1 rekord przydatności (datę + ilość)
   - Naciśnij **"✅ Zapisz"**

#### Metoda 2: Ręczne dodawanie produktu
1. W ProductDetailsPage naciśnij **"➕ Dodaj rekord"**
2. Format: `YYYY-MM-DD:Ilość` (np. `2025-01-20:5`)
3. Potwierdź dodanie

#### Format wpisywania daty i ilości:
- Wymagany format: `2025-01-20:5` (data:ilość)
- Data musi być w formacie ISO (YYYY-MM-DD)
- Ilość to liczba całkowita dodatnia
- Jeśli data już istnieje, ilość zostaje dodana do istniejącej

#### CSV Format (jak przechowujemy dane):
```
5900951000996;Mleko;2025-01-20:2,2025-02-10:3
5900951004578;Chleb;2025-01-15:2
```
Schemat: `Barcode;Name;Date1:Qty1,Date2:Qty2,...`

---

### **R - READ (Odczyt)**

#### 1. **Lista wszystkich produktów** 
- Wyświetlana jest automatycznie na MainPage
- Pokazuje: 
  - 📦 Nazwa produktu
  - 📅 Najbliższa data przydatności (czerwono)
  - 📦 Całkowita ilość (zielono)
  - Kod kreskowy

#### 2. **Wyszukiwanie produktu**
- Wpisz w pole **"🔍 Wyszukaj po nazwie lub kodzie..."**
- Wyszukiwanie działa w rzeczywistym czasie
- Szuka po nazwie AND kodzie kreskowym

#### 3. **Filtry gotowe**
- **🔄 Pokaż wszystkie** - Resetuje wyszukiwanie
- **📅 Ważne w 7 dni** - Pokazuje produkty ważne w ciągu 7 dni (posortowane od najwcześniejszego)

#### 4. **Szczegóły produktu**
- Naciśnij **"✏️ Edytuj"** na liście
- Otwiera się ProductDetailsPage ze wszystkimi rekordami przydatności:
  - Data ważności każdego rekordu
  - Ilość dla każdej daty
  - Statystyka: całkowita ilość i najbliższa data

---

### **U - UPDATE (Edycja)**

#### Edycja istniejącego produktu:

1. **Na MainPage:**
   - Naciśnij **"✏️ Edytuj"** na elemencie listy
   - Lub zeskanuj kod produktu, który już istnieje

2. **W ProductDetailsPage:**
   
   **A. Edycja nazwy:**
   - Zmień tekst w polu "📝 Edytuj nazwę:"
   - Zmiany zapamiętane po kliknięciu "✅ Zapisz"

   **B. Modyfikacja rekordów przydatności:**
   
   - **Dodaj nowy rekord:** 
     - Naciśnij **"➕ Dodaj rekord"**
     - Wpisz datę i ilość
     - System automatycznie sortuje po dacie
   
   - **Edytuj ilość dla istniejącej daty:**
     - Usuń stary rekord przyciskiem **"🗑️"**
     - Dodaj nowy z zaktualizowaną ilością
   
   - **Usuń rekord:**
     - Naciśnij **"🗑️"** obok rekordu
     - Potwierdź usunięcie

3. **Validacja:**
   - Co najmniej 1 rekord musi istnieć
   - Całkowita ilość musi być > 0
   - Nazwa nie może być pusta

4. **Zapis:**
   - Naciśnij **"✅ Zapisz"**
   - Zmiany są automatycznie zapisywane do CSV

#### Edycja przez skanowanie duplikatu:
1. Zeskanuj kod produktu, który już istnieje
2. Zostaniesz przesłany do formularza edycji
3. Dokonaj zmian i naciśnij Zapisz

---

### **D - DELETE (Usuwanie)**

#### Metoda 1: Usuwanie z listy produktów
1. Na MainPage naciśnij **"🗑️ Usuń"** obok produktu
2. Potwierdź w oknie dialogu: "Czy na pewno chcesz usunąć..."
3. Produkt jest natychmiast usuwany z listy i CSV

#### Metoda 2: Usuwanie rekordów przydatności
1. Otwórz produkt (naciśnij "✏️ Edytuj")
2. Naciśnij **"🗑️"** obok konkretnego rekordu
3. Potwierdź usunięcie
4. Jeśli zostały rekordy - kontynuuj edycję
5. Jeśli chcesz usunąć cały produkt - naciśnij Anuluj i usuń z listy

---

## 🐛 Rozwiązanie problemu ze scrollowaniem

### Problem:
Lista produktów się nie scrollowała, ponieważ:
- `ScrollView` owijał `CollectionView`
- `CollectionView` już ma wbudowaną wirtualizację
- Zagnieżdżenie tych dwóch elementów powodowało konflikt

### Rozwiązanie:
- **Usunięto** ScrollView z wokół CollectionView
- Zastosowano Grid z `RowDefinitions="Auto,*"` dla całej sekcji
- `CollectionView` jest teraz w wierszu `Grid.Row="1"` z `*` (zajmuje całą dostępną przestrzeń)
- **Efekt:** CollectionView teraz automatycznie obsługuje scrollowanie

### Struktura po naprawie:
```xaml
<Grid Grid.Row="4" Padding="15,0" RowDefinitions="Auto,*">
    <!-- Label z nagłówkiem (Auto) -->
    <Label Grid.Row="0" Text="📋 Zeskanowane produkty:" />
    
    <!-- CollectionView zajmuje całą resztę (*)  -->
    <CollectionView Grid.Row="1" ItemsSource="{Binding ScannedCodes}">
        <!-- Wirtualizacja + scrollowanie wbudowane -->
    </CollectionView>
</Grid>
```

---

## 📊 Schemat przepływu danych

```
SKANOWANIE
    ↓
OnScanButtonClicked → ScannerPage
    ↓
OnBarcodeScanned (MainPage) ← zeskanowany kod
    ↓
Szukanie w _allProducts
    ↓
Nowy produkt?
├─ TAK → GetProductName (API)
│         ↓
│       Dodaj do ScannedCodes + _allProducts
│         ↓
│       ProductDetailsPage (nowy)
│
└─ NIE  → ProductDetailsPage (edycja)
           ↓
       OnSaveClicked
           ↓
       SaveProducts() → CSV
           ↓
       RefreshFilteredList()
```

---

## 🎮 Praktyczne scenariusze

### Scenariusz 1: Dodanie nowego produktu
```
1. Naciśnij "📷 SKANUJ KOD KRESKOWY"
2. Zeskanuj kod: 5900951000996 (Mleko)
3. W ProductDetailsPage:
   - Naciśnij "➕ Dodaj rekord"
   - Wpisz: 2025-01-20:2
   - Naciśnij "✅ Zapisz"
4. Produkt pojawił się na liście!
```

### Scenariusz 2: Aktualizacja istniejącego produktu
```
1. Na MainPage wpisz "Mleko" w wyszukiwarkę
2. Naciśnij "✏️ Edytuj" na produkcie
3. W ProductDetailsPage:
   - Naciśnij "➕ Dodaj rekord"
   - Wpisz: 2025-02-10:3
   - Teraz mamy dwa rekordy
4. Naciśnij "✅ Zapisz"
```

### Scenariusz 3: Filtrowanie ważności
```
1. Naciśnij "📅 Ważne w 7 dni"
2. Widisz tylko produkty ważne w ciągu tygodnia
3. Posortowane od najwcześniejszego
4. Naciśnij "🔄 Pokaż wszystkie" aby wrócić
```

### Scenariusz 4: Usunięcie produktu
```
1. Na MainPage znajdujesz produkt
2. Naciśnij "🗑️ Usuń"
3. Potwierdź w oknie dialogu
4. Produkt jest usunięty z listy i CSV
```

---

## 💾 Dane persistentne

Wszystkie zmiany są automatycznie zapisywane w pliku:
```
C:\Users\[username]\AppData\Local\PrepersSupplies\products.csv
```

Plik jest:
- Wczytywany przy starcie aplikacji
- Aktualizowany po każdej operacji CRUD
- Zawiera pełną historię produktów i ich ważności

---

## ✅ Checklist pełnego CRUD

- [x] **CREATE** - Dodawanie produktów (skanowanie + ręczne)
- [x] **READ** - Odczyt listy, wyszukiwanie, filtry
- [x] **UPDATE** - Edycja produktów, rekordów, nazw
- [x] **DELETE** - Usuwanie produktów i rekordów
- [x] **SEARCH** - Wyszukiwanie po nazwie i kodzie
- [x] **FILTER** - Filtry gotowe (7 dni, wszystkie)
- [x] **VALIDATE** - Validacja danych przy zapisie
- [x] **PERSIST** - Zapis do CSV
- [x] **SORT** - Sortowanie po dacie przydatności
- [x] **SCROLL** - Naprawiony problem ze scrollowaniem

---

## 🎯 Podsumowanie

**Pełny CRUD został implementowany z następującymi funkcjami:**
1. ✅ Łatwe dodawanie produktów przez skanowanie
2. ✅ Edycja wszystkich aspektów produktu
3. ✅ Usuwanie z potwierdzeniem
4. ✅ Zaawansowane wyszukiwanie i filtrowanie
5. ✅ Automatyczne sortowanie po dacie
6. ✅ Persystencja danych
7. ✅ Naprawiać problem ze scrollowaniem
8. ✅ Responsywny UI z emojis i kolorami

**Aplikacja jest gotowa do użytku! 🚀**
