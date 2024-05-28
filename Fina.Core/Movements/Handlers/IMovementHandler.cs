using Fina.Core.Common.Responses;
using Fina.Core.Movements.Models;
using Fina.Core.Movements.Requests;

namespace Fina.Core.Movements.Handlers;

public interface IMovementHandler
{
    Task<Response<Movement?>> AddAsync(AddMovementToTransactionRequest request);
    Task<Response<Movement?>> UpdateAsync(UpdateMovementRequest request);
    Task<Response<Movement?>> DeleteAsync(DeleteMovementRequest request);
    Task<Response<Movement?>> GetByIdAsync(GetMovementByIdRequest request);
    Task<Response<List<Movement>>> GetByTransactionIdAsync(GetMovementsByTransactionIdRequest request);
}