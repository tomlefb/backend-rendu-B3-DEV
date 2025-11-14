namespace ApiEnCouches.Service.IService;

using ApiEnCouches.Service.DataTransferObject.MeetingRoom;

public interface IMeetingRoomService
{
    Task<GetMeetingRoomDTO> GetById(int roomId);
    Task<List<GetMeetingRoomDTO>> GetAll();
    Task<GetMeetingRoomDTO> Create(CreateMeetingRoomDTO createRoomDto);
    Task<bool> Delete(int roomId);
    Task<AvailabilityDTO> GetAvailability(int roomId, DateTime? startDate = null, DateTime? endDate = null);
}
