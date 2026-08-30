using FCG.Application.DTOs.Users;
using FCG.Application.Services;
using FCG.Application.Services.Interfaces;
using FCG.Application.Validators;
using FCG.Domain.Commons.Result;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Repositories;
using FCG.Domain.Security;
using Moq;

namespace FCG.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _passwordHasher.Setup(x => x.GenerateSalt()).Returns("salt");
        _passwordHasher.Setup(x => x.Hash(It.IsAny<string>(), It.IsAny<string>())).Returns("hashed");
        _passwordHasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        _tokenService.Setup(x => x.Generate(It.IsAny<ReadUserDto>())).Returns(new TokenDto
        {
            Token = "token",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        });

        _userService = new UserService(
            _userRepository.Object,
            _passwordHasher.Object,
            _tokenService.Object,
            new RegisterUserDtoValidator(),
            new LoginDtoValidator(),
            new UpdateUserDtoValidator(new Mock<IUserRoleRepository>().Object));
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Token_When_Credentials_Are_Valid()
    {
        _userRepository.Setup(x => x.GetByEmailAsync("user@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Id = 1,
                Email = "user@test.com",
                Password = "hashed",
                Salt = "salt",
                UserRole = new UserRole { Name = nameof(UserRoles.User) }
            });

        var result = await _userService.LoginAsync(new LoginDto { Email = "user@test.com", Password = "Abcdef1!" }, TestContext.Current.CancellationToken);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("token", result.Value!.Token);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_NotFound_When_Email_Does_Not_Exist()
    {
        _userRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _userService.LoginAsync(new LoginDto { Email = "missing@test.com", Password = "Abcdef1!" }, TestContext.Current.CancellationToken);

        Assert.IsType<NotFoundResult<TokenDto>>(result);
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Conflict_When_Email_Already_Exists()
    {
        _userRepository.Setup(x => x.ExistsByEmailAsync("user@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _userService.RegisterAsync(new RegisterUserDto
        {
            Name = "User",
            Email = "user@test.com",
            Password = "Abcdef1!"
        }, TestContext.Current.CancellationToken);

        Assert.IsType<ConflictResult<ReadUserDto>>(result);
    }

    [Fact]
    public async Task RegisterAsync_Should_Assign_User_Role_By_Default()
    {
        User? capturedUser = null;

        _userRepository.Setup(x => x.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, _) => capturedUser = user)
            .Returns(Task.CompletedTask);
        _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long id, CancellationToken _) => new User
            {
                Id = id,
                Name = "User",
                Email = "user@test.com",
                UserRoleId = (long)UserRoles.User,
                UserRole = new UserRole { Name = nameof(UserRoles.User) },
                CreatedAt = DateTime.UtcNow
            });

        var result = await _userService.RegisterAsync(new RegisterUserDto
        {
            Name = "User",
            Email = "user@test.com",
            Password = "Abcdef1!"
        }, TestContext.Current.CancellationToken);

        Assert.True(result.Success);
        Assert.NotNull(capturedUser);
        Assert.Equal((long)UserRoles.User, capturedUser!.UserRoleId);
    }
}
