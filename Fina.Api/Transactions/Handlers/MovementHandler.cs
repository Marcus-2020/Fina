using Fina.Api.Common.Data;
using Fina.Core.Common.Requests;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Movement;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Transactions.Handlers;

public class MovementHandler : IMovementHandler
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<MovementHandler>();
    private readonly AppDbContext _context;

    public MovementHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Movement?>> CreateAsync(CreateMovementRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Adicionando movimento à transação {TransactionId}", request.TransactionId);

            var movement = new Movement
            {
                TransactionId = request.TransactionId,
                AccountId = request.AccountId,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt
            };

            _context.Movements.Add(movement);

            await _context.SaveChangesAsync();

            _logger.Information("Movimento adicionado à transação {TransactionId} com sucesso", request.TransactionId);

            return new Response<Movement?>(movement,
                StatusCodes.Status200OK,
                "Movimento adicionado à transação com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao adicionar movimento à transação {TransactionId}", request.TransactionId);
            return new Response<Movement?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao adicionar movimento à transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Movement?>> UpdateAsync(UpdateMovementRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Atualizando movimento {Id}", request.Id);

            var movement = await _context.Movements.FindAsync(request.Id);

            if (movement == null)
            {
                _logger.Warning("Movimento {Id} não encontrado", request.Id);
                return new Response<Movement?>(null,
                    StatusCodes.Status404NotFound,
                    "Movimento não encontrado");
            }
            
            if (movement.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Movement>(request.Id, request);
            }

            movement.TransactionId = request.TransactionId;
            movement.AccountId = request.AccountId;
            movement.Amount = request.Amount;
            movement.PaidOrReceivedAt = request.PaidOrReceivedAt;

            await _context.SaveChangesAsync();

            _logger.Information("Movimento {Id} atualizado com sucesso", request.Id);

            return new Response<Movement?>(movement,
                StatusCodes.Status200OK,
                "Movimento atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao atualizar o movimento {Id}", request.Id);
            return new Response<Movement?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao atualizar o movimento");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Movement?>> DeleteAsync(DeleteMovementRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Deletando movimento {Id}", request.Id);

            var movement = await _context.Movements.FindAsync(request.Id);

            if (movement == null)
            {
                _logger.Warning("Movimento {Id} não encontrado", request.Id);
                return new Response<Movement?>(null,
                    StatusCodes.Status404NotFound,
                    "Movimento não encontrado.");
            }
            
            if (movement.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Movement>(request.Id, request);
            }

            _context.Movements.Remove(movement);

            await _context.SaveChangesAsync();

            _logger.Information("Movimento {Id} deletado com sucesso", request.Id);

            return new Response<Movement?>(movement,
                StatusCodes.Status200OK,
                "Movimento deletado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao deletar o movimento {Id}", request.Id);
            return new Response<Movement?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao deletar o movimento");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Movement?>> GetByIdAsync(GetMovementByIdRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando movimento {Id}", request.Id);

            var movement = await _context.Movements
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            if (movement == null)
            {
                _logger.Warning("Movimento {Id} não encontrado", request.Id);
                return new Response<Movement?>(null,
                    StatusCodes.Status404NotFound,
                    "Movimento não encontrado.");
            }
            
            if (movement.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Movement>(request.Id, request);
            }

            _logger.Information("Movimento {Id} encontrado com sucesso", request.Id);

            return new Response<Movement?>(movement,
                StatusCodes.Status200OK,
                "Movimento encontrado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar o movimento {Id}", request.Id);
            return new Response<Movement?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao buscar o movimento");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<PagedResponse<List<Movement>>> GetByTransactionIdAsync(GetMovementsByTransactionIdRequest request)
    {
        var logger = _logger.ForContext("Request", request);
        try
        {
            _logger.Information("Buscando movimentos da transação {TransactionId}", request.TransactionId);
            
            var movements = await _context.Movements
                .Where(m => m.UserId == request.UserId)
                .Where(m => m.TransactionId == request.TransactionId)
                .Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();
            
            _logger.Information("Movimentos da transação {TransactionId} encontrados com sucesso", request.TransactionId);

            return new PagedResponse<List<Movement>>(movements,
                StatusCodes.Status200OK,
                "Movimentos encontrados com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar movimentos da transação {TransactionId}", request.TransactionId);
            return new PagedResponse<List<Movement>>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao buscar movimentos da transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }
    
    private Response<T?> ErrorNotFromTheUser<T>(long movementId, Request request) where T : class
    {
        _logger.Warning("Movimento {Id} não pertence ao usuário {UserId}", movementId, request.UserId);
        return new Response<T?>(null,
            StatusCodes.Status403Forbidden,
            "Movimento não pertence ao usuário.");
    }
}