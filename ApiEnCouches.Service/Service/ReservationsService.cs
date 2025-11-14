namespace ApiEnCouches.Service.Service;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using ApiEnCouches.Service.DataTransferObject.Reservation;
using ApiEnCouches.Service.IService;

public class ReservationsService : IReservationService
{
    private readonly IReservations reservationDataAccess;
    private readonly IMeetingRooms meetingRoomDataAccess;
    private readonly IUsers userDataAccess;

    public ReservationsService(
        IReservations reservationDataAccess,
        IMeetingRooms meetingRoomDataAccess,
        IUsers userDataAccess)
    {
        this.reservationDataAccess = reservationDataAccess;
        this.meetingRoomDataAccess = meetingRoomDataAccess;
        this.userDataAccess = userDataAccess;
    }

    public async Task<GetReservationDTO> GetById(int reservationId, int userId)
    {
        var reservation = await reservationDataAccess.GetById(reservationId);

        if (reservation == null) return null;

        if (reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Vous n'avez pas accès à cette réservation");
        }

        return MapToDTO(reservation);
    }

    public async Task<List<GetReservationDTO>> GetByUserId(int userId)
    {
        var reservations = await reservationDataAccess.GetByUserId(userId);

        return reservations.Select(MapToDTO).ToList();
    }

    public async Task<List<GetReservationDTO>> GetAll()
    {
        var reservations = await reservationDataAccess.GetAll();

        return reservations.Select(MapToDTO).ToList();
    }

    public async Task<GetReservationDTO> Create(CreateReservationDTO createReservationDto, int userId)
    {
        if (createReservationDto.EndDate <= createReservationDto.StartDate)
        {
            throw new Exception("La date de fin doit être postérieure à la date de début");
        }

        var room = await meetingRoomDataAccess.GetById(createReservationDto.RoomId);
        if (room == null)
        {
            throw new Exception("Salle de réunion non trouvée");
        }

        var user = await userDataAccess.GetById(userId);
        if (user == null)
        {
            throw new Exception("Utilisateur non trouvé");
        }

        var hasConflict = await reservationDataAccess.HasConflict(
            createReservationDto.RoomId,
            createReservationDto.StartDate,
            createReservationDto.EndDate
        );

        if (hasConflict)
        {
            throw new Exception("Cette salle est déjà réservée sur ce créneau horaire");
        }

        var reservation = new ReservationsModel
        {
            RoomId = createReservationDto.RoomId,
            UserId = userId,
            StartDate = createReservationDto.StartDate,
            EndDate = createReservationDto.EndDate
        };

        reservation = await reservationDataAccess.Create(reservation);

        return MapToDTO(reservation);
    }

    public async Task<GetReservationDTO> Update(
        int reservationId,
        UpdateReservationDTO updateReservationDto,
        int userId)
    {
        var reservation = await reservationDataAccess.GetById(reservationId);

        if (reservation == null)
        {
            throw new Exception("Réservation non trouvée");
        }

        if (reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Vous ne pouvez modifier que vos propres réservations");
        }

        if (updateReservationDto.RoomId.HasValue)
            reservation.RoomId = updateReservationDto.RoomId.Value;

        if (updateReservationDto.StartDate.HasValue)
            reservation.StartDate = updateReservationDto.StartDate.Value;

        if (updateReservationDto.EndDate.HasValue)
            reservation.EndDate = updateReservationDto.EndDate.Value;

        if (reservation.EndDate <= reservation.StartDate)
        {
            throw new Exception("La date de fin doit être postérieure à la date de début");
        }

        var hasConflict = await reservationDataAccess.HasConflict(
            reservation.RoomId,
            reservation.StartDate,
            reservation.EndDate,
            reservationId
        );

        if (hasConflict)
        {
            throw new Exception("Cette salle est déjà réservée sur ce créneau horaire");
        }

        reservation = await reservationDataAccess.Update(reservation);

        return MapToDTO(reservation);
    }

    public async Task<bool> Delete(int reservationId, int userId)
    {
        var reservation = await reservationDataAccess.GetById(reservationId);

        if (reservation == null)
        {
            throw new Exception("Réservation non trouvée");
        }

        if (reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Vous ne pouvez supprimer que vos propres réservations");
        }

        return await reservationDataAccess.Delete(reservationId);
    }

    private GetReservationDTO MapToDTO(ReservationsModel reservation)
    {
        return new GetReservationDTO
        {
            ReservationId = reservation.ReservationId,
            RoomId = reservation.RoomId,
            RoomName = reservation.MeetingRoom?.RoomName,
            UserId = reservation.UserId,
            UserEmail = reservation.User?.Email,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate
        };
    }
}
