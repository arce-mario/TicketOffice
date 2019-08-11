using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CatchFilms.Models
{
    public class Movie
    {
        public int? movieID {get; set;}
        [DisplayName("Nombre de la película")]
        public string name { get; set; }
        [DisplayName("Tipo")]
        public String type { get; set; }
        [DisplayName("Descripción")]
        public String description { get; set; }
        [DisplayName("Categoría")]
        public string classification { get; set; }
        [DisplayName("Hora")]
        public TimeSpan time { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string coverURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string imageURL { get; set; }
        public float rating { get; set; }
    }
}