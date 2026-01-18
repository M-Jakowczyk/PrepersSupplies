# OCR Skanowanie Daty Przydatnoœci

## Funkcjonalnoœæ
Aplikacja umo¿liwia skanowanie i automatyczne rozpoznawanie daty przydatnoœci produktów przy pomocy technologii OCR (Optical Character Recognition).

## Jak u¿ywaæ?

### 1. Otwórz szczegó³y produktu
- PrzejdŸ do strony edycji/dodawania produktu
- ZnajdŸ sekcjê "Dodaj nowy rekord" z dat¹ przydatnoœci

### 2. U¿yj funkcji skanowania
- Kliknij przycisk ?? obok pola daty
- Wybierz jedn¹ z opcji:
  - **?? Zrób zdjêcie** - u¿yj aparatu telefonu aby zrobiæ zdjêcie etykiety produktu
  - **??? Wybierz z galerii** - wybierz istniej¹ce zdjêcie z galerii

### 3. Rezultat skanowania

#### ? Sukces - rozpoznano datê
- Data zostanie automatycznie wpisana w pole DatePicker
- Mo¿esz j¹ zmieniæ rêcznie jeœli OCR pope³ni³ b³¹d
- Kliknij "? Dodaj rekord" aby zapisaæ

#### ?? Nie rozpoznano daty
- Aplikacja poka¿e komunikat z rozpoznanym tekstem (jeœli jakiœ znaleziono)
- **Zawsze mo¿esz wpisaæ datê rêcznie** w pole DatePicker
- Kliknij "? Dodaj rekord" aby zapisaæ

## Wspierane formaty dat

OCR rozpoznaje nastêpuj¹ce formaty:
- `dd.MM.yyyy` (np. 15.03.2025)
- `dd-MM-yyyy` (np. 15-03-2025)
- `dd/MM/yyyy` (np. 15/03/2025)
- `yyyy-MM-dd` (np. 2025-03-15)
- `yyyy.MM.dd` (np. 2025.03.15)
- `yyyy/MM/dd` (np. 2025/03/15)
- `ddMMyyyy` (np. 15032025)
- `yyyyMMdd` (np. 20250315)
- `dd.MM.yy` (np. 15.03.25)

## Rozpoznawanie etykiet
OCR szuka s³ów kluczowych takich jak:
- **EXP** (expiry date)
- **Best Before**
- **Use By**
- **BB** (best before)
- **Wa¿ne do**
- **Data wa¿noœci**
- **Przydatne do**

## Wskazówki dla najlepszych rezultatów

### ?? Jakoœæ zdjêcia
- Upewnij siê, ¿e data jest **dobrze oœwietlona**
- Trzymaj telefon **stabilnie** (unikaj rozmazanych zdjêæ)
- Data powinna byæ **wyraŸna i czytelna**
- Unikaj odbiæ œwiat³a na etykiecie

### ?? Kadrowanie
- Zbli¿ siê do daty (zrób zbli¿enie)
- Wycentruj datê w kadrze
- Unikaj pochylenia/skrêcenia zdjêcia
- Unikaj cieni na dacie

### ?? Format daty
- OCR najlepiej rozpoznaje **standardowe formaty** (dd.MM.yyyy, yyyy-MM-dd)
- Jeœli data jest wydrukowana nietypow¹ czcionk¹, mo¿e byæ trudniej j¹ rozpoznaæ
- W razie problemów - **zawsze mo¿esz wpisaæ datê rêcznie**

## Uprawnienia

### Android
Aplikacja wymaga nastêpuj¹cych uprawnieñ:
- `CAMERA` - dostêp do aparatu
- `READ_EXTERNAL_STORAGE` - odczyt zdjêæ z galerii
- `READ_MEDIA_IMAGES` - odczyt obrazów (Android 13+)

### iOS
Aplikacja wymaga nastêpuj¹cych uprawnieñ:
- `NSCameraUsageDescription` - dostêp do aparatu
- `NSPhotoLibraryUsageDescription` - dostêp do galerii zdjêæ

Uprawnienia s¹ automatycznie pytane przy pierwszym u¿yciu funkcji.

## Technologia

### Biblioteki
- **Plugin.Maui.OCR** (v1.1.1) - rozpoznawanie tekstu z obrazów
- **Google ML Kit** (Android) - silnik OCR
- **Vision Framework** (iOS) - silnik OCR

### Dzia³anie
1. U¿ytkownik robi zdjêcie lub wybiera z galerii
2. Zdjêcie jest konwertowane na byte[]
3. ML Kit / Vision Framework rozpoznaje tekst na obrazie
4. Regex wyodrêbnia datê z rozpoznanego tekstu
5. Data jest walidowana (rok musi byæ w zakresie aktualny +10 lat)
6. Rozpoznana data jest wstawiana do pola DatePicker

## Rozwi¹zywanie problemów

### OCR nie rozpoznaje daty
- Spróbuj zrobiæ lepsze zdjêcie (wiêcej œwiat³a, bli¿ej, wyraŸniej)
- Upewnij siê, ¿e data jest w jednym z wspieranych formatów
- **Wpisz datê rêcznie** w pole DatePicker

### B³¹d "Brak uprawnieñ do aparatu"
- PrzejdŸ do ustawieñ aplikacji w systemie
- W³¹cz uprawnienia do aparatu i galerii zdjêæ

### Aplikacja siê zawiesza podczas skanowania
- SprawdŸ czy masz wystarczaj¹co du¿o pamiêci na telefonie
- Spróbuj wybraæ mniejsze zdjêcie z galerii
- Restartuj aplikacjê

## Uwagi

?? **Zawsze sprawdŸ rozpoznan¹ datê!** OCR nie jest w 100% dok³adny.

?? **Zawsze mo¿esz wpisaæ datê rêcznie** - pole DatePicker jest zawsze dostêpne.
