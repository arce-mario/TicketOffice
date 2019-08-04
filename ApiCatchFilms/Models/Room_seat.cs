using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Room_seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //Las dos etiquetas de arriba son necesarias para defiir que el atributo movieID es una llave primaria autoincrementable
        [Column("room_Seat_id")]
        public int roomSeatID { get; set; }
        [Required]
        public int status { get; set; }
        [Required]
        public int roomSeatID { get; set; }
    }
}