using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CatchFilms.Models
{
    public class Ticket
    {
      
        public int? ticketID { get; set; }
        public DateTime createAT { get; set; }
        public int? roomSeatID { get; set; }
        public int? priceID { get; set; }
        public int? userID { get; set; }
        public int? functionID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RoomSeat roomSeat { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Function function { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Price price { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public User user { get; set; }
    }
}