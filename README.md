# 📘 Dokumentacja aplikacji **Menadżer Roślin**

## 🌱 Opis aplikacji

**Menadżer Roślin** to aplikacja desktopowa stworzona z wykorzystaniem technologii WPF oraz Entity Framework Core. Jej celem jest wspieranie użytkownika w zarządzaniu kolekcją roślin domowych lub ogrodowych poprzez:

- Dodawanie, edytowanie i usuwanie roślin oraz gatunków.
- Tworzenie przypomnień o potrzebnych zabiegach pielęgnacyjnych (np. podlewanie, nawożenie).
- Przechowywanie historii wykonanych zabiegów.
- Filtrowanie przypomnień według daty, typu zabiegu oraz statusu wykonania.
- Automatyczne generowanie kolejnych przypomnień po wykonaniu zabiegu.

Aplikacja wspiera również interaktywne elementy UI jak listy, okna dialogowe i przyciski, umożliwiające sprawne zarządzanie danymi.

## 🗃️ Opis bazy danych

Baza danych została zaprojektowana z myślą o prostocie i wydajności. Składa się z następujących głównych tabel:

### `Rosliny`
- **RoslinaId** (PK): Identyfikator rośliny.
- **Nazwa**: Nazwa rośliny.
- **DataZakupu**: Data zakupu rośliny.
- **Miejsce**: Lokalizacja rośliny.
- **GatunekId** (FK): Powiązanie z tabelą `Gatunki`.

### `Gatunki`
- **GatunekId** (PK): Identyfikator gatunku.
- **Nazwa**: Nazwa gatunku.
- **WymagaNawadnianiaCoIleDni**: Częstotliwość podlewania.
- **WymagaNawozeniaCoIleDni**: Częstotliwość nawożenia.

### `Przypomnienia`
- **PrzypomnienieId** (PK): Identyfikator przypomnienia.
- **RoslinaId** (FK): Powiązanie z rośliną.
- **TypZabiegu**: Rodzaj zabiegu (np. Podlewanie, Nawożenie).
- **DataPlanowana**: Planowana data wykonania.
- **CzyWykonane**: Status wykonania (bool).

### `Zabiegi`
- **ZabiegId** (PK): Identyfikator zabiegu.
- **RoslinaId** (FK): Powiązanie z rośliną.
- **TypZabiegu**: Typ wykonanego zabiegu.
- **DataWykonania**: Data wykonania zabiegu.
- **Opis**: Dodatkowy opis.

Relacje:
- Każda **Roślina** może posiadać wiele **Przypomnień** i **Zabiegów**.
- Każda **Roślina** należy do jednego **Gatunku**.

