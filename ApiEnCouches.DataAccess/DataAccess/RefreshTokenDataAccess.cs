namespace ApiEnCouches.DataAccess.DataAccess;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenDataAccess : IRefreshTokenDataAccess
{
    private readonly AppDbContext context;

    public RefreshTokenDataAccess(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<RefreshTokenModel> GetByUserId(int userId)
    {
        return await this.context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId);
    }

    public async Task<RefreshTokenModel> GetByToken(string token)
    {
        return await this.context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<RefreshTokenModel> Create(RefreshTokenModel refreshToken)
    {
        await this.context.RefreshTokens.AddAsync(refreshToken);
        await this.context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshTokenModel> Update(RefreshTokenModel refreshToken)
    {
        this.context.RefreshTokens.Update(refreshToken);
        await this.context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<bool> Delete(int userId)
    {
        var refreshToken = await this.context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId);

        if (refreshToken == null) return false;

        this.context.RefreshTokens.Remove(refreshToken);
        await this.context.SaveChangesAsync();
        return true;
    }
}
