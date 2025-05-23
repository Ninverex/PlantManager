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

Baza danych do zarządzania roślinami domowymi została zaprojektowana w celu umożliwienia użytkownikowi łatwego śledzenia informacji o swoich roślinach oraz wykonywanych na nich zabiegach pielęgnacyjnych. Składa się z czterech głównych tabel: Rosliny, Gatunki, Przypomnienia oraz Zabiegi, które pozostają ze sobą w logicznych relacjach.

- Tabela Rosliny zawiera informacje o poszczególnych roślinach należących do użytkownika. Dla każdej rośliny przechowywane są takie dane jak jej nazwa, data zakupu oraz lokalizacja w domu, na przykład „salon” lub „parapet w kuchni”. Dodatkowo każda roślina jest przypisana do konkretnego gatunku poprzez pole GatunekId, co pozwala na określenie jej indywidualnych potrzeb pielęgnacyjnych.

- Tabela Gatunki definiuje typy roślin oraz ich wymagania. Dla każdego gatunku przechowywana jest jego nazwa, a także częstotliwość wykonywania dwóch podstawowych zabiegów pielęgnacyjnych: podlewania i nawożenia. Dzięki tym informacjom system może automatycznie planować harmonogram zabiegów dla wszystkich roślin należących do danego gatunku.

- Tabela Przypomnienia służy do planowania i rejestrowania nadchodzących zabiegów pielęgnacyjnych. Każde przypomnienie jest powiązane z konkretną rośliną i określa rodzaj zabiegu, na przykład „Podlewanie” lub „Nawożenie”, planowaną datę jego wykonania oraz informację, czy zabieg został już wykonany. Umożliwia to użytkownikowi wygodne zarządzanie codziennymi obowiązkami związanymi z opieką nad roślinami.

- Tabela Zabiegi przechowuje informacje o zabiegach, które zostały już wykonane. Każdy wpis zawiera powiązanie z konkretną rośliną, typ wykonanego zabiegu, datę jego wykonania oraz ewentualny opis, w którym można dopisać szczegóły, na przykład jakiego środka użyto do nawożenia. Dzięki tej tabeli użytkownik ma dostęp do pełnej historii pielęgnacji każdej rośliny.

- Relacje między tabelami są jasno określone. Każda roślina należy do jednego gatunku, ale może mieć przypisanych wiele przypomnień oraz wiele wykonanych zabiegów. Taka struktura bazy danych pozwala na skuteczne zarządzanie nawet dużą kolekcją roślin i wspiera użytkownika w ich regularnej i odpowiedniej pielęgnacji.


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

## 🧭 Instrukcja obsługi

Po uruchomieniu aplikacji użytkownik ma dostęp do głównego okna, w którym znajdują się listy roślin, gatunków oraz przypomnień. Oto podstawowe funkcje:

### ➕ Dodawanie rośliny
1. Kliknij przycisk `Dodaj Roślinę`.
2. Wypełnij formularz z nazwą, datą zakupu, miejscem, wybierz gatunek oraz opcjonalnie załącz zdjęcie.
3. Zatwierdź, aby dodać roślinę do bazy.  
![image](https://github.com/user-attachments/assets/962d62d5-ecb2-4bbc-b8d8-eae5e0feb9f5)   

### 📝 Edycja rośliny
1. Zaznacz roślinę z listy.
2. Kliknij `Edytuj`.
3. Wprowadź zmiany i zapisz.

### ❌ Usuwanie rośliny
1. Wybierz roślinę z listy.
2. Kliknij `Usuń`, potwierdź operację.
3. Aplikacja automatycznie usunie powiązane przypomnienia i zabiegi.
![image](https://github.com/user-attachments/assets/68461919-b698-4567-ab94-ca5e4e3c930f)


### 🔍 Szczegóły rośliny
- Kliknij podwójnie lub wybierz roślinę i kliknij `Szczegóły`, aby otworzyć okno ze wszystkimi informacjami.  
![image](https://github.com/user-attachments/assets/bda0febb-ca50-48ca-a62d-eed14764fcfd)  

### 🧬 Zarządzanie gatunkami
- Kliknij `Dodaj Gatunek`, aby otworzyć formularz nowego gatunku.  
![image](https://github.com/user-attachments/assets/c35d2ce3-aed1-4618-875c-eabf4051e2e2)  

### ⏰ Przypomnienia
- Lista przypomnień pokazuje nadchodzące zabiegi pielęgnacyjne.
- Zastosuj filtry: daty, typ zabiegu i status (`Do wykonania`, `Wykonane`, `Wszystkie`).
- Kliknij `Odswież`, aby ponownie załadować dane z bazy.
- Zaznacz przypomnienie (`checkbox`), aby oznaczyć jako wykonane — aplikacja automatycznie utworzy nowy wpis w historii zabiegów i przypomnienie na przyszłość.
![image](https://github.com/user-attachments/assets/bf03a00b-6dff-4e10-a834-955ba436d0c3)  

### 🧹 Czyszczenie filtrów
- Kliknij `Wyczyść`, aby usunąć wszystkie warunki filtrowania przypomnień.
![image](https://github.com/user-attachments/assets/27ff6eca-1281-4c95-ae94-61c23f26ded7)


### 💉 Dodawanie zabiegu pielęgnacyjnego

1. Otwórz szczegóły wybranej rośliny lub wybierz opcję `Dodaj zabieg` (jeśli dostępna).
2. W nowym oknie:
   - Wybierz typ zabiegu (np. Podlewanie, Nawożenie, Przycinanie).
   - Wskaż datę jego wykonania (nie może być w przyszłości).
   - Opcjonalnie dodaj opis wykonania.
   - Zaznacz checkbox `Dodaj przypomnienie`, jeśli chcesz, aby aplikacja automatycznie utworzyła przypomnienie na podstawie interwału dla danego gatunku.
3. Kliknij `Dodaj`, aby zatwierdzić.  
![image](https://github.com/user-attachments/assets/3f00a5ac-6a42-4a0b-b60e-03a9eb256e7d)


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






