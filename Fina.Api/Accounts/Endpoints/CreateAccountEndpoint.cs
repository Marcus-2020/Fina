using System.Security.Claims;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Accounts.Endpoints;

public static class CreateAccountEndpoint
{
    public static Task<Response<Account?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IAccountHandler handler,
        [FromBody] CreateAccountRequest request)
    {
        request.UserId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return handler.CreateAsync(request);
    }
}