using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CatchFilms.Models
{
    public class User
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? userID { get; set; }
        [DisplayName("Nombre")]
        public string firstName { get; set; }
        [DisplayName("Apellido")]
        public string lastName { get; set; }
        [DisplayName("Correo electronico")]
        public string email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DisplayName("Fecha de registro")]
        public DateTime? hireDare { get; set; }
        [DisplayName("Rol")]
        public string userName { get; set; }
        [DisplayName("Contraseña")]
        public string pass { get; set; }
        [DisplayName("Fecha de nacimiento")]
        public DateTime birthDate { get; set; }
        [DisplayName("Id de usuario")]
        public int rol { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string imageUserURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Ticket> tickets { get; set; }
    }
}