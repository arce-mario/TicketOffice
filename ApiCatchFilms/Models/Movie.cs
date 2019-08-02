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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("movie_id")]
        public int movieID {get; set;}
        [Required]
        [StringLength(30)]
        public string name { get; set; }
        [Required]
        [StringLength(30)]
        public String type { get; set; }
        [Required]
        [StringLength(500)]
        public String description { get; set; }
        [Required]
        [StringLength(30)]
        public string classification { get; set; }
        [Required]
        public TimeSpan time { get; set; }
    }
}