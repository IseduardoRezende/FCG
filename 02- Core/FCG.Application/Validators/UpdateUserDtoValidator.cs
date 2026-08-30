using FCG.Application.DTOs.Users;
using FCG.Domain.Repositories;
using FluentValidation;

namespace FCG.Application.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator(IUserRoleRepository userRoleRepository)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.UserRoleId)
            .GreaterThan(0)
            .MustAsync(async (roleId, cancellationToken) =>
                await userRoleRepository.ExistsAsync(roleId, cancellationToken))
            .WithMessage("User role does not exist.");
    }
}