![image](https://github.com/user-attachments/assets/1c0b8445-9baa-4ac9-8b75-78f2dac90fe9)  


## Instrukcja uruchomienia

### 1. Sklonuj repozytorium
```bash
git clone <adres-repozytorium>
cd MenadzerRoslin
```

### 2. Otwórz projekt
Otwórz plik `MenadzerRoslin.sln` w Visual Studio.

### 3. Przywróć zależności
W Visual Studio pakiety NuGet zostaną pobrane automatycznie.  
Jeśli nie, użyj w terminalu:
```bash
dotnet restore
```

### 4. Skonfiguruj bazę danych

- Projekt korzysta z migracji Entity Framework Core, więc najpierw należy utworzyć i zaktualizować bazę danych za pomocą migracji:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- w celu dodania przykładowych danych należy użyc pliku `dane.sql`, możesz zaimportować go ręcznie do PostgreSQL:

  1. Upewnij się, że masz zainstalowany PostgreSQL oraz narzędzia `psql`.
  2. Wykonaj poniższe polecenie, podmieniając odpowiednie dane:

  ```bash
  psql -U nazwa_uzytkownika -d nazwa_bazy -f dane.sql

### 5. Uruchom aplikację
Wybierz projekt `MenadzerRoslin` jako startowy i kliknij **Start** lub naciśnij **F5**.

---

## Uwagi
- Projekt jest aplikacją WPF, więc po uruchomieniu powinno pojawić się okno aplikacji.
- Jeśli pojawią się błędy, sprawdź poprawność przywrócenia pakietów NuGet i konfiguracji bazy danych.

## 🧭 Instrukcja obsługi

Po uruchomieniu aplikacji użytkownik ma dostęp do głównego okna, w którym znajdują się listy roślin, gatunków oraz przypomnień. Oto podstawowe funkcje:

### ➕ Dodawanie rośliny
1. Kliknij przycisk `Dodaj Roślinę`.
2. Wypełnij formularz z nazwą, datą zakupu, miejscem, wybierz gatunek oraz opcjonalnie załącz zdjęcie.
3. Zatwierdź, aby dodać roślinę do bazy.

### 📝 Edycja rośliny
1. Zaznacz roślinę z listy.
2. Kliknij `Edytuj`.
3. Wprowadź zmiany i zapisz.

### ❌ Usuwanie rośliny
1. Wybierz roślinę z listy.
2. Kliknij `Usuń`, potwierdź operację.
3. Aplikacja automatycznie usunie powiązane przypomnienia i zabiegi.

### 🔍 Szczegóły rośliny
- Kliknij podwójnie lub wybierz roślinę i kliknij `Szczegóły`, aby otworzyć okno ze wszystkimi informacjami.

### 🧬 Zarządzanie gatunkami
- Kliknij `Dodaj Gatunek`, aby otworzyć formularz nowego gatunku.

### ⏰ Przypomnienia
- Lista przypomnień pokazuje nadchodzące zabiegi pielęgnacyjne.
- Zastosuj filtry: daty, typ zabiegu i status (`Do wykonania`, `Wykonane`, `Wszystkie`).
- Kliknij `Odswież`, aby ponownie załadować dane z bazy.
- Zaznacz przypomnienie (`checkbox`), aby oznaczyć jako wykonane — aplikacja automatycznie utworzy nowy wpis w historii zabiegów i przypomnienie na przyszłość.

### 🧹 Czyszczenie filtrów
- Kliknij `Wyczyść filtry`, aby usunąć wszystkie warunki filtrowania przypomnień.


### 💉 Dodawanie zabiegu pielęgnacyjnego

1. Otwórz szczegóły wybranej rośliny lub wybierz opcję `Dodaj zabieg` (jeśli dostępna).
2. W nowym oknie:
   - Wybierz typ zabiegu (np. Podlewanie, Nawożenie, Przycinanie).
   - Wskaż datę jego wykonania (nie może być w przyszłości).
   - Opcjonalnie dodaj opis wykonania.
   - Zaznacz checkbox `Dodaj przypomnienie`, jeśli chcesz, aby aplikacja automatycznie utworzyła przypomnienie na podstawie interwału dla danego gatunku.
3. Kliknij `Dodaj`, aby zatwierdzić.

> Jeśli zaznaczono opcję przypomnienia, aplikacja automatycznie wyliczy datę kolejnego zabiegu na podstawie interwałów zdefiniowanych dla danego gatunku (np. co ile dni należy podlewać lub nawozić roślinę) i doda przypomnienie.
---

## Widoki:
- widok główny:  
![image](https://github.com/user-attachments/assets/7c33c1dd-70d5-4d74-a7d2-6c055de9a7d5)  
![image](https://github.com/user-attachments/assets/bf03a00b-6dff-4e10-a834-955ba436d0c3)  
![image](https://github.com/user-attachments/assets/411ec32e-f4bf-4530-8884-6a21b5d68f51)  
- dodawanie gatunku:    
![image](https://github.com/user-attachments/assets/c35d2ce3-aed1-4618-875c-eabf4051e2e2)  
- dodawanie rośliny:  
![image](https://github.com/user-attachments/assets/962d62d5-ecb2-4bbc-b8d8-eae5e0feb9f5)  
- szczegóły rośliny:  
![image](https://github.com/user-attachments/assets/bda0febb-ca50-48ca-a62d-eed14764fcfd)    
- dodawanie zabiegu:  
![image](https://github.com/user-attachments/assets/53773d31-91ac-4270-b1f4-2c25b67d1387)  






