# 🧪 Instrukcja testowania nowej funkcjonalności

## Test 1: Skanowanie i dodawanie produktu

### Kroki:
1. Uruchom aplikację
2. Naciśnij **"📷 SKANUJ KOD KRESKOWY"**
3. Skanuj kod kreskowy (lub wprowadź ręcznie):
   - Mleko: `5900951000996`
   - Chleb: `5900951004578`
4. **Automatycznie** otworzy się formularz `ProductDetailsPage`

### Oczekiwany wynik:
```
✅ Producent/Nazwa produktu jest wczytane z API
✅ Formularz pokazuje szablon z jednym pustym rekordem
✅ Data domyślnie to +1 miesiąc od dziś
✅ Ilość domyślnie to 1
```

## Test 2: Dodawanie multiple rekordów przydatności

### Kroki:
1. W otwartym formularzu:
   - Wpisz datę: `2025-01-20`
   - Wpisz ilość: `2`
   - Naciśnij **"➕ Dodaj datę przydatności"**
   - Wpisz datę: `2025-02-10`
   - Wpisz ilość: `3`
   - Naciśnij **"✅ Zapisz produkt"**

### Oczekiwany wynik:
```
✅ Formularz pokazuje dwa rekordy (stacków)
✅ Każdy rekord ma swoje pola daty i ilości
✅ Przycisk 🗑️ pojawia się obok każdego rekordu
✅ Na głównej liście widać:
   - Nazwa produktu
   - 📅 Przydatny do: 2025-01-20 (najbliższa data)
   - 📦 Ilość: 5 (suma 2+3)
```

## Test 3: Edycja istniejącego produktu

### Kroki:
1. Skanuj ten sam kod po raz drugi
2. **Zamiast duplikatu**, formularz otworzy się dla istniejącego produktu
3. Dodaj kolejny rekord
4. Naciśnij "✅ Zapisz produkt"

### Oczekiwany wynik:
```
✅ Liczba rekordów wzrosła
✅ TotalQuantity się przeliczył
✅ NearestExpiryDate się zaktualizował
✅ Dane w pliku CSV się zaktualizowały
```

## Test 4: Usuwanie rekordów

### Kroki:
1. W formularzu naciśnij **🗑️** przy jeden z rekordów
2. Naciśnij **"✅ Zapisz produkt"**

### Oczekiwany wynik:
```
✅ Rekord został usunięty z listy
✅ TotalQuantity się zmniejszył
✅ Jeśli była to najbliższa data, NearestExpiryDate się zmienił
```

## Test 5: Walidacja formularza

### Test 5a: Brak rekordów
- Otwórz formularz
- Usuń wszystkie rekordy (🗑️)
- Naciśnij "✅ Zapisz produkt"
- **Oczekiwany wynik**: Alert "Dodaj przynajmniej jeden rekord z ilością > 0"

### Test 5b: Ilość = 0
- Otwórz formularz
- Ustaw ilość na 0
- Naciśnij "✅ Zapisz produkt"
- **Oczekiwany wynik**: Alert "Dodaj przynajmniej jeden rekord z ilością > 0"

### Test 5c: Zła data
- Wpisz datę: `invalid-date`
- Naciśnij "✅ Zapisz produkt"
- **Oczekiwany wynik**: Data pozostaje stara (walidacja przy Input)

## Test 6: Anulowanie

### Kroki:
1. Otwórz formularz
2. Dodaj rekordy
3. Naciśnij **"❌ Anuluj"**

### Oczekiwany wynik:
```
✅ Formularz się zamyka
✅ Zmiany NIE są zapisywane
✅ Powracasz do głównej listy
```

## Test 7: Wczytywanie z pliku CSV

### Kroki:
1. Zamknij aplikację całkowicie
2. Usuń/przesuń plik `products.csv` (jeśli istnieje)
3. Uruchom aplikację
4. Skanuj kilka produktów i dodaj ich szczegóły
5. Zamknij aplikację
6. Otwórz ponownie

### Oczekiwany wynik:
```
✅ Wszystkie produkty się wczytały
✅ Wszystkie rekordy przydatności się wczytały
✅ Display pokazuje prawidłowe daty i ilości
```

## Test 8: Wygląd listy produktów

### Oczekiwany wygląd:
```
📦 Preppers Supplies
Menedżer zapasów spożywczych

[📷 SKANUJ KOD KRESKOWY]

[Status ostatniego skanowania]

📋 Zeskanowane produkty:

┌─────────────────────────┐
│ Mleko                   │
│ 📅 Przydatny do: 2025-01-20
│ 📦 Ilość: 5             │
│ Kod: 5900951000996      │
└─────────────────────────┘

┌─────────────────────────┐
│ Chleb                   │
│ 📅 Przydatny do: 2025-01-15
│ 📦 Ilość: 2             │
│ Kod: 5900951004578      │
└─────────────────────────┘
```

## Debugging

### Sprawdzanie logów:
```
✅ Produkty zaktualizowany: {Name}
💾 Produkty zapisane
✅ Dodawanie nowego rekordu przydatności
🗑️ Usuwanie rekordu: {date}
```

### Sprawdzanie pliku CSV:
Plik znajduje się w: `FileSystem.AppDataDirectory/products.csv`

**Zawartość powinno wyglądać:**
```
5900951000996;Mleko;2025-01-20:2,2025-02-10:3
5900951004578;Chleb;2025-01-15:2
```

### Jeśli coś nie działa:
1. Sprawdź console logs (Debug -> Output window)
2. Sprawdź czy plik CSV istnieje i ma prawidłowy format
3. Upewnij się że daty są w formacie YYYY-MM-DD
4. Sprawdź czy ilości są > 0 w CSV
