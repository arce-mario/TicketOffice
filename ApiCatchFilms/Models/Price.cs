using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class Price
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        public int type { get; set; }
        [Required]
        public decimal price { get; set; }
    }
}