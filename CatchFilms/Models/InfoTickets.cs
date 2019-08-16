using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CatchFilms.Models
{
    public class InfoTickets
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 10, ErrorMessage = "El valor debe ser mayor a 0")]
        public int cantAdult { get; set; }
        [Range(0, 10, ErrorMessage = "El valor debe ser mayor a 0")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int cantChildPrice { get; set; }
        [Range(0, 10, ErrorMessage = "El valor debe ser mayor a 0")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int cantOldMan { get; set; }
    }
}