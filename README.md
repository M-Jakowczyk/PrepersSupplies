# 📦 PrepersSupplies

Aplikacja mobilna do zarządzania zapasami produktów spożywczych i innych artykułów z możliwością śledzenia dat przydatności/ważności.

## 🎯 Główne Funkcje

### ✅ Zarządzanie Produktami
- **Dodawanie produktów** - wpisz nazwę i kod kreskowy
- **Edycja produktów** - zmień nazwę produktu
- **Usuwanie produktów** - usuń całe produkty z bazy
- **Wyszukiwanie** - szybko znajdź produkty w bazie

### 📅 Śledzenie Ważności
- **Rekordy przydatności** - przechowuj wiele dat dla jednego produktu
- **Zwiększanie/zmniejszanie ilości** - zarządzaj liczbą sztuk dla każdej daty
- **Automatyczne obliczanie** - całkowita ilość i najbliższa ważność
- **Sortowanie** - automatycznie widz najbliższe daty ważności

### 📷 Rozpoznawanie Daty OCR
- **Skanowanie etykiet** - fotografuj datę przydatności
- **Automatyczne rozpoznawanie** - OCR wyodrębnia datę z obrazu
- **Zawsze wpisanie ręczne** - jeśli OCR nie zadziała, wpisz datę ręcznie
- **Wieloformatowe** - wspiera różne formaty dat na etykietach

### 📊 Statystyka
- **Obliczanie totali** - całkowita ilość produktu
- **Najbliższa ważność** - czyli najwcześniejsza data przydatności
- **Intuicyjny interfejs** - łatwo zrozumieć stan zapasów

---

## 🚀 Instalacja

### Wymagania
- **.NET 10** lub wyższy
- **Visual Studio 2022** z obsługą .NET MAUI
- **Android SDK** (dla Android) lub **Xcode** (dla iOS)

### Kroki Instalacji

1. **Sklonuj repozytorium**
```bash
git clone https://github.com/M-Jakowczyk/PrepersSupplies.git
cd PrepersSupplies
```

2. **Otwórz w Visual Studio**
```bash
start PrepersSupplies.sln
```

3. **Zbuduj projekt**
```
Build → Build Solution (Ctrl+Shift+B)
```

4. **Uruchom na urządzeniu/emulatorze**
```
Debug → Start Debugging (F5)
```

---

## 📱 Jak Używać Aplikacji

### 1. Dodawanie Nowego Produktu
1. Kliknij przycisk **"➕ Dodaj produkt"** na głównym ekranie
2. Wpisz **nazwę produktu**
3. Wpisz lub zeskanuj **kod kreskowy** (opcjonalnie)
4. Kliknij **"✅ Dodaj"**

### 2. Zarządzanie Rekordami Przydatności

#### Dodawanie Rekordu
1. Kliknij na produkt aby otworzyć **Szczegóły produktu**
2. W sekcji **"📅 Rekordy przydatności"** kliknij **"➕ Dodaj nowy rekord"**
3. Wybierz lub wpisz **datę przydatności**
4. Wpisz **ilość (szt)**
5. Kliknij **"✅ Dodaj rekord"**

#### Zmiana Ilości
- Kliknij **"+"** aby zwiększyć ilość o 1
- Kliknij **"−"** aby zmniejszyć ilość o 1
- Jeśli ilość spadnie do 0, rekord zostanie usunięty

#### Usunięcie Rekordu
- Kliknij **"🗑️"** aby usunąć cały rekord
- Potwierdź usunięcie w oknie dialogowym

### 3. 📷 Skanowanie Daty OCR

#### Uruchomienie Skanowania
1. Otwórz **Szczegóły produktu**
2. W sekcji **"Dodaj nowy rekord"** kliknij przycisk **"📷"** obok daty
3. Wybierz opcję:
   - **"📷 Zrób zdjęcie"** - użyj aparatu telefonu
   - **"🖼️ Wybierz z galerii"** - wybierz istniejące zdjęcie

#### Rezultat Skanowania

**✅ Sukces** - Data została rozpoznana
- Data pojawi się w polu DatePicker
- Możesz ją zmienić ręcznie jeśli OCR popełnił błąd
- Kliknij "✅ Dodaj rekord"

**⚠️ Brak rozpoznania** - Nie znaleziono daty
- Zobaczysz komunikat z tekstu rozpoznanym z obrazu (jeśli jakiś znaleziono)
- **Zawsze możesz wpisać datę ręcznie** w polu DatePicker
- Kliknij "✅ Dodaj rekord"

#### Wspierane Formaty Dat
- `15.03.2025` (dd.MM.yyyy)
- `15-03-2025` (dd-MM-yyyy)
- `15/03/2025` (dd/MM/yyyy)
- `2025-03-15` (yyyy-MM-dd)
- `2025.03.15` (yyyy.MM.dd)
- `2025/03/15` (yyyy/MM/dd)
- `15032025` (ddMMyyyy)
- `20250315` (yyyyMMdd)
- `15.03.25` (dd.MM.yy)

#### OCR rozpoznaje Słowa Kluczowe
- **EXP**, **Best Before**, **Use By**, **BB**
- **Ważne do**, **Data ważności**, **Przydatne do**

### 4. Edycja Produktu
1. Kliknij na produkt aby otworzyć **Szczegóły produktu**
2. W sekcji **"📝 Edytuj nazwę"** zmień nazwę
3. Zarządzaj rekordami przydatności
4. Kliknij **"✅ Zapisz"** aby zapisać zmiany lub **"❌ Anuluj"** aby wyjść

---

