using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;

namespace MenadzerRoslin
{
    public partial class DodajRoslineWindow : Window
    {
        private readonly ApplicationDbContext _context;

        public DodajRoslineWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;

            // Ustawienie domyślnej daty na dzisiaj
            DataZakupuDatePicker.SelectedDate = DateTime.Today;

            // Załadowanie gatunków do ComboBox
            GatunekComboBox.ItemsSource = _context.Gatunki.ToList();
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
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

                // Utworzenie nowej rośliny
                var roslina = new Roslina
                {
                    Nazwa = NazwaTextBox.Text,
                    GatunekId = (GatunekComboBox.SelectedItem as Gatunek).GatunekId,
                    Miejsce = (MiejsceComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    DataZakupu = DataZakupuDatePicker.SelectedDate.Value
                };

                _context.Rosliny.Add(roslina);
                _context.SaveChanges();
                

                _context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania rośliny: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}