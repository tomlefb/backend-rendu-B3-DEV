namespace ApiEnCouches.DataAccess.Seed;

using ApiEnCouches.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

public class ReservationSeeder : ISeeder
{
    public async Task Execute(AppDbContext db, bool isProduction)
    {
        if (isProduction) return;

        if (!db.Reservations.Any())
        {
            var users = await db.Users.ToListAsync();
            var rooms = await db.MeetingRooms.ToListAsync();

            if (users.Any() && rooms.Any())
            {
                var now = DateTime.UtcNow;

                var reservations = new List<ReservationsModel>
                {
                    new ReservationsModel
                    {
                        UserId = users[0].UserId,
                        RoomId = rooms[0].RoomId,
                        StartDate = now.AddDays(1).AddHours(9),
                        EndDate = now.AddDays(1).AddHours(11)
                    },
                    new ReservationsModel
                    {
                        UserId = users[1].UserId,
                        RoomId = rooms[1].RoomId,
                        StartDate = now.AddDays(2).AddHours(14),
                        EndDate = now.AddDays(2).AddHours(16)
                    },
                    new ReservationsModel
                    {
                        UserId = users[2].UserId,
                        RoomId = rooms[0].RoomId,
                        StartDate = now.AddDays(3).AddHours(10),
                        EndDate = now.AddDays(3).AddHours(12)
                    },
                    new ReservationsModel
                    {
                        UserId = users[0].UserId,
                        RoomId = rooms[2].RoomId,
                        StartDate = now.AddDays(1).AddHours(15),
                        EndDate = now.AddDays(1).AddHours(17)
                    }
                };

                await db.Reservations.AddRangeAsync(reservations);
                await db.SaveChangesAsync();
            }
        }
    }
}
