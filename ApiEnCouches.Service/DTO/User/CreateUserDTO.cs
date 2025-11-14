namespace ApiEnCouches.Service.DataTransferObject.User;

using System.ComponentModel.DataAnnotations;

public class CreateUserDTO
{
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    [StringLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [StringLength(100, MinimumLength = 12, ErrorMessage = "Le mot de passe doit contenir au moins 12 caractères")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$",
        ErrorMessage = "Le mot de passe doit contenir au moins 12 caractères, 1 majuscule, 1 minuscule, 1 chiffre et 1 caractère spécial")]
    public string Password { get; set; }
}
