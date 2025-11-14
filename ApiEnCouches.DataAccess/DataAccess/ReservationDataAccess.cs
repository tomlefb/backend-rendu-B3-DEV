namespace ApiEnCouches.DataAccess.DataAccess;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using Microsoft.EntityFrameworkCore;

public class ReservationDataAccess : IReservations
{
    private readonly AppDbContext context;

    public ReservationDataAccess(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<ReservationsModel> GetById(int reservationId)
    {
        return await this.context.Reservations
            .Include(r => r.User)
            .Include(r => r.MeetingRoom)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<List<ReservationsModel>> GetByUserId(int userId)
    {
        return await this.context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.MeetingRoom)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<List<ReservationsModel>> GetAll()
    {
        return await this.context.Reservations
            .Include(r => r.User)
            .Include(r => r.MeetingRoom)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<ReservationsModel> Create(ReservationsModel reservation)
    {
        await this.context.Reservations.AddAsync(reservation);
        await this.context.SaveChangesAsync();
        return await GetById(reservation.ReservationId);
    }

    public async Task<ReservationsModel> Update(ReservationsModel reservation)
    {
        this.context.Reservations.Update(reservation);
        await this.context.SaveChangesAsync();
        return await GetById(reservation.ReservationId);
    }

    public async Task<bool> Delete(int reservationId)
    {
        var reservation = await this.context.Reservations.FindAsync(reservationId);
        if (reservation == null) return false;

        this.context.Reservations.Remove(reservation);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasConflict(int roomId, DateTime startDate, DateTime endDate, int? excludeReservationId = null)
    {
        var query = this.context.Reservations
            .Where(r => r.RoomId == roomId);

        if (excludeReservationId.HasValue)
        {
            query = query.Where(r => r.ReservationId != excludeReservationId.Value);
        }

        return await query.AnyAsync(r =>
            (startDate >= r.StartDate && startDate < r.EndDate) ||
            (endDate > r.StartDate && endDate <= r.EndDate) ||
            (startDate <= r.StartDate && endDate >= r.EndDate)
        );
    }
}
