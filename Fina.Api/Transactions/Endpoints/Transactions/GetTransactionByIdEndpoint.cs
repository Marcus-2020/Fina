using System.Security.Claims;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Requests.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints.Transactions;

public static class GetTransactionByIdEndpoint
{
    public static Task<Response<Core.Transactions.Models.Transaction?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] ITransactionHandler handler,
        [FromRoute] long id)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        GetTransactionByIdRequest request = new()
        {
            Id = id,
            UserId = userId
        };
        
        return handler.GetByIdAsync(request);
    }
}