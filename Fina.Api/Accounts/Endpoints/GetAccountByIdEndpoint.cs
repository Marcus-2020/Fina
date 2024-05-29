using System.Security.Claims;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Accounts.Endpoints;

public static class GetAccountByIdEndpoint
{
    public static Task<Response<Account?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IAccountHandler handler,
        [FromRoute] long id)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        GetAccountByIdRequest request = new()
        {
            Id = id,
            UserId = userId
        };
        
        return handler.GetByIdAsync(request);
    }
}