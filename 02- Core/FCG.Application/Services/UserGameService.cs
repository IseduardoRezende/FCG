using FCG.Application.Abstractions;
using FCG.Application.DTOs.UserGames;
using FCG.Application.Extensions;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Entities;
using FCG.Domain.Filters;
using FCG.Domain.Repositories;
using FluentValidation;

namespace FCG.Application.Services;

public class UserGameService : IUserGameService
{
    private readonly IUserGameRepository _userGameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IValidator<CreateUserGameDto> _validator;

    public UserGameService(
        IUserGameRepository userGameRepository,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        ICurrentUser currentUser,
        IValidator<CreateUserGameDto> validator)
    {
        _userGameRepository = userGameRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<Result<ReadUserGameDto>> PurchaseAsync(CreateUserGameDto dto, CancellationToken cancellationToken = default)
    {
        var validation = (await _validator.ValidateAsync(dto, cancellationToken)).ToInvalidResult<ReadUserGameDto>();

        if (validation is not null)        
            return validation;        

        var targetUserId = ResolveTargetUserId(dto.UserId);

        if (targetUserId is null)        
            return InvalidResult<ReadUserGameDto>.Create(new Error("User id is required."));        

        if (dto.UserId.HasValue && !_currentUser.IsAdministrator)        
            return InvalidResult<ReadUserGameDto>.Create(new Error("Only administrators can purchase games for other users."));        

        var user = await _userRepository.GetByIdAsync(targetUserId.Value, cancellationToken);
        
        if (user is null)        
            return NotFoundResult<ReadUserGameDto>.Create(new Error("User not found."));        

        var game = await _gameRepository.GetByIdAsync(dto.GameId, cancellationToken);
        
        if (game is null)        
            return NotFoundResult<ReadUserGameDto>.Create(new Error("Game not found."));        

        if (await _userGameRepository.ExistsAsync(targetUserId.Value, dto.GameId, cancellationToken))        
            return ConflictResult<ReadUserGameDto>.Create(new Error("Game already purchased."));        

        var userGame = new UserGame
        {
            UserId = targetUserId.Value,
            GameId = dto.GameId,
            PurchasedAt = DateTime.UtcNow
        };

        await _userGameRepository.AddAsync(userGame, cancellationToken);
        await _userGameRepository.SaveChangesAsync(cancellationToken);

        var created = await _userGameRepository.GetByIdAsync(userGame.Id, cancellationToken);
        return SuccessResult<ReadUserGameDto>.Create(MapToReadDto(created!));
    }

    public async Task<Result<Pagination<ReadUserGameDto>>> GetLibraryAsync(UserGameFilter filter, CancellationToken cancellationToken = default)
    {
        var targetUserId = ResolveLibraryUserId(filter.UserId);
        
        if (targetUserId is null)        
            return InvalidResult<Pagination<ReadUserGameDto>>.Create(new Error("User id is required."));        

        if (filter.UserId.HasValue && filter.UserId.Value != _currentUser.UserId && !_currentUser.IsAdministrator)        
            return InvalidResult<Pagination<ReadUserGameDto>>.Create(new Error("You can only access your own library."));        

        filter.UserId = targetUserId;

        var (items, totalCount) = await _userGameRepository.GetPagedAsync(filter, cancellationToken);
        var mapped = items.Select(MapToReadDto).ToList();

        return SuccessResult<Pagination<ReadUserGameDto>>.Create(
            new Pagination<ReadUserGameDto>(mapped, totalCount, filter.CurrentPage, filter.PageSize));
    }

    public async Task<Result<ReadUserGameDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var userGame = await _userGameRepository.GetByIdAsync(id, cancellationToken);
        
        if (userGame is null)        
            return NotFoundResult<ReadUserGameDto>.Create(new Error("Purchase not found."));        

        if (userGame.UserId != _currentUser.UserId && !_currentUser.IsAdministrator)        
            return InvalidResult<ReadUserGameDto>.Create(new Error("You can only access your own library."));        

        return SuccessResult<ReadUserGameDto>.Create(MapToReadDto(userGame));
    }

    private long? ResolveTargetUserId(long? requestedUserId)
    {
        if (requestedUserId.HasValue)        
            return requestedUserId.Value;        

        return _currentUser.UserId;
    }

    private long? ResolveLibraryUserId(long? requestedUserId)
    {
        if (_currentUser.IsAdministrator && requestedUserId.HasValue)        
            return requestedUserId.Value;        

        return _currentUser.UserId;
    }

    private static ReadUserGameDto MapToReadDto(UserGame userGame)
    {
        return new ReadUserGameDto
        {
            Id = userGame.Id,
            UserId = userGame.UserId,
            GameId = userGame.GameId,
            GameName = userGame.Game?.Name ?? string.Empty,
            GamePrice = userGame.Game?.Price ?? 0,
            PurchasedAt = userGame.PurchasedAt
        };
    }
}
