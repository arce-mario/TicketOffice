using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchFilms.Models
{
    public class User
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? hireDare { get; set; }
        public string userName { get; set; }
        public string pass { get; set; }
        public DateTime birthDate { get; set; }
        public int rol { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string imageUserURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Ticket> tickets { get; set; }
    }
}