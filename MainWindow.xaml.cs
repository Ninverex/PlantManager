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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TypZabieguComboBox.SelectedIndex = 0;
                StatusComboBox.SelectedIndex = 0;
                LoadData();
            }
            catch (Exception ex)
            {
                ShowError("inicjalizacji okna", ex);
            }
        }

        private void LoadData()
        {
            try
            {
                _context.ChangeTracker.Clear();
                RoslinyListView.ItemsSource = _context.Rosliny.Include(r => r.Gatunek).ToList();
                GatunkiListView.ItemsSource = _context.Gatunki.ToList();

                var przypomnienia = _context.Przypomnienia.Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana).ToList();
                PrzypomnieniaPelneListView.ItemsSource = przypomnienia;

                if (IsLoaded && CzyFiltryAktywne())
                    FiltrujPrzypomnienia(null, null);
            }
            catch (Exception ex)
            {
                ShowError("ładowania danych", ex);
            }
        }

        private bool CzyFiltryAktywne() =>
            DataOdPicker.SelectedDate.HasValue ||
            DataDoPicker.SelectedDate.HasValue ||
            TypZabieguComboBox.SelectedIndex > 0 ||
            StatusComboBox.SelectedIndex > 0;

        private void RoslinyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoslina = RoslinyListView.SelectedItem as Roslina;
            bool selected = _selectedRoslina != null;
            SzczegolyButton.IsEnabled = selected;
            EdytujButton.IsEnabled = selected;
            UsunButton.IsEnabled = selected;
        }

        private void RoslinyListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_selectedRoslina != null)
                PokazSzczegolyRosliny(_selectedRoslina);
        }

        private void FiltrujPrzypomnienia(object sender, EventArgs e)
        {
            try
            {
                if (_context == null || PrzypomnieniaPelneListView == null)
                    return;

                var przypomnienia = _context.Przypomnienia
                    .Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana)
                    .ToList();

                DateTime? dataOd = DataOdPicker?.SelectedDate;
                DateTime? dataDo = DataDoPicker?.SelectedDate;

                string typZabiegu = null;
                if (TypZabieguComboBox?.SelectedItem is ComboBoxItem typItem)
                    typZabiegu = typItem.Content as string;

                string status = null;
                if (StatusComboBox?.SelectedItem is ComboBoxItem statusItem)
                    status = statusItem.Content as string;

                przypomnienia = przypomnienia.Where(p =>
                    (!dataOd.HasValue || p.DataPlanowana.Date >= dataOd.Value.Date) &&
                    (!dataDo.HasValue || p.DataPlanowana.Date <= dataDo.Value.Date) &&
                    (string.IsNullOrEmpty(typZabiegu) || typZabiegu == "Wszystkie" || p.TypZabiegu == typZabiegu) &&
                    (string.IsNullOrEmpty(status) || status == "Wszystkie" ||
                     (status == "Wykonane" && p.CzyWykonane) ||
                     (status == "Do wykonania" && !p.CzyWykonane))
                ).ToList();

                PrzypomnieniaPelneListView.ItemsSource = przypomnienia;
            }
            catch (Exception ex)
            {
                ShowError("filtrowania przypomnień", ex);
            }
        }


        private void WyczyscFiltry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataOdPicker.SelectedDate = null;
                DataDoPicker.SelectedDate = null;
                TypZabieguComboBox.SelectedIndex = 0;
                StatusComboBox.SelectedIndex = 0;
                PrzypomnieniaPelneListView.ItemsSource = _context.Przypomnienia.Include(p => p.Roslina)
                    .OrderBy(p => p.DataPlanowana).ToList();
            }
            catch (Exception ex)
            {
                ShowError("czyszczenia filtrów", ex);
            }
        }

        private void OdswiezPrzypomnienia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.ChangeTracker.Clear();
                FiltrujPrzypomnienia(sender, e);
                MessageBox.Show("Dane zostały odświeżone.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("odświeżania danych", ex);
            }
        }

        private void UsunRosline_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoslina == null) return;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć roślinę '{_selectedRoslina.Nazwa}'?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                var roslina = _context.Rosliny
                    .Include(r => r.Przypomnienia)
                    .Include(r => r.Zabiegi)
                    .FirstOrDefault(r => r.RoslinaId == _selectedRoslina.RoslinaId);

                if (roslina == null)
                    throw new Exception("Roślina nie istnieje w bazie danych.");

                _context.Przypomnienia.RemoveRange(roslina.Przypomnienia);
                _context.Zabiegi.RemoveRange(roslina.Zabiegi);
                _context.Rosliny.Remove(roslina);
                _context.SaveChanges();

                LoadData();
                MessageBox.Show($"Roślina '{roslina.Nazwa}' została usunięta.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("usuwania rośliny", ex);
            }
        }

        private void PokazSzczegoly_Click(object sender, RoutedEventArgs e) =>
            PokazSzczegolyRosliny(_selectedRoslina);

        private void PokazSzczegolyRosliny(Roslina roslina)
        {
            var okno = new SzczegolyRoslinyWindow(_context, roslina) { Owner = this };
            if (okno.ShowDialog() == true) LoadData();
        }

        private void DodajRosline_Click(object sender, RoutedEventArgs e)
        {
            var okno = new DodajRoslineWindow(_context) { Owner = this };
            if (okno.ShowDialog() == true) LoadData();
        }

        private void EdytujRosline_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoslina == null) return;
            var okno = new EdytujRoslineWindow(_context, _selectedRoslina) { Owner = this };
            if (okno.ShowDialog() == true) LoadData();
        }

        private void DodajGatunek_Click(object sender, RoutedEventArgs e)
        {
            var okno = new DodajGatunekWindow(_context) { Owner = this };
            if (okno.ShowDialog() == true)
            {
                LoadData();
                MessageBox.Show("Gatunek został dodany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Przypomnienie_Checked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox checkBox) || !(checkBox.DataContext is Przypomnienie przypomnienie)) return;

            try
            {
                using var ctx = new ApplicationDbContext();
                var przypDb = ctx.Przypomnienia.FirstOrDefault(p => p.PrzypomnienieId == przypomnienie.PrzypomnienieId);
                var roslina = ctx.Rosliny.Include(r => r.Gatunek).FirstOrDefault(r => r.RoslinaId == przypomnienie.RoslinaId);
                if (przypDb == null || roslina == null) throw new Exception("Brak danych przypomnienia lub rośliny.");

                przypDb.CzyWykonane = true;
                ctx.SaveChanges();

                var nextDate = przypomnienie.TypZabiegu switch
                {
                    "Podlewanie" => przypomnienie.DataPlanowana.AddDays(roslina.Gatunek.WymagaNawadnianiaCoIleDni),
                    "Nawożenie" => przypomnienie.DataPlanowana.AddDays(roslina.Gatunek.WymagaNawozeniaCoIleDni),
                    _ => przypomnienie.DataPlanowana.AddDays(7)
                };

                ctx.Zabiegi.Add(new Zabieg
                {
                    RoslinaId = przypomnienie.RoslinaId,
                    TypZabiegu = przypomnienie.TypZabiegu,
                    DataWykonania = DateTime.Now,
                    Opis = $"Wykonano z przypomnienia z dnia {przypomnienie.DataPlanowana:dd.MM.yyyy}"
                });
                ctx.Przypomnienia.Add(new Przypomnienie
                {
                    RoslinaId = przypomnienie.RoslinaId,
                    TypZabiegu = przypomnienie.TypZabiegu,
                    DataPlanowana = nextDate,
                    CzyWykonane = false
                });
                ctx.SaveChanges();

                MessageBox.Show($"Dodano nowe przypomnienie o {przypomnienie.TypZabiegu.ToLower()} na {nextDate:dd.MM.yyyy}.",
                    "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                _context.ChangeTracker.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                checkBox.IsChecked = false;
                ShowError("aktualizacji przypomnienia", ex);
            }
        }

        private void ShowError(string context, Exception ex) =>
            MessageBox.Show($"Błąd podczas {context}: {ex.Message}\n\nSzczegóły: {ex.StackTrace}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}
