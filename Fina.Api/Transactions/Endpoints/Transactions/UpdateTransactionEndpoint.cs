using System.Security.Claims;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Requests.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints.Transactions;

public static class UpdateTransactionEndpoint
{
    public static Task<Response<Core.Transactions.Models.Transaction?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] ITransactionHandler handler,
        [FromBody] UpdateTransactionRequest request)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        request.UserId = userId;
        return handler.UpdateAsync(request);
    }
}