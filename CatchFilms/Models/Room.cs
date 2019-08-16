using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CatchFilms.Models
{
    public class Room
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? roomID { get; set; }
        [DisplayName("Número de sala")]
        [Range(1, 100000, ErrorMessage = "El valor debe ser mayor a 0")]
        [Required(ErrorMessage = "El campo es requerido")]
        public int number { get; set; }
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        public String description { get; set; }
        //Los siguientes atributos son campos generados automaticamente o calculados
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(3, 10, ErrorMessage = "El valor debe estar entre 3 y 10")]
        public int? rows { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(6, 20, ErrorMessage = "El valor debe estar entre 6 y 20")]
        public int? columns { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string seatArray { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string notAvailable { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RoomSeat> roomSeats { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? seatAvalaibles { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? seatNotAvalaibles { get; set; }
    }
}