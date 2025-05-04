namespace Fron.Domain.AuthEntities;
public class UserRole
{
    public long RoleId { get; set; }
    public long UserId { get; set; }
    public Role Role { get; set; } = new Role();
    public User User { get; set; } = new User();
    public DateTime ModifiedDate { get; set; }
}
