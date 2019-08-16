using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchFilms.Models
{
    public class Seat
    {
       
        public int seatID { get; set; }
        [StringLength(20, ErrorMessage = "El valor debe ser entre 10 y 20.", MinimumLength =10)]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int column { get; set; }
        public string row { get; set; }
    }
}