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
            
            // Załadowanie danych przy starcie aplikacji
            LoadData();
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
                PrzypomnieniaPelneListView.ItemsSource = wszystkiePrzypomnienia;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RoslinyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoslina = RoslinyListView.SelectedItem as Roslina;
            
            // Aktywacja/dezaktywacja przycisków akcji
            bool isRoslinaSelected = _selectedRoslina != null;
            SzczegolyButton.IsEnabled = isRoslinaSelected;
            EdytujButton.IsEnabled = isRoslinaSelected;
        }

        private void RoslinyListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_selectedRoslina != null)
            {
                PokazSzczegolyRosliny(_selectedRoslina);
            }
        }
        
        private void OdswiezPrzypomnienia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Wyczyść pamięć podręczną kontekstu
                _context.ChangeTracker.Clear();
        
                // Załaduj wszystkie przypomnienia na nowo
                var wszystkiePrzypomnienia = _context.Przypomnienia
                    .Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana)
                    .ToList();
            
                // Przypisz do listy
                PrzypomnieniaPelneListView.ItemsSource = wszystkiePrzypomnienia;
                
                MessageBox.Show($"Dane zostały odświeżone. Znaleziono {wszystkiePrzypomnienia.Count} przypomnień.", 
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