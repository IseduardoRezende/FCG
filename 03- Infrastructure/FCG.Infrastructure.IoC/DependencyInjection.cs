using FCG.Application.Services;
using FCG.Application.Services.Interfaces;
using FCG.Application.Validators;
using FCG.Domain.Repositories;
using FCG.Domain.Security;
using FCG.Infrastructure.Repositories;
using FCG.Infrastructure.Security;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IUserGameRepository, UserGameRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IUserGameService, UserGameService>();

        services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

        return services;
    }
}
