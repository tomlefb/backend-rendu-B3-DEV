namespace ApiEnCouches.DataAccess.Models;

public class RefreshTokenModel
{
    public int Id { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiryDate { get; set; }

    public UsersModel User { get; set; }
}
