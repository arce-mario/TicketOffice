﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CatchFilms.Models
{
    public class Movie
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? movieID {get; set;}
        [Required(ErrorMessage ="Debe ingresar el nombre de la película.")]
        [DisplayName("Nombre de la película")]
        public string name { get; set; }
        [DisplayName("Tipo")]
        public String type { get; set; }
        [DisplayName("Descripción de la película")]
        public String description { get; set; }
        [Required(ErrorMessage = "Debe ingresar la categoría de la película.")]
        [DisplayName("Categoría")]
        public string classification { get; set; }
        [Required(ErrorMessage = "Debe ingresar la duración de la película.")]
        [DisplayName("Duración")]
        public TimeSpan time { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DisplayName("Imagen de portada")]
        public string coverURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DisplayName("Imagen de la película")]
        public string imageURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int status { get; set; }
        public float rating { get; set; }
        //Esto campos son generados automaticamente, en algunos casos calculados, no forman parte de la entidad en la base datos
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Function> functions { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string moviePremier { get; set; }
    }
}