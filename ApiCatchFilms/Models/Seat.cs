using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //Las dos etiquetas de arriba son necesarias para defiir que el atributo movieID es una llave primaria autoincrementable
        [Column("seat_id")]
        public int seatID { get; set; }
        [Required]
        public int column { get; set; }
        [Required]
        public char row { get; set; }
    }
}