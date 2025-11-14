namespace ApiEnCouches.Service.IService;

using ApiEnCouches.Service.DataTransferObject.Auth;

public interface IAuthService
{
    Task<AuthResponseDTO> Register(RegisterDTO registerDto);
    Task<AuthResponseDTO> Login(LoginDTO loginDto);
    Task<AuthResponseDTO> RefreshToken(string refreshToken);
    Task<bool> Logout(int userId);
}
