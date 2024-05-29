using System.Security.Claims;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Movement;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints.Movements;

public static class CreateMovementEndpoint
{
    public static Task<Response<Movement?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IMovementHandler handler,
        [FromBody] CreateMovementRequest request)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        request.UserId = userId;
        return handler.CreateAsync(request);
    }
}