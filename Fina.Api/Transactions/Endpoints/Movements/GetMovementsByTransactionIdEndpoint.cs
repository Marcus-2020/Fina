using System.Security.Claims;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Movement;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints.Movements;

public static class GetMovementsByTransactionIdEndpoint
{
    public static Task<PagedResponse<List<Movement>>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IMovementHandler handler,
        [FromRoute] long transactionId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        GetMovementsByTransactionIdRequest request = new()
        {
            TransactionId = transactionId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = userId
        };
        
        return handler.GetByTransactionIdAsync(request);
    }
}