## 💡 Wskazówki dla Najlepszych Rezultatów

### 📸 Jakość Zdjęcia
- Upewnij się, że data jest **dobrze oświetlona**
- Trzymaj telefon **stabilnie** (unikaj rozmazanych zdjęć)
- Data powinna być **wyraźna i czytelna**
- Unikaj odbić światła na etykiecie

### 🎯 Kadrowanie
- Zbliż się do daty (zrób zbliżenie)
- Wycentruj datę w kadrze
- Unikaj pochylenia/skręcenia zdjęcia
- Unikaj cieni na dacie

### 🔤 Format Daty
- OCR najlepiej rozpoznaje **standardowe formaty**
- Jeśli data jest wydrukowana nietypową czcionką, może być trudniej
- **W razie problemów - zawsze możesz wpisać datę ręcznie**

---

## 🔧 Technologia

### Stack Technologiczny
- **.NET 10** - framework aplikacji
- **.NET MAUI** - cross-platform UI framework
  - Android
  - Windows

### Główne Biblioteki
| Biblioteka | Wersja | Cel |
|-----------|--------|-----|
| `ZXing.Net.Maui.Controls` | 0.7.4 | Skanowanie kodów kreskowych |
| `Plugin.Maui.OCR` | 1.1.1 | Rozpoznawanie tekstu z obrazów |
| `Microsoft.Maui.Controls` | 10.0.0 | Framework UI |

### Silniki OCR
- **Android**: Google ML Kit Text Recognition (Bundled)

### Architektura
```
PrepersSupplies/
├── Models/
│   └── ProductItem.cs              # Model produktu i rekordu przydatności
├── Services/
│   ├── OcrDateService.cs           # Serwis rozpoznawania daty
│   └── [inne serwisy]
├── Views/
│   ├── MainPage.xaml               # Główny ekran
│   ├── ProductDetailsPage.xaml     # Szczegóły produktu
│   ├── ScannerPage.xaml            # Skanowanie kodów kreskowych
│   └── [inne ekrany]
├── MauiProgram.cs                  # Konfiguracja aplikacji
└── [inne pliki]
```

---

## 📋 Wymagane Uprawnienia

### Android
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.READ_MEDIA_IMAGES" />
```

**Uprawnienia są automatycznie pytane przy pierwszym użyciu funkcji.**

---

## 🐛 Rozwiązywanie Problemów

### OCR nie rozpoznaje daty
1. Spr spróbuj zrobić lepsze zdjęcie (więcej światła, bliżej, wyraźniej)
2. Upewnij się, że data jest w jednym ze wspieranych formatów
3. **Wpisz datę ręcznie** - zawsze jest to możliwe

### Błąd "Brak uprawnień do aparatu"
1. Przejdź do ustawień aplikacji w systemie
2. Włącz uprawnienia do aparatu i galerii zdjęć
3. Restart aplikacji

### Aplikacja się zawiesza podczas skanowania
1. Sprawdź czy masz wystarczająco dużo pamięci na telefonie
2. Spróbuj wybrać mniejsze zdjęcie z galerii
3. Restartuj aplikację

### Kod kreskowy nie zostaje zeskanowany
1. Upewnij się, że kod jest **dobrze oświetlony**
2. Trzymaj telefon **stabilnie**
3. Kod powinien być **wyraźny i czytelny**

---

## ⚠️ Ważne Uwagi

- **Zawsze sprawdź rozpoznaną datę!** OCR nie jest w 100% dokładny.
- **Zawsze możesz wpisać datę ręcznie** - pole DatePicker jest zawsze dostępne.
- **Kopia zapasowa danych** - pamiętaj aby regularnie robić kopię zapasową bazy danych.
- **Uprawnienia** - upewnij się, że aplikacja ma wymagane uprawnienia.

---

## 🤝 Wkład do Projektu

Jeśli chciałbyś wnieść wkład do projektu:

1. Forkuj repozytorium
2. Stwórz gałąź dla swojej funkcji (`git checkout -b feature/AmazingFeature`)
3. Zacommituj zmiany (`git commit -m 'Add some AmazingFeature'`)
4. Pushuj do gałęzi (`git push origin feature/AmazingFeature`)
5. Otwórz Pull Request

---

## 📄 Licencja

Ten projekt jest na licencji **MIT** - zobacz plik `LICENSE` aby uzyskać szczegóły.

---

## 📞 Kontakt

**Autor**: Mateusz Jakowczyk  
**GitHub**: [@M-Jakowczyk](https://github.com/M-Jakowczyk)  
**Repozytorium**: [PrepersSupplies](https://github.com/M-Jakowczyk/PrepersSupplies)

---

## 📝 Zmiana Logu

### v1.0.0 (Aktualna wersja)
- ✅ Zarządzanie produktami (dodawanie, edycja, usuwanie)
- ✅ Rekordy przydatności z zarządzaniem ilością
- ✅ Skanowanie kodów kreskowych
- ✅ **NEW**: Rozpoznawanie daty przydatności za pomocą OCR
- ✅ Statystyka produktów
- ✅ Interfejs w języku polskim

---

## 🎓 Nauka i Zasoby

### Dokumentacja Technologii
- [Microsoft .NET MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [Plugin.Maui.OCR](https://github.com/jfversluis/Plugin.Maui.OCR)
- [ZXing.Net.Maui](https://github.com/Redth/ZXing.Net.MAUI)
- [Google ML Kit](https://developers.google.com/ml-kit)

---

## 🎉 Dziękuję za korzystanie z **PrepersSupplies**!

Jeśli aplikacja Ci się podoba, daj jej ⭐ na GitHub!