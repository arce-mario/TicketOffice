using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchFilms.Models
{
    public class Function
    {
       
        public int functionID { get; set; }
        public int movieID { get; set; }
        public int roomID { get; set; }
        public int priceID { get; set; }
        public DateTime time { get; set; }
        public int type { get; set; }
        public int typeMovie { get; set; }        
        public string description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Movie movie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Price price { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Room room { get; set; }
    }
}