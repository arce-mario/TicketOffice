using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class ApiCatchFilmsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ApiCatchFilmsContext() : base("name=ApiCatchFilmsContext")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Function> Functions { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Movie> Movies { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Price> Prices { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Room> Rooms { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Seat> Seats { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.RoomSeat> RoomSeats { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Ticket> Tickets { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasRequired(p => p.price).WithMany().HasForeignKey(p => p.priceID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Ticket>().HasRequired(p => p.roomSeat).WithMany().HasForeignKey(p => p.roomSeatID).WillCascadeOnDelete(false);
        }
    }
}
