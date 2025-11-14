namespace ApiEnCouches.DataAccess.Models;

public class MeetingRoomsModel
{
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public int Capacity { get; set; }

    public ICollection<ReservationsModel> Reservations { get; set; }
}
