# 📖 Instrukcja - Jak dodawać rekordy przydatności (NOWY INTERFEJS)

## 🎯 Szybki start

Dodawanie rekordu przydatności jest teraz **intuicyjne i szybkie** - bez wpisywania formatu!

---

## 📝 Krok po kroku

### Krok 1: Otwórz formularz produktu
```
1. Na liście produktów naciśnij [✏️ Edytuj]
2. LUB zeskanuj kod produktu, który już istnieje
3. Otwiera się: ProductDetailsPage
```

### Krok 2: Znajdź sekcję "Dodaj nowy rekord"
```
Znajduje się ona w dolnej części strony
┌──────────────────────────────┐
│ ➕ Dodaj nowy rekord        │
│                              │
│ 📅 Data przydatności        │
│ [▼ 2025-01-27]  ← Tu!      │
│                              │
│ 📦 Ilość (szt)              │
│ [2] [−] [2] [+] ← I tu!    │
│                              │
│ [✅ Dodaj rekord]           │
└──────────────────────────────┘
```

### Krok 3: Wybierz datę
```
1. Kliknij na pole daty [▼ 2025-01-27]
2. Otwiera się system KALENDARZ
3. Nawiguj do żądanego miesiąca/roku
4. Kliknij na dzień
5. Data się automatycznie aktualizuje
```

**Wskazówka:** Domyślnie ustawiona jest data +7 dni od dzisiaj. Jeśli chcesz tę datę - nic nie musisz robić!

### Krok 4: Ustaw ilość
Masz 3 opcje:

#### Opcja A: Przyciski +/-
```
Początkowa ilość: 1

Kliknij [+] trzy razy:
1 → 2 → 3 → 4 (4 szt.)

Kliknij [−] dwa razy:
4 → 3 → 2 (2 szt.)
```

#### Opcja B: Ręczne wpisanie
```
1. Kliknij w pole [2]
2. Wymaż istniejącą liczbę
3. Wpisz nową: np. 10
4. Kliknie poza polem lub Enter
```

#### Opcja C: Kombinacja
```
- Kliknij + kilka razy → 1, 2, 3
- Kliknij pole i wpisz 15 → 15
- Kliknij − aby zmniejszyć → 14
```

### Krok 5: Sprawdź podgląd
```
┌────────────────────┐
│ Podgląd:           │
│ 📅 2025-01-27     │
│ 📦 10 szt.        │
└────────────────────┘

To dokładnie to, co zostanie dodane!
```

### Krok 6: Dodaj rekord
```
Kliknij [✅ Dodaj rekord]

Otwiera się alert:
✅ Sukces
"Dodano nowy rekord
2025-01-27: 10 szt."

Kliknij [OK]
```

### Krok 7: Powtórz lub zapisz
```
Po dodaniu rekordu:
- Ilość resetuje się na 1
- Data resetuje się na +7 dni
- Sekcja jest gotowa do następnego rekordu

LUB

Jeśli skończyłeś dodawać:
1. Scroll do góry
2. Kliknij [✅ Zapisz] na dole
3. Produkty zostają zapisane w CSV
```

---

## 💡 Praktyczne przykłady

### Przykład 1: Mleko (ważne za 7 dni)
```
1. Produktu: "Mleko"
2. Kliknij [▼] → Data już +7 dni ✅
3. Ilość: Kliknij + dwukrotnie → 3 szt.
4. Podgląd: 📅 2025-02-03, 📦 3 szt.
5. Kliknij [✅ Dodaj rekord]
```
⏱️ Czas: ~5 sekund

### Przykład 2: Chleb (ważny dzisiaj)
```
1. Produkt: "Chleb"
2. Kliknij [▼] → Otwiera się kalendarz
3. Nawiguj do dzisiaj (albo cofnij 7 dni)
4. Kliknij dzisiaj
5. Ilość: Kliknij w pole, wpisz 2
6. Podgląd: 📅 2025-01-27, 📦 2 szt.
7. Kliknij [✅ Dodaj rekord]
```
⏱️ Czas: ~10 sekund

### Przykład 3: Cukier (ważny za 6 miesięcy)
```
1. Produkt: "Cukier"
2. Kliknij [▼] → Otwiera się kalendarz
3. Nawiguj do czerwca 2025
4. Kliknij dzień (np. 27)
5. Ilość: Wpisz 1 (worek)
6. Podgląd: 📅 2025-06-27, 📦 1 szt.
7. Kliknij [✅ Dodaj rekord]
```
⏱️ Czas: ~10 sekund

### Przykład 4: Wiele parcji tego samego produktu
```
Chcemy dodać mleko z różnymi datami:
- Partia 1: 2025-01-27, 2 szt.
- Partia 2: 2025-02-10, 5 szt.

Krok 1: Dodaj pierwszą partię (2025-01-27, 2 szt.)
        [✅ Dodaj rekord]

Krok 2: System resetuje na +7 dni (2025-02-03)
        Zmień na 2025-02-10
        Ustaw ilość na 5
        [✅ Dodaj rekord]

Krok 3: Obie partie są w liście:
        📅 2025-01-27: 2 szt.
        📅 2025-02-10: 5 szt.
```

