# Menadżer Roślin 🌿
Menadżer Roślin to aplikacja desktopowa WPF służąca do zarządzania kolekcją roślin, ich gatunkami oraz przypomnieniami o pielęgnacji (np. podlewaniu, nawożeniu). Projekt oparty jest o Entity Framework i umożliwia pełną kontrolę nad bazą danych lokalnych roślin.

### Wymagania
- .NET 9 
- Visual Studio 2022 lub inny kompatybilny IDE
- PostgreSQL z narzędziem `psql` (do importu bazy danych)

## Funkcje  
- 📋 Lista roślin i gatunków – przeglądaj, dodawaj i edytuj rośliny oraz gatunki.

- 🔔 Przypomnienia – system przypomnień o koniecznych zabiegach pielęgnacyjnych (np. podlewanie, nawożenie).

- ✅ Obsługa wykonanych zabiegów – oznacz przypomnienie jako wykonane, a aplikacja automatycznie doda wpis do historii zabiegów i utworzy nowe przypomnienie na przyszłość.

- 🔄 Odświeżanie danych – aktualizuj listy roślin, gatunków i przypomnień bez ponownego uruchamiania aplikacji.

- 👁️ Szczegóły rośliny – podgląd i edycja szczegółowych informacji o wybranej roślinie.

- 🧪 Obsługa wyjątków i komunikatów użytkownika – aplikacja reaguje na błędy i informuje użytkownika o sukcesie operacji.

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
dotnet ef database update
```

- Jeśli z jakiegoś powodu wolisz użyć pliku `menadzerroslindatabase.sql`, możesz zaimportować go ręcznie do PostgreSQL:

  1. Upewnij się, że masz zainstalowany PostgreSQL oraz narzędzia `psql`.
  2. Utwórz bazę danych:
     ```bash
     createdb menadzerroslin
     ```
  3. Zaimportuj skrypt:
     ```bash
     psql -d menadzerroslin -f menadzerroslindatabase.sql
     ```
  4. W pliku `appsettings.json` zmodyfikuj connection string tak, aby wskazywał na tę bazę, np.:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=menadzerroslin;Username=twoj_uzytkownik;Password=twoje_haslo"
     }
     ```

### 5. Uruchom aplikację
Wybierz projekt `MenadzerRoslin` jako startowy i kliknij **Start** lub naciśnij **F5**.

---

## Uwagi
- Projekt jest aplikacją WPF, więc po uruchomieniu powinno pojawić się okno aplikacji.
- Jeśli pojawią się błędy, sprawdź poprawność przywrócenia pakietów NuGet i konfiguracji bazy danych.


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






