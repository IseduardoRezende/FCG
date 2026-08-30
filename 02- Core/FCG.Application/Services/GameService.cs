using FCG.Application.DTOs.Games;
using FCG.Application.Extensions;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FluentValidation;

namespace FCG.Application.Services.Interfaces;

public interface IGameService
{
    Task<Result<ReadGameDto>> CreateAsync(CreateGameDto dto, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ReadGameDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<ReadGameDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Result<ReadGameDto>> UpdateAsync(long id, UpdateGameDto dto, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IValidator<CreateGameDto> _createValidator;
    private readonly IValidator<UpdateGameDto> _updateValidator;

    public GameService(
        IGameRepository gameRepository,
        IValidator<CreateGameDto> createValidator,
        IValidator<UpdateGameDto> updateValidator)
    {
        _gameRepository = gameRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<ReadGameDto>> CreateAsync(CreateGameDto dto, CancellationToken cancellationToken = default)
    {
        var validation = (await _createValidator.ValidateAsync(dto, cancellationToken)).ToInvalidResult<ReadGameDto>();
        if (validation is not null)
        {
            return validation;
        }

        var game = new Game
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _gameRepository.AddAsync(game, cancellationToken);
        await _gameRepository.SaveChangesAsync(cancellationToken);

        var created = await _gameRepository.GetByIdAsync(game.Id, cancellationToken);
        return SuccessResult<ReadGameDto>.Create(MapToReadDto(created!));
    }

    public async Task<Result<IReadOnlyList<ReadGameDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var games = await _gameRepository.GetAllAsync(cancellationToken);
        var result = games.Select(MapToReadDto).ToList();
        return SuccessResult<IReadOnlyList<ReadGameDto>>.Create(result);
    }

    public async Task<Result<ReadGameDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(id, cancellationToken);
        if (game is null)
        {
            return NotFoundResult<ReadGameDto>.Create(new Error("Game not found."));
        }

        return SuccessResult<ReadGameDto>.Create(MapToReadDto(game));
    }

    public async Task<Result<ReadGameDto>> UpdateAsync(long id, UpdateGameDto dto, CancellationToken cancellationToken = default)
    {
        var validation = (await _updateValidator.ValidateAsync(dto, cancellationToken)).ToInvalidResult<ReadGameDto>();
        if (validation is not null)
        {
            return validation;
        }

        var game = await _gameRepository.GetByIdAsync(id, cancellationToken);
        if (game is null)
        {
            return NotFoundResult<ReadGameDto>.Create(new Error("Game not found."));
        }

        game.Name = dto.Name.Trim();
        game.Description = dto.Description.Trim();
        game.Price = dto.Price;

        await _gameRepository.UpdateAsync(game, cancellationToken);
        await _gameRepository.SaveChangesAsync(cancellationToken);

        var updated = await _gameRepository.GetByIdAsync(id, cancellationToken);
        return SuccessResult<ReadGameDto>.Create(MapToReadDto(updated!));
    }

    public async Task<Result<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(id, cancellationToken);
        if (game is null)
        {
            return NotFoundResult<bool>.Create(new Error("Game not found."));
        }

        game.Delete();
        await _gameRepository.UpdateAsync(game, cancellationToken);
        await _gameRepository.SaveChangesAsync(cancellationToken);

        return SuccessResult<bool>.Create(true);
    }

    private static ReadGameDto MapToReadDto(Game game)
    {
        return new ReadGameDto
        {
            Id = game.Id,
            Name = game.Name,
            Description = game.Description,
            Price = game.Price,
            CreatedAt = game.CreatedAt
        };
    }
}
