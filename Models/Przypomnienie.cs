using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenadzerRoslin.Models
{
    public class Przypomnienie
    {
        [Key]
        public int PrzypomnienieId { get; set; }
        
        // Klucz obcy do Roslina
        [Required]
        public int RoslinaId { get; set; }
        
        [ForeignKey("RoslinaId")]
        public virtual Roslina Roslina { get; set; }
        
        [Required(ErrorMessage = "Typ zabiegu jest wymagany")]
        public string TypZabiegu { get; set; }
        
        [Required(ErrorMessage = "Data planowana jest wymagana")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataPlanowana { get; set; }
        
        public bool CzyWykonane { get; set; }
    }
}