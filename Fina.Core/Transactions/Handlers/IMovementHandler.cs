using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Movement;

namespace Fina.Core.Transactions.Handlers;

public interface IMovementHandler
{
    Task<Response<Movement?>> CreateAsync(CreateMovementRequest request);
    Task<Response<Movement?>> UpdateAsync(UpdateMovementRequest request);
    Task<Response<Movement?>> DeleteAsync(DeleteMovementRequest request);
    Task<Response<Movement?>> GetByIdAsync(GetMovementByIdRequest request);
    Task<PagedResponse<List<Movement>>> GetByTransactionIdAsync(GetMovementsByTransactionIdRequest request);
}