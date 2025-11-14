namespace ApiEnCouches.DataAccess.IDataAccess;

using ApiEnCouches.DataAccess.Models;

public interface IReservations
{
    Task<ReservationsModel> GetById(int reservationId);
    Task<List<ReservationsModel>> GetByUserId(int userId);
    Task<List<ReservationsModel>> GetAll();
    Task<ReservationsModel> Create(ReservationsModel reservation);
    Task<ReservationsModel> Update(ReservationsModel reservation);
    Task<bool> Delete(int reservationId);
    Task<bool> HasConflict(int roomId, DateTime startDate, DateTime endDate, int? excludeReservationId = null);
}
