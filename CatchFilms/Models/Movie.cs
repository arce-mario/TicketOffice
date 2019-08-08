using System;

namespace CatchFilms.Models
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