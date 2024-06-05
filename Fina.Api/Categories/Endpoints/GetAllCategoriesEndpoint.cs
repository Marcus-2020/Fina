using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Categories.Endpoints;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .WithName("Categories: GetAll")
            .WithSummary("Get all categories")
            .WithDescription("Get all categories")
            .WithOrder(5)
            .Produces<PagedResponse<List<Category>?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        GetAllCategoriesRequest request = new()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = ApiConfiguration.UserId
        };
        
        PagedResponse<List<Category>?> response =  await handler.GetAllAsync(request);

        if (response.IsSuccess) return TypedResults.Ok(response);
        
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => TypedResults.NotFound(response),
            StatusCodes.Status500InternalServerError => TypedResults.StatusCode(500),
            _ => TypedResults.BadRequest(response)
        };
    }
}