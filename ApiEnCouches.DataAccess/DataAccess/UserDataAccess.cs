namespace ApiEnCouches.DataAccess.DataAccess;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using Microsoft.EntityFrameworkCore;

public class UserDataAccess : IUsers
{
    private readonly AppDbContext context;

    public UserDataAccess(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<UsersModel> GetById(int userId)
    {
        return await this.context.Users
            .Include(u => u.Reservations)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<UsersModel> GetByEmail(string email)
    {
        return await this.context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<UsersModel>> GetAll()
    {
        return await this.context.Users
            .Include(u => u.Reservations)
            .ToListAsync();
    }

    public async Task<UsersModel> Create(UsersModel user)
    {
        await this.context.Users.AddAsync(user);
        await this.context.SaveChangesAsync();
        return user;
    }

    public async Task<UsersModel> Update(UsersModel user)
    {
        this.context.Users.Update(user);
        await this.context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> Delete(int userId)
    {
        var user = await this.context.Users.FindAsync(userId);
        if (user == null) return false;

        this.context.Users.Remove(user);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmailExists(string email)
    {
        return await this.context.Users.AnyAsync(u => u.Email == email);
    }
}
