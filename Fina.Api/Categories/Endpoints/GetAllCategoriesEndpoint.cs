using System.Security.Claims;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Categories.Endpoints;

public static class GetAllCategoriesEndpoint
{
    public static Task<PagedResponse<List<Category>>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] ICategoryHandler handler,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        GetAllCategoriesRequest request = new()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = userId
        };
        
        return handler.GetAllAsync(request);
    }
}