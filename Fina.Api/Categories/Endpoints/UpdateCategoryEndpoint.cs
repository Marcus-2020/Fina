using System.Security.Claims;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Categories.Endpoints;

public static class UpdateCategoryEndpoint
{
    public static Task<Response<Category?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] ICategoryHandler handler,
        [FromBody] UpdateCategoryRequest request)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        request.UserId = userId;
        return handler.UpdateAsync(request);
    }
}