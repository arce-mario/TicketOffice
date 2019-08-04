﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_id")]
        public int roomID { get; set; }
        [Required]
        public int number { get; set; }
        [StringLength(500)]
        public String description { get; set; }
        public virtual ICollection<Function> functions { get; set; }
        public virtual ICollection<RoomSeat> roomSeats { get; set; }
    }
}