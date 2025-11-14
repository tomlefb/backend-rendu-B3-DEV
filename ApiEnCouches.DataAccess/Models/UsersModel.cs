namespace ApiEnCouches.DataAccess.Models;

public class UsersModel
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<ReservationsModel> Reservations { get; set; }
    public RefreshTokenModel RefreshToken { get; set; }
}
