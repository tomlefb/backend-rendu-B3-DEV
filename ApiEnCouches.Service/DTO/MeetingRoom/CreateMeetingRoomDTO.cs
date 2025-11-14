namespace ApiEnCouches.Service.DataTransferObject.MeetingRoom;

using System.ComponentModel.DataAnnotations;

public class CreateMeetingRoomDTO
{
    [Required(ErrorMessage = "Le nom de la salle est obligatoire")]
    [StringLength(200, ErrorMessage = "Le nom de la salle ne peut pas dépasser 200 caractères")]
    public string RoomName { get; set; }

    [Required(ErrorMessage = "La capacité est obligatoire")]
    [Range(1, 1000, ErrorMessage = "La capacité doit être entre 1 et 1000 personnes")]
    public int Capacity { get; set; }
}
