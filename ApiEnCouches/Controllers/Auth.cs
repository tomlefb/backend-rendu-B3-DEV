namespace ApiEnCouches.Controllers;

using ApiEnCouches.Service.DataTransferObject.Auth;
using ApiEnCouches.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class Auth : Controller
{
    private readonly IAuthService authService;

    public Auth(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost(Routes.AuthRoutes.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await authService.Register(registerDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost(Routes.AuthRoutes.Login)]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await authService.Login(loginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost(Routes.AuthRoutes.Refresh)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO refreshTokenDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await authService.RefreshToken(refreshTokenDto.RefreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost(Routes.AuthRoutes.Logout)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token invalide" });
            }

            int userId = int.Parse(userIdClaim);
            var result = await authService.Logout(userId);

            if (result)
                return Ok(new { message = "Déconnexion réussie" });
            else
                return BadRequest(new { message = "Échec de la déconnexion" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
