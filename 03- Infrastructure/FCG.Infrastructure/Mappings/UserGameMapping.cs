using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings;

public class UserGameMapping : BaseEntityMapping<UserGame>
{
    public override void Configure(EntityTypeBuilder<UserGame> builder)
    {
        base.Configure(builder);

        builder.HasIndex(c => new { c.UserId, c.GameId }).IsUnique();

        builder.HasOne(c => c.User)
            .WithMany(c => c.UserGames)
            .HasForeignKey(c => c.UserId);

        builder.HasOne(c => c.Game)
            .WithMany(c => c.UserGames)
            .HasForeignKey(c => c.GameId);
    }
}
