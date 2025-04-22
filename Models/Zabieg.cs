using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenadzerRoslin.Models
{
    public class Zabieg
    {
        [Key]
        public int ZabiegId { get; set; }
        
        // Klucz obcy do Roslina
        [Required]
        public int RoslinaId { get; set; }
        
        [ForeignKey("RoslinaId")]
        public virtual Roslina Roslina { get; set; }
        
        [Required(ErrorMessage = "Typ zabiegu jest wymagany")]
        public string TypZabiegu { get; set; }
        
        [Required(ErrorMessage = "Data wykonania jest wymagana")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [PastDate(ErrorMessage = "Data wykonania nie może być z przyszłości")]
        public DateTime DataWykonania { get; set; }
        
        public string Opis { get; set; }
    }
}