namespace ApiEnCouches.Service.DataTransferObject.MeetingRoom;

public class AvailabilityDTO
{
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public int Capacity { get; set; }
    public List<AvailableSlotDTO> AvailableSlots { get; set; }
}

public class AvailableSlotDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
