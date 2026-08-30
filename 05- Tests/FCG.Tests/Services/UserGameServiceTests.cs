using FCG.Application.Abstractions;
using FCG.Application.DTOs.UserGames;
using FCG.Application.Services.Interfaces;
using FCG.Application.Validators;
using FCG.Domain.Commons.Result;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Services;

public class UserGameServiceTests
{
    private readonly Mock<IUserGameRepository> _userGameRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IGameRepository> _gameRepository = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly UserGameService _userGameService;

    public UserGameServiceTests()
    {
        _currentUser.Setup(x => x.UserId).Returns(1);
        _currentUser.Setup(x => x.IsAdministrator).Returns(false);

        _userGameService = new UserGameService(
            _userGameRepository.Object,
            _userRepository.Object,
            _gameRepository.Object,
            _currentUser.Object,
            new CreateUserGameDtoValidator());
    }

    [Fact]
    public async Task PurchaseAsync_Should_Succeed_When_Game_Is_Available()
    {
        _userRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 1, Email = "user@test.com" });
        _gameRepository.Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Game { Id = 10, Name = "Game", Price = 99.9m });
        _userGameRepository.Setup(x => x.ExistsAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _userGameRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long id, CancellationToken _) => new UserGame
            {
                Id = id,
                UserId = 1,
                GameId = 10,
                PurchasedAt = DateTime.UtcNow,
                Game = new Game { Name = "Game", Price = 99.9m }
            });

        var result = await _userGameService.PurchaseAsync(new CreateUserGameDto { GameId = 10 }, TestContext.Current.CancellationToken);

        Assert.True(result.Success);
        Assert.Equal("Game", result.Value!.GameName);
    }

    [Fact]
    public async Task PurchaseAsync_Should_Return_Conflict_When_Game_Already_Purchased()
    {
        _userRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 1 });
        _gameRepository.Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Game { Id = 10, Name = "Game" });
        _userGameRepository.Setup(x => x.ExistsAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _userGameService.PurchaseAsync(new CreateUserGameDto { GameId = 10 }, TestContext.Current.CancellationToken);

        Assert.IsType<ConflictResult<FCG.Application.DTOs.UserGames.ReadUserGameDto>>(result);
    }
}
