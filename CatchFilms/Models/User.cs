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
        [Required (ErrorMessage = "El nombre es un campo requerido.")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "El apellido es un campo requerido.")]
        [DisplayName("Apellido")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "El email es un campo requerido.")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Correo electronico")]
        public string email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DisplayName("Fecha de registro")]
        public DateTime? hireDare { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es un campo requerido.")]
        [DisplayName("Nombre de usuario")]
        public string userName { get; set; }
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "La contraseña debe tener mínimo 8 caracteres y contener 3 de 4 de lo siguiente: Mayúscula (A-Z), Minúscula(a-z), Número (0-9) Caracter especial (e.g. !@#$%^&*)")]
        public string pass { get; set; }
        [Required(ErrorMessage = "Ingrese el formato correcto")]
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