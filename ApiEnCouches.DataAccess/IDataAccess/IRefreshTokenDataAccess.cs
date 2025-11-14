namespace ApiEnCouches.DataAccess.IDataAccess;

using ApiEnCouches.DataAccess.Models;

public interface IRefreshTokenDataAccess
{
    Task<RefreshTokenModel> GetByUserId(int userId);
    Task<RefreshTokenModel> GetByToken(string token);
    Task<RefreshTokenModel> Create(RefreshTokenModel refreshToken);
    Task<RefreshTokenModel> Update(RefreshTokenModel refreshToken);
    Task<bool> Delete(int userId);
}
