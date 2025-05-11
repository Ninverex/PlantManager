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
    public partial class DodajRoslineWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private string _selectedImagePath;
        private string _imagesFolder;

        public DodajRoslineWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;

            // Ustawienie domyślnej daty na dzisiaj
            DataZakupuDatePicker.SelectedDate = DateTime.Today;

            // Załadowanie gatunków do ComboBox
            GatunekComboBox.ItemsSource = _context.Gatunki.ToList();
            
            // Utworzenie folderu na zdjęcia, jeśli nie istnieje
            _imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(_imagesFolder))
            {
                Directory.CreateDirectory(_imagesFolder);
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
                
                // Wyświetl podgląd zdjęcia
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_selectedImagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ZdjeciePreview.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas ładowania zdjęcia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    ZdjeciePreview.Source = null;
                    _selectedImagePath = null;
                    ZdjeciePathTextBox.Text = "";
                }
            }
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

                if (DataZakupuDatePicker.SelectedDate > DateTime.Now)
                {
                    MessageBox.Show("Data zakupu nie może być z przyszłości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kopiowanie zdjęcia do folderu aplikacji, jeśli zostało wybrane
                string zdjeciePath = null;
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(_selectedImagePath)}";
                    string destinationPath = Path.Combine(_imagesFolder, fileName);
                    
                    try
                    {
                        File.Copy(_selectedImagePath, destinationPath);
                        zdjeciePath = destinationPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas kopiowania zdjęcia: {ex.Message}\nRoślina zostanie dodana bez zdjęcia.", 
                            "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // Utworzenie nowej rośliny
                var roslina = new Roslina
                {
                    Nazwa = NazwaTextBox.Text,
                    GatunekId = (GatunekComboBox.SelectedItem as Gatunek).GatunekId,
                    Miejsce = (MiejsceComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    DataZakupu = DataZakupuDatePicker.SelectedDate.Value,
                    ZdjeciePath = zdjeciePath
                };

                _context.Rosliny.Add(roslina);
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