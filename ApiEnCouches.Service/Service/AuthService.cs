namespace ApiEnCouches.Service.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using ApiEnCouches.Service.DataTransferObject.Auth;
using ApiEnCouches.Service.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

public class AuthService : IAuthService
{
    private readonly IUsers userDataAccess;
    private readonly IRefreshTokenDataAccess refreshTokenDataAccess;
    private readonly IConfiguration configuration;

    public AuthService(
        IUsers userDataAccess,
        IRefreshTokenDataAccess refreshTokenDataAccess,
        IConfiguration configuration)
    {
        this.userDataAccess = userDataAccess;
        this.refreshTokenDataAccess = refreshTokenDataAccess;
        this.configuration = configuration;
    }

    public async Task<AuthResponseDTO> Register(RegisterDTO registerDto)
    {
        if (await userDataAccess.EmailExists(registerDto.Email))
        {
            throw new Exception("Un utilisateur avec cet email existe déjà");
        }

        var user = new UsersModel
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Password = BCrypt.HashPassword(registerDto.Password)
        };

        user = await userDataAccess.Create(user);

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateAndSaveRefreshToken(user.UserId);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.UserId,
            Email = user.Email
        };
    }

    public async Task<AuthResponseDTO> Login(LoginDTO loginDto)
    {
        var user = await userDataAccess.GetByEmail(loginDto.Email);

        if (user == null || !BCrypt.Verify(loginDto.Password, user.Password))
        {
            throw new Exception("Email ou mot de passe incorrect");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateAndSaveRefreshToken(user.UserId);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.UserId,
            Email = user.Email
        };
    }

    public async Task<AuthResponseDTO> RefreshToken(string refreshToken)
    {
        var storedToken = await refreshTokenDataAccess.GetByToken(refreshToken);

        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new Exception("Refresh token invalide ou expiré");
        }

        var user = storedToken.User;
        if (user == null)
        {
            user = await userDataAccess.GetById(storedToken.UserId);
        }

        var accessToken = GenerateAccessToken(user);
        var newRefreshToken = await GenerateAndSaveRefreshToken(user.UserId);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            UserId = user.UserId,
            Email = user.Email
        };
    }

    public async Task<bool> Logout(int userId)
    {
        return await refreshTokenDataAccess.Delete(userId);
    }

    private string GenerateAccessToken(UsersModel user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:AccessTokenExpirationMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateAndSaveRefreshToken(int userId)
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        var token = Convert.ToBase64String(randomBytes);

        var existingToken = await refreshTokenDataAccess.GetByUserId(userId);
        if (existingToken != null)
        {
            existingToken.Token = token;
            existingToken.ExpiryDate = DateTime.UtcNow.AddDays(
                Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationDays"]));
            await refreshTokenDataAccess.Update(existingToken);
        }
        else
        {
            var refreshToken = new RefreshTokenModel
            {
                Token = token,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(
                    Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationDays"]))
            };
            await refreshTokenDataAccess.Create(refreshToken);
        }

        return token;
    }
}
