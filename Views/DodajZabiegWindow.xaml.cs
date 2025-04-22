using System;
using System.Windows;
using System.Windows.Controls;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;

namespace MenadzerRoslin
{
    public partial class DodajZabiegWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly Roslina _roslina;

        public Zabieg Zabieg { get; private set; }
        public bool DodajPrzypomnienie => DodajPrzypomnienieCheckBox.IsChecked ?? false;

        public DodajZabiegWindow(ApplicationDbContext context, Roslina roslina)
        {
            InitializeComponent();
            _context = context;
            _roslina = roslina;

            // Wypełnienie pól danymi
            RoslinaTextBlock.Text = _roslina.Nazwa;
            DataWykonaniaDatePicker.SelectedDate = DateTime.Today;
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Walidacja danych
                if (TypZabieguComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Wybierz typ zabiegu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DataWykonaniaDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Wybierz datę wykonania zabiegu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DataWykonaniaDatePicker.SelectedDate > DateTime.Today)
                {
                    MessageBox.Show("Data wykonania nie może być z przyszłości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Utworzenie nowego zabiegu
                Zabieg = new Zabieg
                {
                    RoslinaId = _roslina.RoslinaId,
                    TypZabiegu = (TypZabieguComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    DataWykonania = DataWykonaniaDatePicker.SelectedDate.Value,
                    Opis = OpisTextBox.Text
                };

                _context.Zabiegi.Add(Zabieg);
                _context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania zabiegu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}