using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Responses;

namespace Fina.Core.Accounts.Handlers;

public interface IAccountHandler
{
    Task<Response<Account?>> CreateAsync(CreateAccountRequest request);
    Task<Response<Account?>> UpdateAsync(UpdateAccountRequest request);
    Task<Response<Account?>> DeleteAsync(DeleteAccountRequest request);
    Task<Response<Account?>> GetByIdAsync(GetAccountByIdRequest request);
    Task<Response<List<Account>>> GetAllAsync(GetAllAccountsRequest request);
}