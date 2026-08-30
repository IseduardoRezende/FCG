namespace FCG.Domain.Entities;

public class UserRole : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<User>? Users { get; set; }
}
