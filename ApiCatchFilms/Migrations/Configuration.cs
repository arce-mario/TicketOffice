namespace ApiCatchFilms.Migrations
{
    using ApiCatchFilms.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApiCatchFilms.Models.ApiCatchFilmsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ApiCatchFilms.Models.ApiCatchFilmsContext";
        }

        protected override void Seed(ApiCatchFilms.Models.ApiCatchFilmsContext context)
        {
            context.Users.AddOrUpdate( u => u.userID, new User(){
                firstName ="Roberto",
                lastName ="Rodríguez",
                email ="roberto@gmail.com",
                hireDare = new DateTime(2019,8,4),
                userName ="Administrador",
                pass ="Usuario123.",
                birthDate = new DateTime(1999,05,29),
                rol = 1
            });
            context.SaveChanges();

            context.Rooms.AddOrUpdate(r => r.roomID,new Room() {
                number = 1,
                description = "Proyector de EPSON FULLHD"
            });
            context.SaveChanges();

            context.Seats.AddOrUpdate(s => s.seatID, new Seat() {
                column = 1,
                row = "A"
            });
            context.SaveChanges();

            try
            {
                int lastSeatID = context.Seats.OrderByDescending(s => s.seatID).First().seatID;
                int lastRoomID = context.Rooms.OrderByDescending(r => r.roomID).First().roomID;

                context.RoomSeats.AddOrUpdate(rs => rs.roomSeatID, new RoomSeat
                {
                    roomID = lastRoomID,
                    seatID = lastSeatID,
                    status = 1
                });
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Concat("Ha ocurrido un error",e.Message));
            }
        }
    }
}