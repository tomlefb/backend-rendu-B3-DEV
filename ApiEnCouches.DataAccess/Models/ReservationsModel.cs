namespace ApiEnCouches.DataAccess.Models;

public class ReservationsModel
{
    public int ReservationId { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public MeetingRoomsModel MeetingRoom { get; set; }
    public UsersModel User { get; set; }
}
