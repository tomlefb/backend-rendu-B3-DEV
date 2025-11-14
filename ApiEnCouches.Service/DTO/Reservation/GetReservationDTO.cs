namespace ApiEnCouches.Service.DataTransferObject.Reservation;

public class GetReservationDTO
{
    public int ReservationId { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public int UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
