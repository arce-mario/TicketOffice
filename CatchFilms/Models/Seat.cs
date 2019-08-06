using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class Seat
    {
       
        public int seatID { get; set; }
        
        public int column { get; set; }
        
        public char row { get; set; }
    }
}