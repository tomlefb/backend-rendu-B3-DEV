namespace ApiEnCouches.Service.DataTransferObject.User;

using System.ComponentModel.DataAnnotations;

public class UpdateUserDTO
{
    [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
    public string FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    public string LastName { get; set; }

    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    [StringLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères")]
    public string Email { get; set; }
}
