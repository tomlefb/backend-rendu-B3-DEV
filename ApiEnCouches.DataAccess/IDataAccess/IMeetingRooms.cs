namespace ApiEnCouches.DataAccess.IDataAccess;

using ApiEnCouches.DataAccess.Models;

public interface IMeetingRooms
{
    Task<MeetingRoomsModel> GetById(int roomId);
    Task<List<MeetingRoomsModel>> GetAll();
    Task<MeetingRoomsModel> Create(MeetingRoomsModel room);
    Task<bool> Delete(int roomId);
    Task<List<ReservationsModel>> GetReservationsByRoomId(int roomId);
}
