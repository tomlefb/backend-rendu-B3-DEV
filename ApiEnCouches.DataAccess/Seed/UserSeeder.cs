namespace ApiEnCouches.DataAccess.Seed;

using ApiEnCouches.DataAccess.Models;
using BCrypt.Net;

public class UserSeeder : ISeeder
{
    public async Task Execute(AppDbContext db, bool isProduction)
    {
        if (isProduction) return;

        if (!db.Users.Any())
        {
            var users = new List<UsersModel>
            {
                new UsersModel
                {
                    FirstName = "Jean",
                    LastName = "Dupont",
                    Email = "jean.dupont@example.com",
                    Password = BCrypt.HashPassword("Password123!@#")
                },
                new UsersModel
                {
                    FirstName = "Marie",
                    LastName = "Martin",
                    Email = "marie.martin@example.com",
                    Password = BCrypt.HashPassword("SecurePass123!@#")
                },
                new UsersModel
                {
                    FirstName = "Pierre",
                    LastName = "Durand",
                    Email = "pierre.durand@example.com",
                    Password = BCrypt.HashPassword("MyPassword123!@#")
                }
            };

            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();
        }
    }
}
