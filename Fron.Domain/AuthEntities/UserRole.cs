using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fron.Domain.AuthEntities;
public class UserRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long UserId { get; set; }
    public Role Role { get; set; } = new Role();
    public User User { get; set; } = new User();
    public DateTime ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}
