using FCG.Application.DTOs.UserGames;
using FluentValidation;

namespace FCG.Application.Validators;

public class CreateUserGameDtoValidator : AbstractValidator<CreateUserGameDto>
{
    public CreateUserGameDtoValidator()
    {
        RuleFor(x => x.GameId).GreaterThan(0);
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .When(x => x.UserId.HasValue);
    }
}
