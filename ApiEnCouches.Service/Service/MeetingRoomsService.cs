namespace ApiEnCouches.Service.Service;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using ApiEnCouches.Service.DataTransferObject.MeetingRoom;
using ApiEnCouches.Service.IService;

public class MeetingRoomsService : IMeetingRoomService
{
    private readonly IMeetingRooms meetingRoomDataAccess;

    public MeetingRoomsService(IMeetingRooms meetingRoomDataAccess)
    {
        this.meetingRoomDataAccess = meetingRoomDataAccess;
    }

    public async Task<GetMeetingRoomDTO> GetById(int roomId)
    {
        var room = await meetingRoomDataAccess.GetById(roomId);
        if (room == null) return null;

        return new GetMeetingRoomDTO
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            Capacity = room.Capacity
        };
    }

    public async Task<List<GetMeetingRoomDTO>> GetAll()
    {
        var rooms = await meetingRoomDataAccess.GetAll();

        return rooms.Select(r => new GetMeetingRoomDTO
        {
            RoomId = r.RoomId,
            RoomName = r.RoomName,
            Capacity = r.Capacity
        }).ToList();
    }

    public async Task<GetMeetingRoomDTO> Create(CreateMeetingRoomDTO createRoomDto)
    {
        var room = new MeetingRoomsModel
        {
            RoomName = createRoomDto.RoomName,
            Capacity = createRoomDto.Capacity
        };

        room = await meetingRoomDataAccess.Create(room);

        return new GetMeetingRoomDTO
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            Capacity = room.Capacity
        };
    }

    public async Task<bool> Delete(int roomId)
    {
        return await meetingRoomDataAccess.Delete(roomId);
    }

    public async Task<AvailabilityDTO> GetAvailability(int roomId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var room = await meetingRoomDataAccess.GetById(roomId);
        if (room == null)
        {
            throw new Exception("Salle de réunion non trouvée");
        }

        var reservations = await meetingRoomDataAccess.GetReservationsByRoomId(roomId);

        startDate ??= DateTime.UtcNow.Date;
        endDate ??= DateTime.UtcNow.Date.AddDays(30);

        var availableSlots = CalculateAvailableSlots(reservations, startDate.Value, endDate.Value);

        return new AvailabilityDTO
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            AvailableSlots = availableSlots
        };
    }

    private List<AvailableSlotDTO> CalculateAvailableSlots(
        List<ReservationsModel> reservations,
        DateTime startDate,
        DateTime endDate)
    {
        var availableSlots = new List<AvailableSlotDTO>();

        if (!reservations.Any())
        {
            availableSlots.Add(new AvailableSlotDTO
            {
                StartDate = startDate,
                EndDate = endDate
            });
            return availableSlots;
        }

        var sortedReservations = reservations
            .Where(r => r.EndDate > startDate && r.StartDate < endDate)
            .OrderBy(r => r.StartDate)
            .ToList();

        if (!sortedReservations.Any())
        {
            availableSlots.Add(new AvailableSlotDTO
            {
                StartDate = startDate,
                EndDate = endDate
            });
            return availableSlots;
        }

        if (sortedReservations.First().StartDate > startDate)
        {
            availableSlots.Add(new AvailableSlotDTO
            {
                StartDate = startDate,
                EndDate = sortedReservations.First().StartDate
            });
        }

        for (int i = 0; i < sortedReservations.Count - 1; i++)
        {
            if (sortedReservations[i].EndDate < sortedReservations[i + 1].StartDate)
            {
                availableSlots.Add(new AvailableSlotDTO
                {
                    StartDate = sortedReservations[i].EndDate,
                    EndDate = sortedReservations[i + 1].StartDate
                });
            }
        }

        if (sortedReservations.Last().EndDate < endDate)
        {
            availableSlots.Add(new AvailableSlotDTO
            {
                StartDate = sortedReservations.Last().EndDate,
                EndDate = endDate
            });
        }

        return availableSlots;
    }
}
