using Fina.Api.Common.Data;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Requests;
using Fina.Core.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Accounts.Handlers;

public class AccountHandler : IAccountHandler
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<AccountHandler>();
    private readonly AppDbContext _context;

    public AccountHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Account?>> CreateAsync(CreateAccountRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Criando conta {Name}", request.Name);
            
            var account = new Account
            {
                Name = request.Name,
                Balance = request.Balance,
                UserId = request.UserId
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();
            
            _logger.Information("Conta {Name} criada com sucesso", request.Name);

            return new Response<Account?>(account,
                StatusCodes.Status200OK, 
                "Conta criada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao criar a conta {Name}", request.Name);
            return new Response<Account?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao criar a conta");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Account?>> UpdateAsync(UpdateAccountRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Atualizando conta {Id}", request.Id);
            
            var account = await _context.Accounts.FindAsync(request.Id);

            if (account == null)
            {
                _logger.Warning("Conta {Id} não encontrada", request.Id);
                return new Response<Account?>(null,
                    StatusCodes.Status404NotFound,
                    "Conta não encontrada.");
            }
            
            if (account.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Account>(request.Id, request);
            }

            account.Name = request.Name;
            account.Balance = request.Balance;

            await _context.SaveChangesAsync();
            
            _logger.Information("Conta {Id} atualizada com sucesso", request.Id);

            return new Response<Account?>(account,
                StatusCodes.Status200OK, 
                "Conta atualizada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao atualizar a conta {Id}", request.Id);
            return new Response<Account?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar a conta");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Account?>> DeleteAsync(DeleteAccountRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Deletando conta {Id}", request.Id);
            
            var account = await _context.Accounts.FindAsync(request.Id);

            if (account == null)
            {
                _logger.Warning("Conta {Id} não encontrada", request.Id);
                return new Response<Account?>(null,
                    StatusCodes.Status404NotFound,
                    "Conta não encontrada.");
            }
            
            if (account.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Account>(request.Id, request);
            }

            _context.Accounts.Remove(account);

            await _context.SaveChangesAsync();
            
            _logger.Information("Conta {Id} deletada com sucesso", request.Id);

            return new Response<Account?>(account,
                StatusCodes.Status200OK, 
                "Conta deletada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao deletar a conta {Id}", request.Id);
            return new Response<Account?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao deletar a conta");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Account?>> GetByIdAsync(GetAccountByIdRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando conta {Id}", request.Id);
            
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (account == null)
            {
                _logger.Warning("Conta {Id} não encontrada", request.Id);
                return new Response<Account?>(null,
                    StatusCodes.Status404NotFound,
                    "Conta não encontrada.");
            }
            
            if (account.UserId != request.UserId)
            {
                return ErrorNotFromTheUser<Account>(request.Id, request);
            }
            
            _logger.Information("Conta {Id} encontrada com sucesso", request.Id);

            return new Response<Account?>(account,
                StatusCodes.Status200OK, 
                "Conta encontrada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar a conta {Id}", request.Id);
            return new Response<Account?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao buscar a conta");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<PagedResponse<List<Account>>> GetAllAsync(GetAllAccountsRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando todas as contas");

            var accounts = await _context.Accounts
                .Where(x => x.UserId == request.UserId)
                .Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            _logger.Information("Contas encontradas com sucesso");

            return new PagedResponse<List<Account>>(accounts,
                StatusCodes.Status200OK,
                "Contas encontradas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar as contas");
            return new PagedResponse<List<Account>>(new(),
                StatusCodes.Status500InternalServerError,
                $"Erro ao buscar as contas");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }
    
    private Response<T?> ErrorNotFromTheUser<T>(long accountId, Request request) where T : class
    {
        _logger.Warning("Conta {Id} não pertence ao usuário {UserId}", accountId, request.UserId);
        return new Response<T?>(null,
            StatusCodes.Status403Forbidden,
            "Conta não pertence ao usuário.");
    }
}