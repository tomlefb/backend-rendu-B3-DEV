namespace ApiEnCouches.Service.DataTransferObject.Auth;

using System.ComponentModel.DataAnnotations;

public class RefreshTokenDTO
{
    [Required(ErrorMessage = "Le refresh token est obligatoire")]
    public string RefreshToken { get; set; }
}
