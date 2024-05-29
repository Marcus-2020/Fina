using System.Security.Claims;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Accounts.Models;
using Fina.Core.Accounts.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Accounts.Endpoints;

public static class GetAllAccountsEndpoint
{
    public static Task<PagedResponse<List<Account>>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] IAccountHandler handler,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        GetAllAccountsRequest request = new()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        return handler.GetAllAsync(request);
    }
}