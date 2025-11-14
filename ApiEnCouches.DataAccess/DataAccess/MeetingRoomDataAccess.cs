namespace ApiEnCouches.DataAccess.DataAccess;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using Microsoft.EntityFrameworkCore;

public class MeetingRoomDataAccess : IMeetingRooms
{
    private readonly AppDbContext context;

    public MeetingRoomDataAccess(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<MeetingRoomsModel> GetById(int roomId)
    {
        return await this.context.MeetingRooms
            .Include(m => m.Reservations)
            .FirstOrDefaultAsync(m => m.RoomId == roomId);
    }

    public async Task<List<MeetingRoomsModel>> GetAll()
    {
        return await this.context.MeetingRooms
            .Include(m => m.Reservations)
            .ToListAsync();
    }

    public async Task<MeetingRoomsModel> Create(MeetingRoomsModel room)
    {
        await this.context.MeetingRooms.AddAsync(room);
        await this.context.SaveChangesAsync();
        return room;
    }

    public async Task<bool> Delete(int roomId)
    {
        var room = await this.context.MeetingRooms.FindAsync(roomId);
        if (room == null) return false;

        this.context.MeetingRooms.Remove(room);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ReservationsModel>> GetReservationsByRoomId(int roomId)
    {
        return await this.context.Reservations
            .Where(r => r.RoomId == roomId)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }
}
