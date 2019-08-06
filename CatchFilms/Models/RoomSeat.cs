using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    [Table("Room_seat")]
    public class RoomSeat
    {
        
        public int roomSeatID { get; set; }
        public int status { get; set; }
        public int seatID { get; set; }
        public int roomID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Room room { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Seat seat { get; set; }
    }
}