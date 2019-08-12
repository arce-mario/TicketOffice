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
        [DisplayName("Precio para adultos")]
        public decimal adultPrice { set; get; }
        [DisplayName("Precio para niños")]
        public decimal childPrice { get; set; }
        [DisplayName("Precio para la tercera edad")]
        public decimal oldManPrice { get; set; }
    }
}