using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class Price
    {
       
        public int priceID { get; set; }
        public decimal adultPrice { set; get; }
        public decimal childPrice { get; set; }
        public decimal oldManPrice { get; set; }
    }
}