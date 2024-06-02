using System.Security.Claims;
using Fina.Api.Common.Data;
using Fina.Core.Categories.Models;
using Fina.Core.Common.Enums;
using Fina.Core.Common.Extensions;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace Fina.Api.Transactions.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public TransactionHandler(AppDbContext context,
        ILogger logger)
    {
        _context = context;

        //string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        _logger = logger.ForContext<ITransactionHandler>();
        //_logger.BindProperty("UserId", userId, false, out _);
    }

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);

            Transaction transaction = new()
            {
                Title = request.Title,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Type = request.Type,
                UserId = request.UserId,
                CreatedAt = DateTime.Now,
                PaidOrReceivedAt = request.PaidOrReceivedAt
            };

            if (!(await ValidateIfCategoryExists(request.CategoryId, request.UserId)))
            {
                _logger.Information("Transação não foi criada pois a categoria {CategoryId} não existe",
                    request.CategoryId);

                return new(null,
                    StatusCodes.Status400BadRequest,
                    "Categoria informada é inválida");
            }

            if (transaction is { Type: TransactionTypeEnum.Withdrawal, Amount: >= 0 })
                transaction.Amount *= -1;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            _logger.Information("Transação {TransactionId} criada com sucesso", transaction.Id);
            return new(transaction, StatusCodes.Status201Created,
                message: "Transação criada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar criar a transação");
            return new(null,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar criar a transação");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);

            var transaction = await _context.Transactions.FindAsync(request.Id);
            if (transaction == null)
            {
                _logger.Warning("Transação com ID {TransactionId} não encontrada", request.Id);
                return new(null, StatusCodes.Status404NotFound, "Transação não encontrada");
            }

            bool categoryChanged = transaction.CategoryId != request.CategoryId;
            if (categoryChanged)
            {
                if (!(await ValidateIfCategoryExists(request.CategoryId, request.UserId)))
                {
                    _logger.Information("Transação não foi atualizada pois a categoria {CategoryId} não existe",
                        request.CategoryId);

                    return new(null,
                        StatusCodes.Status400BadRequest,
                        "Categoria informada é inválida");
                }
            }

            transaction.Title = request.Title;
            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;

            if (transaction is { Type: TransactionTypeEnum.Withdrawal, Amount: >= 0 })
                transaction.Amount *= -1;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            _logger.Information("Transação {TransactionId} atualizada com sucesso", transaction.Id);
            return new(transaction,
                message: "Transação atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar atualizar a transação");
            return new(null, StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar atualizar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await _context.Transactions.FindAsync(request.Id);
            if (transaction == null)
            {
                _logger.Warning("Transação com ID {TransactionId} não encontrada", request.Id);
                return new(null, StatusCodes.Status404NotFound, "Transação não encontrada");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            _logger.Information("Transação {TransactionId} excluída com sucesso", transaction.Id);
            return new(transaction,
                message: "Transação excluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar excluir a transação {TransactionId}", request.Id);
            return new(null, StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id);

            if (transaction == null)
            {
                _logger.Warning("Transação com ID {TransactionId} não encontrada", request.Id);
                return new(null, StatusCodes.Status404NotFound, "Transação não encontrada");
            }

            _logger.Information("Transação com ID {TransactionId} encontrada com sucesso", request.Id);
            return new(transaction,
                message: "Transação recuperada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar recuperar a transação {TransactionId}", request.Id);
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError,
                "Ocorreu um eroo ao tentar recuperar a transação");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetAllAsync(GetTransactionByPeriodRequest request)
    {
        _logger.BindProperty("Request", request, true, out _);

        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Não foi possível determinar a data inicial e final para consulta das transações {@StartEndDate}",
                new { request.StartDate, request.EndDate });

            return new(null, StatusCodes.Status500InternalServerError,
                "Não foi possível determinar a data inicial e final para consulta das transações");
        }

        try
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.PaidOrReceivedAt >= request.StartDate &&
                    t.PaidOrReceivedAt <= request.EndDate &&
                    t.UserId == request.UserId)
                .OrderBy(t => t.PaidOrReceivedAt);

            int totalCount = await query.CountAsync();

            List<Transaction> transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            _logger.Information("Transações recuperadas com sucesso");
            
            return new PagedResponse<List<Transaction>?>(transactions,
                totalCount,
                request.PageNumber,
                request.PageSize,
                message: "Transações recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar recuperar as transações");
            return new(null, StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar recuperar as transações");
        }
    }

    private async Task<bool> ValidateIfCategoryExists(long categoryId, string userId)
    {
        Category? category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

        return category is not null;
    }
}