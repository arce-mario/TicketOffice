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
        [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DisplayName("Precio para adultos")]
        public decimal adultPrice { set; get; }
        [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DisplayName("Precio para niños")]
        public decimal childPrice { get; set; }
        [Range(1, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DisplayName("Precio para la tercera edad")]
        public decimal oldManPrice { get; set; }
    }
}