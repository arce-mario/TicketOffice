using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Price
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //Las dos etiquetas de arriba son necesarias para defiir que el atributo movieID es una llave primaria autoincrementable
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        public int type { get; set; }
        [Required]
        public decimal price { get; set; }
    }
}