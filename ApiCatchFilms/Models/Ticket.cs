using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Ticket
    {
        [Key]
        [Column("ticket_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ticketID { get; set; }
        [Required]
        [Column("create_at")]
        public DateTime createAT { get; set; }
        [Column("room_seat_id")]
        public int roomSeatID { get; set; }
        [Required]
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        [Column("user_id")]
        public int userID { get; set; }
        [Required]
        [Column("function_id")]
        public int functionID { get; set; }
        public virtual RoomSeat roomSeat { get; set; }
        public virtual Function function { get; set; }
        public Price price { get; set; }
        public virtual User user { get; set; }
    }
}