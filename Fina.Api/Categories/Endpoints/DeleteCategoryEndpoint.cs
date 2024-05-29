using System.Security.Claims;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Categories.Endpoints;

public static class DeleteCategoryEndpoint
{
    public static Task<Response<Category?>> HandleAsync(
        [FromServices] ClaimsPrincipal principal,
        [FromServices] ICategoryHandler handler,
        [FromRoute] long id)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        DeleteCategoryRequest request = new() { Id = id, UserId = userId };
        return handler.DeleteAsync(request);
    }
}