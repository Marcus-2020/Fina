using Fina.Api.Common.Data;
using Fina.Core.Common.Requests;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Transactions.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<TransactionHandler>();
    private readonly AppDbContext _context;

    public TransactionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Criando transação {Name}", request.Title);

            var transaction = new Transaction
            {
                Title = request.Title,
                Type = request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                PaidAmount = 0.0M,
                UserId = request.UserId
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            _logger.Information("Transação {Name} criada com sucesso", request.Title);

            return new Response<Transaction?>(transaction,
                StatusCodes.Status200OK,
                "Transação criada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Erro ao criar a transação {Name}", request.Title);
            return new Response<Transaction?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao criar a transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Atualizando transação {Id}", request.Id);

            var transaction = await _context.Transactions.FindAsync(request.Id);

            if (transaction is null)
            {
                return new Response<Transaction?>(null,
                    StatusCodes.Status404NotFound,
                    "Transação não encontrada.");
            }
            
            if (transaction.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Transaction>(request.Id, request);
            }
            
            transaction.Title = request.Title;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.Type = request.Type;
            
            await _context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction,
                StatusCodes.Status200OK,
                "Transação atualizada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Erro ao atualizar a transação {Id}", request.Id);
            return new Response<Transaction?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao atualizar a transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Deletando transação {Id}", request.Id);

            var transaction = await _context.Transactions.FindAsync(request.Id);

            if (transaction is null)
            {
                return new Response<Transaction?>(null,
                    StatusCodes.Status404NotFound,
                    "Transação não encontrada.");
            }
            
            if (transaction.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Transaction>(request.Id, request);
            }

            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction,
                StatusCodes.Status200OK,
                "Transação deletada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Erro ao deletar a transação {Id}", request.Id);
            return new Response<Transaction?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao deletar a transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando transação {Id}", request.Id);

            var transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id);

            if (transaction is null)
            {
                return new Response<Transaction?>(null,
                    StatusCodes.Status404NotFound,
                    "Transação não encontrada.");
            }
            
            if (transaction.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Transaction>(request.Id, request);
            }

            return new Response<Transaction?>(transaction,
                StatusCodes.Status200OK,
                "Transação encontrada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Erro ao buscar a transação {Id}", request.Id);
            return new Response<Transaction?>(null,
                StatusCodes.Status500InternalServerError,
                "Erro ao buscar a transação");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando transações por período");

            var transactions = await _context.Transactions
                .Where(t => t.UserId == request.UserId)
                .Where(t => t.CreatedAt >= request.StartDate && t.CreatedAt <= request.EndDate)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResponse<List<Transaction>>(transactions,
                StatusCodes.Status200OK,
                "Transações encontradas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Erro ao buscar as transações por período");
            return new PagedResponse<List<Transaction>>(new(),
                StatusCodes.Status500InternalServerError,
                "Erro ao buscar as transações por período");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }
    
    private Response<T?> ErrorNotFromTheUser<T>(long transactionId, Request request) where T : class
    {
        _logger.Warning("Transação {Id} não pertence ao usuário {UserId}", transactionId, request.UserId);
        return new Response<T?>(null,
            StatusCodes.Status403Forbidden,
            "Transação não pertence ao usuário.");
    }
}