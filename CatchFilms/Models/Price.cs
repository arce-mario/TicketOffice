using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchFilms.Models
{
    public class Price
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? priceID { get; set; }
        [MinLength(1, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DisplayName("Precio para adultos")]
        public decimal adultPrice { set; get; }
        [MinLength(1, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DisplayName("Precio para niños")]
        public decimal childPrice { get; set; }
        [MinLength(1, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DisplayName("Precio para la tercera edad")]
        public decimal oldManPrice { get; set; }
    }
}