using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;

namespace MenadzerRoslin
{
    public partial class EdytujRoslineWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly Roslina _roslina;
        private string _selectedImagePath;
        private string _imagesFolder;
        private bool _zdjecieZmienione = false;
        private bool _zdjecieUsuniete = false;

        public EdytujRoslineWindow(ApplicationDbContext context, Roslina roslina)
        {
            InitializeComponent();
            _context = context;
            _roslina = roslina;

            _imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(_imagesFolder))
            {
                Directory.CreateDirectory(_imagesFolder);
            }

            GatunekComboBox.ItemsSource = _context.Gatunki.ToList();

            NazwaTextBox.Text = _roslina.Nazwa;
            GatunekComboBox.SelectedItem = _context.Gatunki.FirstOrDefault(g => g.GatunekId == _roslina.GatunekId);

            foreach (ComboBoxItem item in MiejsceComboBox.Items)
            {
                if (item.Content.ToString() == _roslina.Miejsce)
                {
                    MiejsceComboBox.SelectedItem = item;
                    break;
                }
            }

            DataZakupuDatePicker.SelectedDate = _roslina.DataZakupu;

            if (!string.IsNullOrEmpty(_roslina.ZdjeciePath) && File.Exists(_roslina.ZdjeciePath))
            {
                ZdjeciePathTextBox.Text = _roslina.ZdjeciePath;
                LoadZdjeciePreview(_roslina.ZdjeciePath);
                UsunZdjecieButton.Visibility = Visibility.Visible;
            }
        }

        private void LoadZdjeciePreview(string imagePath)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ZdjeciePreview.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania zdjęcia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                ZdjeciePreview.Source = null;
            }
        }

        private void WybierzZdjecie_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Wybierz zdjęcie rośliny",
                Filter = "Pliki obrazów|*.jpg;*.jpeg;*.png;*.bmp|Wszystkie pliki|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                ZdjeciePathTextBox.Text = _selectedImagePath;
                LoadZdjeciePreview(_selectedImagePath);
                _zdjecieZmienione = true;
                _zdjecieUsuniete = false;
                UsunZdjecieButton.Visibility = Visibility.Visible;
            }
        }

        private void UsunZdjecie_Click(object sender, RoutedEventArgs e)
        {
            ZdjeciePreview.Source = null;
            ZdjeciePathTextBox.Text = "";
            _selectedImagePath = null;
            _zdjecieZmienione = false;
            _zdjecieUsuniete = true;
            UsunZdjecieButton.Visibility = Visibility.Collapsed;
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

                if (NazwaTextBox.Text.Length < 3)
                {
                    MessageBox.Show("Nazwa rośliny musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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

                // Wymagaj zdjęcia, jeśli zostało usunięte i nie dodano nowego
                if (_zdjecieUsuniete && string.IsNullOrEmpty(_selectedImagePath) && string.IsNullOrEmpty(_roslina.ZdjeciePath))
                {
                    MessageBox.Show("Musisz dodać zdjęcie rośliny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obsługa zdjęcia
                if (_zdjecieZmienione && !string.IsNullOrEmpty(_selectedImagePath))
                {
                    if (!string.IsNullOrEmpty(_roslina.ZdjeciePath) && File.Exists(_roslina.ZdjeciePath))
                    {
                        try
                        {
                            File.Delete(_roslina.ZdjeciePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Nie udało się usunąć starego zdjęcia: {ex.Message}", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(_selectedImagePath)}";
                    string destinationPath = Path.Combine(_imagesFolder, fileName);

                    try
                    {
                        File.Copy(_selectedImagePath, destinationPath);
                        _roslina.ZdjeciePath = destinationPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas kopiowania zdjęcia: {ex.Message}\nRoślina zostanie zapisana bez zmiany zdjęcia.",
                            "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else if (_zdjecieUsuniete)
                {
                    if (!string.IsNullOrEmpty(_roslina.ZdjeciePath) && File.Exists(_roslina.ZdjeciePath))
                    {
                        try
                        {
                            File.Delete(_roslina.ZdjeciePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Nie udało się usunąć zdjęcia: {ex.Message}", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    _roslina.ZdjeciePath = null;
                }

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
