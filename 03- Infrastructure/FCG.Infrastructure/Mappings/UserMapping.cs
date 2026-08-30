using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings;

public class UserMapping : BaseEntityMapping<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Password).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Salt).HasMaxLength(64).IsRequired();

        builder.HasIndex(c => c.Email).IsUnique();

        builder.HasOne(c => c.UserRole)
            .WithMany(c => c.Users)
            .HasForeignKey(c => c.UserRoleId);
    }
}
