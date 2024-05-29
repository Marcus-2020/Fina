using System.Security.Claims;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Accounts.Endpoints;

public static class DeleteAccountEndpoint
{
    public static Task<Response<Account?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IAccountHandler handler,
        [FromBody] DeleteAccountRequest request)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        request.UserId = userId;
        return handler.DeleteAsync(request);
    }
}