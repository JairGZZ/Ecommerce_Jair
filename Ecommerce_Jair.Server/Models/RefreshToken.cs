namespace Ecommerce_Jair.Server.Models;

public partial class RefreshToken
{ 
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool Revoked { get; set; } = false;

    public virtual User User { get; set; } = null!;
}
