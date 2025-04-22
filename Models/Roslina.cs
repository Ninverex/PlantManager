using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenadzerRoslin.Models
{
    public class Roslina
    {
        [Key]
        public int RoslinaId { get; set; }
        
        [Required(ErrorMessage = "Nazwa rośliny jest wymagana")]
        [MinLength(3, ErrorMessage = "Nazwa rośliny musi mieć co najmniej 3 znaki")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$", ErrorMessage = "Nazwa rośliny może zawierać tylko litery i spacje")]
        public string Nazwa { get; set; }
        
        [Required(ErrorMessage = "Data zakupu jest wymagana")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [PastDate(ErrorMessage = "Data zakupu nie może być z przyszłości")]
        public DateTime DataZakupu { get; set; }
        
        [Required(ErrorMessage = "Miejsce jest wymagane")]
        public string Miejsce { get; set; }
        
        // Klucz obcy do Gatunek
        [Required(ErrorMessage = "Gatunek jest wymagany")]
        public int GatunekId { get; set; }
        
        [ForeignKey("GatunekId")]
        public virtual Gatunek Gatunek { get; set; }
        
        // Relacje jeden-do-wielu z Zabieg i Przypomnienie
        public virtual ICollection<Zabieg> Zabiegi { get; set; }
        public virtual ICollection<Przypomnienie> Przypomnienia { get; set; }
    }
    
    // Własny atrybut walidacji dla daty z przeszłości
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date <= DateTime.Now;
        }
    }
}