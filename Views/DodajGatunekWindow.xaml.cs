using System;
using System.Windows;
using System.Windows.Controls;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;
using System.Text.RegularExpressions;

namespace MenadzerRoslin.Views
{
    public partial class DodajGatunekWindow : Window
    {
        private readonly ApplicationDbContext _context;

        public Gatunek NowyGatunek { get; private set; }

        public DodajGatunekWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void DodajButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Walidacja danych
                if (string.IsNullOrWhiteSpace(NazwaGatunkuTextBox.Text))
                {
                    MessageBox.Show("Nazwa gatunku jest wymagana.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(NazwaGatunkuTextBox.Text, @"^[A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż ]+$"))
                {
                    MessageBox.Show("Nazwa gatunku może zawierać tylko litery i spacje (bez cyfr i znaków specjalnych).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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

                if (tempMin < -10 || tempMin > 40)
                {
                    MessageBox.Show("Minimalna temperatura musi być w zakresie od -10 do 40°C.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(TempMaxTextBox.Text, out double tempMax))
                {
                    MessageBox.Show("Podaj prawidłową maksymalną temperaturę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (tempMax < -5 || tempMax > 50)
                {
                    MessageBox.Show("Maksymalna temperatura musi być w zakresie od -5 do 50°C.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (tempMin >= tempMax)
                {
                    MessageBox.Show("Temperatura minimalna musi być mniejsza od maksymalnej.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Utworzenie nowego gatunku
                NowyGatunek = new Gatunek
                {
                    NazwaGatunku = NazwaGatunkuTextBox.Text,
                    WymagaNawadnianiaCoIleDni = podlewanieDni,
                    WymagaNawozeniaCoIleDni = nawozenieDni,
                    Swiatlo = (SwiatloComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    TemperaturaMin = tempMin,
                    TemperaturaMax = tempMax
                };

                _context.Gatunki.Add(NowyGatunek);
                _context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania gatunku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
