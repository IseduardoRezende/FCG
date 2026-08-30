using FCG.Application.DTOs.Users;

namespace FCG.Application.Services.Interfaces;

public interface ITokenService
{
    TokenDto Generate(ReadUserDto user);
}
