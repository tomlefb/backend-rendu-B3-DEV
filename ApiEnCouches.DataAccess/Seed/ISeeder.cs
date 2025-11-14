namespace ApiEnCouches.DataAccess.Seed
{
    public interface ISeeder
    {
        Task Execute(AppDbContext appDbContext, bool isProduction);
    }
}