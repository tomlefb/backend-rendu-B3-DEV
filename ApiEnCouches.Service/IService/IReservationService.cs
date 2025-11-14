namespace ApiEnCouches.Service.IService;

using ApiEnCouches.Service.DataTransferObject.Reservation;

public interface IReservationService
{
    Task<GetReservationDTO> GetById(int reservationId, int userId);
    Task<List<GetReservationDTO>> GetByUserId(int userId);
    Task<List<GetReservationDTO>> GetAll();
    Task<GetReservationDTO> Create(CreateReservationDTO createReservationDto, int userId);
    Task<GetReservationDTO> Update(int reservationId, UpdateReservationDTO updateReservationDto, int userId);
    Task<bool> Delete(int reservationId, int userId);
}