---

## ⚠️ Ważne uwagi

### Minimalna ilość
```
Nie możesz ustawić ilość < 1
Przycisk [−] nie zadziała, jeśli ilość = 1
```

### Tylko liczby całkowite
```
✅ MOŻNA: 1, 2, 5, 100
❌ NIEGO: 1.5, 2.7, 0.5

Jeśli wpiszesz 1.5, będzie zaokrąglone do 2
```

### Jeśli data już istnieje
```
Scenariusz: Miałeś mleko na 2025-01-27 z ilością 2

Dodajesz: Jeszcze 3 szt. na tę samą datę

Wynik: Automatycznie zaktualizuje się na 5 szt.
Alert: "Zaktualizowano rekord
        2025-01-27: 5 szt."
```

### Kolejność rekordów
```
System automatycznie sortuje rekordy po dacie!

Dodajesz w porządku:
1. 2025-02-10: 5 szt.
2. 2025-01-27: 2 szt.
3. 2025-03-15: 1 szt.

Wyświetli się:
1. 2025-01-27: 2 szt. (najwcześniej)
2. 2025-02-10: 5 szt.
3. 2025-03-15: 1 szt. (najpóźniej)
```

---

## 🆘 Co jeśli coś pójdzie nie tak?

### Problem: Nie mogę zmienić daty
```
Rozwiązanie:
1. Kliknij na [▼] obok daty
2. Powinien się otworzyć kalendarz
3. Jeśli się nie otwiera - spróbuj jeszcze raz
4. Jeśli dalej nie działa - powiedz debugger'owi
```

### Problem: Przycisk + nie zwiększa ilości
```
Rozwiązanie:
1. Sprawdź czy pole Entry ma fokus (niebieskie obramowanie)
2. Kliknij [+] ponownie
3. Jeśli dalej nie działa - odśwież aplikację
```

### Problem: Nie mogę wpisać liczby w pole ilości
```
Rozwiązanie:
1. Kliknij dokładnie w środek pola [2]
2. Całkowicie wymaż liczbę
3. Wpisz nową liczbę
4. Kliknij gdzieś poza polem (aby potwierdzić)
```

### Problem: Alert mówi "Błąd"
```
Możliwe przyczyny:
- Brak wybranej daty → Wybierz datę w [▼]
- Ilość = 0 lub ujemna → Ustaw ilość > 0
- Brak nazwy produktu → Sprawdź nazwę na górze

Rozwiązanie: Sprawdź wymóg i spróbuj ponownie
```

---

## ✅ Checklist dodawania rekordu

- [ ] Otwierasz ProductDetailsPage
- [ ] Widzisz sekcję "➕ Dodaj nowy rekord"
- [ ] Klikasz na datepicker [▼]
- [ ] Wybierasz datę z kalendarza
- [ ] Ustawiasz ilość (przyciskami lub wpisując)
- [ ] Sprawdzasz podgląd
- [ ] Klikasz [✅ Dodaj rekord]
- [ ] Widzisz alert "✅ Sukces"
- [ ] Klikasz [OK]
- [ ] Ilość i data resetują się na domyślne

Jeśli wszystko ✅ - gratulacje! Rekord został dodany! 🎉

---

## 📊 Porównanie: Stary vs Nowy sposób

| Aspekt | STARY | NOWY |
|--------|-------|------|
| Sposób | Wpisywanie "YYYY-MM-DD:Ilość" | Graficzne pola |
| Czas | ~20 sekund | ~7 sekund |
| Łatwość | Trudne | Bardzo łatwe |
| Błędy | Możliwe | Niemożliwe |
| Potwierdzenie | Brak | Jest alert |

**NOWY sposób jest 3x szybszy i bezpieczniejszy!** ✨

---

## 🎯 Kluczowe punkty do zapamiętania

1. 📅 **DatePicker** - Klik na ▼, wybierz z kalendarza
2. 📦 **Ilość** - Przyciski +/- lub bezpośrednio w polu
3. 👀 **Podgląd** - Zawsze sprawdź podgląd
4. ✅ **Dodaj** - Kliknij niebieski przycisk
5. 🔄 **Reset** - Po dodaniu wszystko resetuje się

---

## 💬 Feedback i sugestie

Jeśli nowy interfejs podoba Ci się - super! 🎉

Jeśli masz uwagi:
- Coś jest niezrozumiałe?
- Chciałbyś innej funkcjonalności?
- Znalazłeś błąd?

**Powiedz o tym!** Twój feedback pomaga w rozwijaniu aplikacji.

---

**Już gotowy? Przejdź do dodawania rekordów! 🚀**
