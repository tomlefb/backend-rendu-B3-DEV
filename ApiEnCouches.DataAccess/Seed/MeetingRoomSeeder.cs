namespace ApiEnCouches.DataAccess.Seed;

using ApiEnCouches.DataAccess.Models;

public class MeetingRoomSeeder : ISeeder
{
    public async Task Execute(AppDbContext db, bool isProduction)
    {
        if (isProduction) return;

        if (!db.MeetingRooms.Any())
        {
            var rooms = new List<MeetingRoomsModel>
            {
                new MeetingRoomsModel
                {
                    RoomName = "Salle de conférence A",
                    Capacity = 20
                },
                new MeetingRoomsModel
                {
                    RoomName = "Salle de réunion B",
                    Capacity = 10
                },
                new MeetingRoomsModel
                {
                    RoomName = "Salle de brainstorming C",
                    Capacity = 6
                },
                new MeetingRoomsModel
                {
                    RoomName = "Grande salle D",
                    Capacity = 50
                },
                new MeetingRoomsModel
                {
                    RoomName = "Petit bureau E",
                    Capacity = 4
                }
            };

            await db.MeetingRooms.AddRangeAsync(rooms);
            await db.SaveChangesAsync();
        }
    }
}
