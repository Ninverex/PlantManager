using System;
using System.Linq;
using System.Windows;
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

            // Załadowanie historii zabiegów
            LoadZabiegi();
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
                // Odświeżenie listy zabiegów
                LoadZabiegi();

                // Dodanie przypomnienia dla następnego zabiegu
                if (dodajZabiegWindow.DodajPrzypomnienie && dodajZabiegWindow.Zabieg.TypZabiegu == "Podlewanie")
                {
                    var dataNastepnegoPodlewania = dodajZabiegWindow.Zabieg.DataWykonania.AddDays(_roslina.Gatunek.WymagaNawadnianiaCoIleDni);
                    var przypomnienie = new Przypomnienie
                    {
                        RoslinaId = _roslina.RoslinaId,
                        TypZabiegu = "Podlewanie",
                        DataPlanowana = dataNastepnegoPodlewania,
                        CzyWykonane = false
                    };
                    _context.Przypomnienia.Add(przypomnienie);
                    _context.SaveChanges();
                }
                else if (dodajZabiegWindow.DodajPrzypomnienie && dodajZabiegWindow.Zabieg.TypZabiegu == "Nawożenie")
                {
                    var dataNastepnegoNawozenia = dodajZabiegWindow.Zabieg.DataWykonania.AddDays(_roslina.Gatunek.WymagaNawadnianiaCoIleDni);
                    var przypomnienie = new Przypomnienie
                    {
                        RoslinaId = _roslina.RoslinaId,
                        TypZabiegu = "Nawożenie",
                        DataPlanowana = dataNastepnegoNawozenia,
                        CzyWykonane = false
                    };
                    _context.Przypomnienia.Add(przypomnienie);
                    _context.SaveChanges();
                }
                else
                {
                    var dataNastepnegoZabiegu = dodajZabiegWindow.Zabieg.DataWykonania.AddDays(7); // Inny typ zabiegu co 7 dni
                    var przypomnienie = new Przypomnienie
                    {
                        RoslinaId = _roslina.RoslinaId,
                        TypZabiegu = dodajZabiegWindow.Zabieg.TypZabiegu,
                        DataPlanowana = dataNastepnegoZabiegu,
                        CzyWykonane = false
                    };
                    _context.Przypomnienia.Add(przypomnienie);
                    _context.SaveChanges();
                }
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

                // Powiadomienie głównego okna o zmianach
                DialogResult = true;
            }
        }
    }
}