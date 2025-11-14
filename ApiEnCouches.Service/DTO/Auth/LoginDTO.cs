namespace ApiEnCouches.Service.DataTransferObject.Auth;

using System.ComponentModel.DataAnnotations;

public class LoginDTO
{
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; }
}
