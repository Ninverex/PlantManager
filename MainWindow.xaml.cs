using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;
using MenadzerRoslin.Views;

namespace MenadzerRoslin
{
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private Roslina _selectedRoslina;

        public MainWindow()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
    
            // Dodaj obsługę zdarzenia Loaded, aby upewnić się, że kontrolki są zainicjalizowane
            this.Loaded += MainWindow_Loaded;
    
            // Załadowanie danych przy starcie aplikacji
            LoadData();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Inicjalizacja kontrolek filtrowania
                if (TypZabieguComboBox != null && TypZabieguComboBox.Items.Count > 0)
                {
                    TypZabieguComboBox.SelectedIndex = 0;
                }
        
                if (StatusComboBox != null && StatusComboBox.Items.Count > 0)
                {
                    StatusComboBox.SelectedIndex = 0;
                }
        
                // Odśwież dane po załadowaniu okna
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas inicjalizacji okna: {ex.Message}\n\nSzczegóły: {ex.StackTrace}", 
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LoadData()
        {
            try
            {
                // Wyczyść pamięć podręczną kontekstu
                _context.ChangeTracker.Clear();

                // Załadowanie roślin
                var rosliny = _context.Rosliny.Include(r => r.Gatunek).ToList();
                RoslinyListView.ItemsSource = rosliny;

                // Załadowanie gatunków
                var gatunki = _context.Gatunki.ToList();
                GatunkiListView.ItemsSource = gatunki;
        
                // Załadowanie wszystkich przypomnień
                var wszystkiePrzypomnienia = _context.Przypomnienia
                    .Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana)
                    .ToList();
        
                // Przypisz do listy
                PrzypomnieniaPelneListView.ItemsSource = wszystkiePrzypomnienia;
        
                // Sprawdź, czy kontrolki filtrów zostały już zainicjalizowane
                if (IsLoaded && DataOdPicker != null && DataDoPicker != null && 
                    TypZabieguComboBox != null && StatusComboBox != null)
                {
                    // Sprawdź, czy jakiekolwiek filtry są aktywne
                    bool filtrDataOd = DataOdPicker.SelectedDate.HasValue;
                    bool filtrDataDo = DataDoPicker.SelectedDate.HasValue;
                    bool filtrTypZabiegu = TypZabieguComboBox.SelectedIndex > 0;
                    bool filtrStatus = StatusComboBox.SelectedIndex > 0;
            
                    // Jeśli jakikolwiek filtr jest aktywny, zastosuj filtrowanie
                    if (filtrDataOd || filtrDataDo || filtrTypZabiegu || filtrStatus)
                    {
                        FiltrujPrzypomnienia(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania danych: {ex.Message}", 
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RoslinyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoslina = RoslinyListView.SelectedItem as Roslina;
            
            // Aktywacja/dezaktywacja przycisków akcji
            bool isRoslinaSelected = _selectedRoslina != null;
            SzczegolyButton.IsEnabled = isRoslinaSelected;
            EdytujButton.IsEnabled = isRoslinaSelected;
            UsunButton.IsEnabled = isRoslinaSelected;
        }

        private void RoslinyListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_selectedRoslina != null)
            {
                PokazSzczegolyRosliny(_selectedRoslina);
            }
        }
        
        private void FiltrujPrzypomnienia(object sender, EventArgs e)
{
    try
    {
        // Sprawdź, czy kontrolki są zainicjalizowane
        if (PrzypomnieniaPelneListView == null || _context == null)
        {
            return; // Wyjdź z metody, jeśli kontrolki nie są gotowe
        }

        // Pobierz wszystkie przypomnienia
        var wszystkiePrzypomnienia = _context.Przypomnienia
            .Include(p => p.Roslina)
            .OrderBy(p => p.DataPlanowana)
            .ToList();

        // Zastosuj filtr daty "od"
        if (DataOdPicker != null && DataOdPicker.SelectedDate.HasValue)
        {
            var dataOd = DataOdPicker.SelectedDate.Value.Date;
            wszystkiePrzypomnienia = wszystkiePrzypomnienia
                .Where(p => p.DataPlanowana.Date >= dataOd)
                .ToList();
        }

        // Zastosuj filtr daty "do"
        if (DataDoPicker != null && DataDoPicker.SelectedDate.HasValue)
        {
            var dataDo = DataDoPicker.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1); // Koniec dnia
            wszystkiePrzypomnienia = wszystkiePrzypomnienia
                .Where(p => p.DataPlanowana.Date <= dataDo.Date)
                .ToList();
        }

        // Zastosuj filtr typu zabiegu
        if (TypZabieguComboBox != null && TypZabieguComboBox.SelectedItem != null)
        {
            var selectedItem = TypZabieguComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var typZabiegu = selectedItem.Content as string;
                if (!string.IsNullOrEmpty(typZabiegu) && typZabiegu != "Wszystkie")
                {
                    wszystkiePrzypomnienia = wszystkiePrzypomnienia
                        .Where(p => p.TypZabiegu == typZabiegu)
                        .ToList();
                }
            }
        }

        // Zastosuj filtr statusu
        if (StatusComboBox != null && StatusComboBox.SelectedItem != null)
        {
            var selectedItem = StatusComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var status = selectedItem.Content as string;
                if (!string.IsNullOrEmpty(status))
                {
                    if (status == "Do wykonania")
                    {
                        wszystkiePrzypomnienia = wszystkiePrzypomnienia
                            .Where(p => p.CzyWykonane == false)
                            .ToList();
                    }
                    else if (status == "Wykonane")
                    {
                        wszystkiePrzypomnienia = wszystkiePrzypomnienia
                            .Where(p => p.CzyWykonane == true)
                            .ToList();
                    }
                }
            }
        }

