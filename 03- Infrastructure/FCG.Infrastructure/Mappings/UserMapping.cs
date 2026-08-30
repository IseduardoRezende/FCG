using FCG.Domain.Entities;
using FCG.Domain.Enums;
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

        builder.HasIndex(c => c.Email);

        builder.HasOne(c => c.UserRole)
            .WithMany(c => c.Users)
            .HasForeignKey(c => c.UserRoleId);

        builder.HasData(
            new User
            {
                Id = -1,
                Name = "Admin",
                Email = "fcg@admin.com",
                UserRoleId = (long)UserRoles.Administrator,
                Password = "OxFPEaZRtmloJcAIHMItyJfep3S4tc5/ViQaZxtiiDQ=",
                Salt = "7f3c9a2e1b4d6f8a0c2e4a6b8d0f2a4c",
                CreatedAt = new DateTime(2026, 08, 30, 00, 00, 00, DateTimeKind.Utc),
                IsDeleted = false,
            }
        );
    }
}
