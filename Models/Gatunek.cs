using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenadzerRoslin.Models
{
    public class Gatunek
    {
        [Key]
        public int GatunekId { get; set; }
        
        [Required(ErrorMessage = "Nazwa gatunku jest wymagana")]
        [MinLength(3, ErrorMessage = "Nazwa gatunku musi mieć co najmniej 3 znaki")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$", ErrorMessage = "Nazwa gatunku może zawierać tylko litery i spacje")]
        public string NazwaGatunku { get; set; }
        
        [Range(1, 60, ErrorMessage = "Częstotliwość nawadniania musi być między 1 a 60 dni")]
        public int WymagaNawadnianiaCoIleDni { get; set; }
        
        [Range(1, 365, ErrorMessage = "Częstotliwość nawożenia musi być między 1 a 365 dni")]
        public int WymagaNawozeniaCoIleDni { get; set; }
        
        [Required(ErrorMessage = "Informacja o wymaganiach dotyczących światła jest wymagana")]
        public string Swiatlo { get; set; }
        
        [Range(-10, 40, ErrorMessage = "Minimalna temperatura musi być między -10°C a 40°C")]
        public double TemperaturaMin { get; set; }
        
        [Range(-5, 50, ErrorMessage = "Maksymalna temperatura musi być między -5°C a 50°C")]
        public double TemperaturaMax { get; set; }
        
        // Relacja jeden-do-wielu z Roslina
        public virtual ICollection<Roslina> Rosliny { get; set; }
    }
}