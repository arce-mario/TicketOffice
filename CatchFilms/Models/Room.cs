using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CatchFilms.Models
{
    public class Room
    {
       
        public int roomID { get; set; }
        public int number { get; set; }        
        public String description { get; set; }
    }
}