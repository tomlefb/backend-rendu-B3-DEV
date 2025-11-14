namespace ApiEnCouches.DataAccess
{
    using ApiEnCouches.DataAccess.Models;

    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<MeetingRoomsModel> MeetingRooms { get; set; }
        public DbSet<ReservationsModel> Reservations { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsersModel>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Password).IsRequired();
            });

            modelBuilder.Entity<MeetingRoomsModel>(entity =>
            {
                entity.HasKey(m => m.RoomId);
                entity.Property(m => m.RoomName).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Capacity).IsRequired();
            });

            modelBuilder.Entity<ReservationsModel>(entity =>
            {
                entity.HasKey(r => r.ReservationId);
                entity.Property(r => r.StartDate).IsRequired();
                entity.Property(r => r.EndDate).IsRequired();

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.MeetingRoom)
                    .WithMany(m => m.Reservations)
                    .HasForeignKey(r => r.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshTokenModel>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
                entity.Property(rt => rt.ExpiryDate).IsRequired();

                entity.HasOne(rt => rt.User)
                    .WithOne(u => u.RefreshToken)
                    .HasForeignKey<RefreshTokenModel>(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}