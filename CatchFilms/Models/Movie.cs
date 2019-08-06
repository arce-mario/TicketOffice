using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Movie
    {
        
        public int movieID {get; set;}
        
        public string name { get; set; }
        
        public String type { get; set; }
       
        public String description { get; set; }
        
        public string classification { get; set; }
        
        public TimeSpan time { get; set; }
    }
}