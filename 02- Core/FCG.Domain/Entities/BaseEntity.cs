namespace FCG.Domain.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }

    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }
}
