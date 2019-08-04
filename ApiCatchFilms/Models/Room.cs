using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //Las dos etiquetas de arriba son necesarias para defiir que el atributo movieID es una llave primaria autoincrementable
        [Column("room_id")]
        public int roomID { get; set; }
        [Required]
        public int number { get; set; }
    }
}