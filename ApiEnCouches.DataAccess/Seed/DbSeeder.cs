namespace ApiEnCouches.DataAccess.Seed
{
    public class DbSeeder
    {
        private IEnumerable<ISeeder> seeders;

        public DbSeeder(IEnumerable<ISeeder> seeders)
        {
            this.seeders = seeders;
        }

        public async Task Execute(AppDbContext dbContext, bool isProduction)
        {
            foreach (var seeder in seeders)
            {
                await seeder.Execute(dbContext, isProduction);
            }
        }
    }
}
