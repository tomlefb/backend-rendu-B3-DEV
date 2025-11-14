namespace ApiEnCouches.Service.DataTransferObject.Auth;

public class AuthResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
}