        // Przypisz przefiltrowane dane do ListView
        PrzypomnieniaPelneListView.ItemsSource = wszystkiePrzypomnienia;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Błąd podczas filtrowania przypomnień: {ex.Message}\n\nSzczegóły: {ex.StackTrace}", 
            "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        
        private void UsunRosline_Click(object sender, RoutedEventArgs e)
{
    if (_selectedRoslina == null)
    {
        return;
    }

    // Potwierdzenie usunięcia
    var result = MessageBox.Show(
        $"Czy na pewno chcesz usunąć roślinę '{_selectedRoslina.Nazwa}'?\n\n" +
        "Spowoduje to również usunięcie wszystkich przypomnień i zabiegów związanych z tą rośliną.",
        "Potwierdzenie usunięcia",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning);

    if (result == MessageBoxResult.Yes)
    {
        try
        {
            // Pobierz roślinę z kontekstu
            var roslina = _context.Rosliny
                .Include(r => r.Przypomnienia)
                .Include(r => r.Zabiegi)
                .FirstOrDefault(r => r.RoslinaId == _selectedRoslina.RoslinaId);

            if (roslina == null)
            {
                MessageBox.Show("Nie można znaleźć rośliny w bazie danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Usuń wszystkie przypomnienia związane z rośliną
            if (roslina.Przypomnienia != null && roslina.Przypomnienia.Any())
            {
                _context.Przypomnienia.RemoveRange(roslina.Przypomnienia);
            }

            // Usuń wszystkie zabiegi związane z rośliną
            if (roslina.Zabiegi != null && roslina.Zabiegi.Any())
            {
                _context.Zabiegi.RemoveRange(roslina.Zabiegi);
            }

            // Usuń roślinę
            _context.Rosliny.Remove(roslina);
            _context.SaveChanges();

            // Odśwież dane
            LoadData();

            MessageBox.Show($"Roślina '{roslina.Nazwa}' została usunięta.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas usuwania rośliny: {ex.Message}\n\nSzczegóły: {ex.InnerException?.Message}",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
        
        private void WyczyscFiltry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Sprawdź, czy kontrolki są zainicjalizowane
                if (DataOdPicker == null || DataDoPicker == null || 
                    TypZabieguComboBox == null || StatusComboBox == null || 
                    PrzypomnieniaPelneListView == null)
                {
                    return; // Wyjdź z metody, jeśli kontrolki nie są gotowe
                }

                // Wyczyść filtry
                DataOdPicker.SelectedDate = null;
                DataDoPicker.SelectedDate = null;
        
                // Ustaw indeksy ComboBox na "Wszystkie"
                TypZabieguComboBox.SelectedIndex = 0;
                StatusComboBox.SelectedIndex = 0;

                // Załaduj wszystkie przypomnienia
                var wszystkiePrzypomnienia = _context.Przypomnienia
                    .Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana)
                    .ToList();
        
                PrzypomnieniaPelneListView.ItemsSource = wszystkiePrzypomnienia;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas czyszczenia filtrów: {ex.Message}\n\nSzczegóły: {ex.StackTrace}", 
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void OdswiezPrzypomnienia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Wyczyść pamięć podręczną kontekstu
                _context.ChangeTracker.Clear();
                
                // Zastosuj filtry ponownie
                FiltrujPrzypomnienia(sender, e);
                
                MessageBox.Show("Dane zostały odświeżone.", 
                    "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odświeżania danych: {ex.Message}", 
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PokazSzczegoly_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoslina != null)
            {
                PokazSzczegolyRosliny(_selectedRoslina);
            }
        }

        private void PokazSzczegolyRosliny(Roslina roslina)
        {
            var szczegolyWindow = new SzczegolyRoslinyWindow(_context, roslina);
            szczegolyWindow.Owner = this;
            if (szczegolyWindow.ShowDialog() == true)
            {
                // Odświeżenie danych po edycji
                LoadData();
            }
        }

        private void DodajRosline_Click(object sender, RoutedEventArgs e)
        {
            var dodajRoslineWindow = new DodajRoslineWindow(_context);
            dodajRoslineWindow.Owner = this;
            if (dodajRoslineWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EdytujRosline_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoslina != null)
            {
                var edytujRoslineWindow = new EdytujRoslineWindow(_context, _selectedRoslina);
                edytujRoslineWindow.Owner = this;
                if (edytujRoslineWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }
        private void DodajGatunek_Click(object sender, RoutedEventArgs e)
        {
            var dodajGatunekWindow = new DodajGatunekWindow(_context);
            dodajGatunekWindow.Owner = this;
            if (dodajGatunekWindow.ShowDialog() == true)
            {
                // Odświeżenie listy gatunków
                LoadData();
                MessageBox.Show("Gatunek został dodany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        

private void Przypomnienie_Checked(object sender, RoutedEventArgs e)
{
    var checkBox = sender as CheckBox;
    var przypomnienie = checkBox.DataContext as Przypomnienie;

    if (przypomnienie != null && (checkBox.IsChecked ?? false))
    {
        try
        {
            // Pobierz dane z przypomnienia
            int przypomnienieId = przypomnienie.PrzypomnienieId;
            int roslinaId = przypomnienie.RoslinaId;
            string typZabiegu = przypomnienie.TypZabiegu;
            DateTime dataPlanowana = przypomnienie.DataPlanowana;
            
            // Utwórz nowy kontekst, aby uniknąć problemów z śledzeniem zmian
            using (var newContext = new ApplicationDbContext())
            {
                // Pobierz przypomnienie z nowego kontekstu
                var przypomnienieDb = newContext.Przypomnienia
                    .FirstOrDefault(p => p.PrzypomnienieId == przypomnienieId);
                    
                if (przypomnienieDb == null)
                {
                    MessageBox.Show("Nie można znaleźć przypomnienia w bazie danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    checkBox.IsChecked = false;
                    return;
                }
                
                // Pobierz roślinę z gatunkiem
                var roslina = newContext.Rosliny
                    .Include(r => r.Gatunek)
                    .FirstOrDefault(r => r.RoslinaId == roslinaId);

                if (roslina == null)
                {
                    MessageBox.Show("Nie można znaleźć rośliny dla tego przypomnienia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    checkBox.IsChecked = false;
                    return;
                }
                
                // Oblicz datę następnego zabiegu
                DateTime dataNastepnegoZabiegu;
                if (typZabiegu == "Podlewanie")
                {
                    dataNastepnegoZabiegu = DateTime.SpecifyKind(
                        dataPlanowana.Date.AddDays(roslina.Gatunek.WymagaNawadnianiaCoIleDni), 
                        DateTimeKind.Unspecified);
                }
                else if (typZabiegu == "Nawożenie")
                {
                    dataNastepnegoZabiegu = DateTime.SpecifyKind(
                        dataPlanowana.Date.AddDays(roslina.Gatunek.WymagaNawozeniaCoIleDni), 
                        DateTimeKind.Unspecified);
                }
                else
                {
                    dataNastepnegoZabiegu = DateTime.SpecifyKind(
                        dataPlanowana.Date.AddDays(7), 
                        DateTimeKind.Unspecified);
                }
                
                // Oznacz przypomnienie jako wykonane
                przypomnienieDb.CzyWykonane = true;
                newContext.SaveChanges();
                
                // Dodaj nowy zabieg
                newContext.Zabiegi.Add(new Zabieg
                {
                    RoslinaId = roslinaId,
                    TypZabiegu = typZabiegu,
                    DataWykonania = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
                    Opis = $"Wykonano z przypomnienia z dnia {dataPlanowana.ToString("dd.MM.yyyy")}"
                });
                newContext.SaveChanges();
                
                // Dodaj nowe przypomnienie
                newContext.Przypomnienia.Add(new Przypomnienie
                {
                    RoslinaId = roslinaId,
                    TypZabiegu = typZabiegu,
                    DataPlanowana = dataNastepnegoZabiegu,
                    CzyWykonane = false
                });
                newContext.SaveChanges();
                
                MessageBox.Show($"Dodano nowe przypomnienie o {typZabiegu.ToLower()} na dzień {dataNastepnegoZabiegu.ToString("dd.MM.yyyy")}.", 
                               "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            // Odśwież dane w głównym kontekście
            _context.ChangeTracker.Clear();
            LoadData();
        }
        catch (Exception ex)
        {
            // Przywróć stan checkboxa
            checkBox.IsChecked = false;
            
            MessageBox.Show($"Błąd podczas aktualizacji przypomnienia: {ex.Message}\n\nSzczegóły: {ex.InnerException?.Message}", 
                           "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

        
        


        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}