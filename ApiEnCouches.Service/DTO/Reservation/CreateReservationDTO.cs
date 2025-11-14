namespace ApiEnCouches.Service.DataTransferObject.Reservation;

using System.ComponentModel.DataAnnotations;

public class CreateReservationDTO
{
    [Required(ErrorMessage = "L'ID de la salle est obligatoire")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "La date de début est obligatoire")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La date de fin est obligatoire")]
    public DateTime EndDate { get; set; }
}
