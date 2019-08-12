using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchFilms.Models
{
    public class Function
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? functionID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? movieID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? roomID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? priceID { get; set; }
        [DisplayName("Fecha y hora de la función")]
        public DateTime time { get; set; }
        [DisplayName("Tipo (Estreno, Premier...)")]
        public int type { get; set; }
        [DisplayName("Tipo (2D,3D)")]
        public int typeMovie { get; set; }
        [DisplayName("Detalles de la función")]
        public string description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Movie movie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Price price { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Room room { get; set; }
    }
}