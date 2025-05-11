using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using MenadzerRoslin.Data;
using MenadzerRoslin.Models;

namespace MenadzerRoslin.Views
{
    public partial class SzczegolyRoslinyWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly Roslina _roslina;

        public SzczegolyRoslinyWindow(ApplicationDbContext context, Roslina roslina)
        {
            InitializeComponent();
            _context = context;
            _roslina = roslina;

            // Wypełnienie pól danymi rośliny
            NazwaTextBlock.Text = _roslina.Nazwa;
            GatunekTextBlock.Text = _roslina.Gatunek.NazwaGatunku;
            MiejsceTextBlock.Text = _roslina.Miejsce;
            DataZakupuTextBlock.Text = _roslina.DataZakupu.ToString("dd.MM.yyyy");

            WymaganiaTextBlock.Text = $"Podlewanie co {_roslina.Gatunek.WymagaNawadnianiaCoIleDni} dni\n" +
                                     $"Nawożenie co {_roslina.Gatunek.WymagaNawozeniaCoIleDni} dni\n" +
                                     $"Światło: {_roslina.Gatunek.Swiatlo}\n" +
                                     $"Temperatura: {_roslina.Gatunek.TemperaturaMin}°C - {_roslina.Gatunek.TemperaturaMax}°C";

            // Załadowanie zdjęcia, jeśli istnieje
            LoadZdjecie();

            // Załadowanie historii zabiegów
            LoadZabiegi();
        }

        private void LoadZdjecie()
        {
            if (!string.IsNullOrEmpty(_roslina.ZdjeciePath) && File.Exists(_roslina.ZdjeciePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_roslina.ZdjeciePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ZdjecieImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas ładowania zdjęcia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    ZdjecieImage.Source = null;
                }
            }
            else
            {
                // Ustaw domyślne zdjęcie lub komunikat
                ZdjecieImage.Source = null;
            }
        }

        private void LoadZabiegi()
        {
            var zabiegi = _context.Zabiegi
                .Where(z => z.RoslinaId == _roslina.RoslinaId)
                .OrderByDescending(z => z.DataWykonania)
                .ToList();
            ZabiegiListView.ItemsSource = zabiegi;
        }

        private void DodajZabieg_Click(object sender, RoutedEventArgs e)
        {
            var dodajZabiegWindow = new DodajZabiegWindow(_context, _roslina);
            if (dodajZabiegWindow.ShowDialog() == true)
            {
                LoadZabiegi(); // Odświeżenie listy zabiegów
            }
        }

        private void EdytujRosline_Click(object sender, RoutedEventArgs e)
        {
            var edytujRoslineWindow = new EdytujRoslineWindow(_context, _roslina);
            if (edytujRoslineWindow.ShowDialog() == true)
            {
                // Odświeżenie danych rośliny
                _context.Entry(_roslina).Reload();
                _context.Entry(_roslina.Gatunek).Reload();

                // Aktualizacja UI
                NazwaTextBlock.Text = _roslina.Nazwa;
                GatunekTextBlock.Text = _roslina.Gatunek.NazwaGatunku;
                MiejsceTextBlock.Text = _roslina.Miejsce;
                DataZakupuTextBlock.Text = _roslina.DataZakupu.ToString("dd.MM.yyyy");

                WymaganiaTextBlock.Text = $"Podlewanie co {_roslina.Gatunek.WymagaNawadnianiaCoIleDni} dni\n" +
                                         $"Nawożenie co {_roslina.Gatunek.WymagaNawozeniaCoIleDni} dni\n" +
                                         $"Światło: {_roslina.Gatunek.Swiatlo}\n" +
                                         $"Temperatura: {_roslina.Gatunek.TemperaturaMin}°C - {_roslina.Gatunek.TemperaturaMax}°C";

                // Załadowanie zdjęcia ponownie
                LoadZdjecie();

                DialogResult = true;
            }
        }
    }
}