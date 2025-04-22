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
                // Załadowanie roślin
                _context.Rosliny.Include(r => r.Gatunek).Load();
                RoslinyListView.ItemsSource = _context.Rosliny.Local.ToObservableCollection();

                // Załadowanie gatunków
                _context.Gatunki.Load();
                GatunkiListView.ItemsSource = _context.Gatunki.Local.ToObservableCollection();

                // Załadowanie przypomnień na dziś
                var dzisiaj = DateTime.Today;
                var przypomnieniaDzis = _context.Przypomnienia
                    .Include(p => p.Roslina)
                    .Where(p => p.DataPlanowana.Date <= dzisiaj && !p.CzyWykonane)
                    .ToList();
                PrzypomnieniaDzisListView.ItemsSource = przypomnieniaDzis;

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

        private void DodajZabieg_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoslina != null)
            {
                var dodajZabiegWindow = new DodajZabiegWindow(_context, _selectedRoslina);
                dodajZabiegWindow.Owner = this;
                if (dodajZabiegWindow.ShowDialog() == true)
                {
                    // Dodanie przypomnienia dla następnego zabiegu
                    if (dodajZabiegWindow.DodajPrzypomnienie && dodajZabiegWindow.Zabieg.TypZabiegu == "Podlewanie")
                    {
                        var dataNastepnegoPodlewania = dodajZabiegWindow.Zabieg.DataWykonania.AddDays(_selectedRoslina.Gatunek.WymagaNawadnianiaCoIleDni);
                        var przypomnienie = new Przypomnienie
                        {
                            RoslinaId = _selectedRoslina.RoslinaId,
                            TypZabiegu = "Podlewanie",
                            DataPlanowana = dataNastepnegoPodlewania,
                            CzyWykonane = false
                        };
                        _context.Przypomnienia.Add(przypomnienie);
                        _context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void DodajGatunek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Walidacja danych
                if (string.IsNullOrWhiteSpace(NazwaGatunkuTextBox.Text))
                {
                    MessageBox.Show("Nazwa gatunku jest wymagana.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(PodlewanieTextBox.Text, out int podlewanieDni) || podlewanieDni <= 0)
                {
                    MessageBox.Show("Podaj prawidłową liczbę dni dla podlewania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(NawozenieTextBox.Text, out int nawozenieDni) || nawozenieDni <= 0)
                {
                    MessageBox.Show("Podaj prawidłową liczbę dni dla nawożenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SwiatloComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Wybierz rodzaj światła.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(TempMinTextBox.Text, out double tempMin))
                {
                    MessageBox.Show("Podaj prawidłową minimalną temperaturę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(TempMaxTextBox.Text, out double tempMax))
                {
                    MessageBox.Show("Podaj prawidłową maksymalną temperaturę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (tempMin >= tempMax)
                {
                    MessageBox.Show("Temperatura minimalna musi być mniejsza od maksymalnej.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Utworzenie nowego gatunku
                var gatunek = new Gatunek
                {
                    NazwaGatunku = NazwaGatunkuTextBox.Text,
                    WymagaNawadnianiaCoIleDni = podlewanieDni,
                    WymagaNawozeniaCoIleDni = nawozenieDni,
                    Swiatlo = (SwiatloComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    TemperaturaMin = tempMin,
                    TemperaturaMax = tempMax
                };

                _context.Gatunki.Add(gatunek);
                _context.SaveChanges();

                // Wyczyszczenie pól formularza
                NazwaGatunkuTextBox.Clear();
                PodlewanieTextBox.Clear();
                NawozenieTextBox.Clear();
                SwiatloComboBox.SelectedIndex = -1;
                TempMinTextBox.Clear();
                TempMaxTextBox.Clear();

                // Odświeżenie listy gatunków
                LoadData();

                MessageBox.Show("Gatunek został dodany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania gatunku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Przypomnienie_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var przypomnienie = checkBox.DataContext as Przypomnienie;

            if (przypomnienie != null)
            {
                // Aktualizacja statusu przypomnienia
                przypomnienie.CzyWykonane = checkBox.IsChecked ?? false;
                _context.SaveChanges();

                // Jeśli przypomnienie zostało oznaczone jako wykonane, dodaj nowy zabieg
                if (przypomnienie.CzyWykonane)
                {
                    var zabieg = new Zabieg
                    {
                        RoslinaId = przypomnienie.RoslinaId,
                        TypZabiegu = przypomnienie.TypZabiegu,
                        DataWykonania = DateTime.Now,
                        Opis = "Wykonano z przypomnienia"
                    };
                    _context.Zabiegi.Add(zabieg);

                    // Dodaj nowe przypomnienie na przyszłość
                    if (przypomnienie.TypZabiegu == "Podlewanie")
                    {
                        var roslina = _context.Rosliny.Include(r => r.Gatunek).FirstOrDefault(r => r.RoslinaId == przypomnienie.RoslinaId);
                        if (roslina != null)
                        {
                            var dataNastepnegoPodlewania = DateTime.Now.AddDays(roslina.Gatunek.WymagaNawadnianiaCoIleDni);
                            var nowePrzypomnienie = new Przypomnienie
                            {
                                RoslinaId = przypomnienie.RoslinaId,
                                TypZabiegu = "Podlewanie",
                                DataPlanowana = dataNastepnegoPodlewania,
                                CzyWykonane = false
                            };
                            _context.Przypomnienia.Add(nowePrzypomnienie);
                        }
                    }
                    else if (przypomnienie.TypZabiegu == "Nawożenie")
                    {
                        var roslina = _context.Rosliny.Include(r => r.Gatunek).FirstOrDefault(r => r.RoslinaId == przypomnienie.RoslinaId);
                        if (roslina != null)
                        {
                            var dataNastepnegoNawozenia = DateTime.Now.AddDays(roslina.Gatunek.WymagaNawozeniaCoIleDni);
                            var nowePrzypomnienie = new Przypomnienie
                            {
                                RoslinaId = przypomnienie.RoslinaId,
                                TypZabiegu = "Nawożenie",
                                DataPlanowana = dataNastepnegoNawozenia,
                                CzyWykonane = false
                            };
                            _context.Przypomnienia.Add(nowePrzypomnienie);
                        }
                    }

                    _context.SaveChanges();
                    LoadData();
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