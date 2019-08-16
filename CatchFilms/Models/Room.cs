using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CatchFilms.Models
{
    public class Room
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? roomID { get; set; }
        [DisplayName("Número de sala")]
        [Range(1, 100000, ErrorMessage = "El número de sala debe ser mayor a 0.")]
        [Required (ErrorMessage = "Este campo es requerido.")]
        public int number { get; set; }
        [DisplayName("Descripción")]
        public String description { get; set; }
    }
}