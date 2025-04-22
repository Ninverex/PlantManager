using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;

namespace MenadzerRoslin
{
    public partial class EdytujRoslineWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly Roslina _roslina;

        public EdytujRoslineWindow(ApplicationDbContext context, Roslina roslina)
        {
            InitializeComponent();
            _context = context;
            _roslina = roslina;

            // Załadowanie gatunków do ComboBox
            GatunekComboBox.ItemsSource = _context.Gatunki.ToList();

            // Wypełnienie pól danymi rośliny
            NazwaTextBox.Text = _roslina.Nazwa;
            GatunekComboBox.SelectedItem = _context.Gatunki.FirstOrDefault(g => g.GatunekId == _roslina.GatunekId);
            
            // Wybór odpowiedniego miejsca z ComboBox
            foreach (ComboBoxItem item in MiejsceComboBox.Items)
            {
                if (item.Content.ToString() == _roslina.Miejsce)
                {
                    MiejsceComboBox.SelectedItem = item;
                    break;
                }
            }
            
            DataZakupuDatePicker.SelectedDate = _roslina.DataZakupu;
        }

        private void ZapiszButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Walidacja danych
                if (string.IsNullOrWhiteSpace(NazwaTextBox.Text))
                {
                    MessageBox.Show("Nazwa rośliny jest wymagana.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (GatunekComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Wybierz gatunek rośliny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MiejsceComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Wybierz miejsce dla rośliny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DataZakupuDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Wybierz datę zakupu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DataZakupuDatePicker.SelectedDate > DateTime.Today)
                {
                    MessageBox.Show("Data zakupu nie może być z przyszłości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Aktualizacja danych rośliny
                _roslina.Nazwa = NazwaTextBox.Text;
                _roslina.GatunekId = (GatunekComboBox.SelectedItem as Gatunek).GatunekId;
                _roslina.Miejsce = (MiejsceComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                _roslina.DataZakupu = DataZakupuDatePicker.SelectedDate.Value;

                _context.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas edycji rośliny: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}