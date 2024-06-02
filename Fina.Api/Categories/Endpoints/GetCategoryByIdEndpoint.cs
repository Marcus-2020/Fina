using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;

namespace Fina.Api.Categories.Endpoints;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: GetById")
            .WithSummary("Get a category by ID")
            .WithDescription("Get a category by ID")
            .WithOrder(4)
            .Produces<Response<Category>?>();

    private static async Task<IResult> HandleAsync(long id, ICategoryHandler handler)
    {
        GetCategoryByIdRequest request = new()
        {
            Id = id,
            UserId = ApiConfiguration.UserId
        };
        
        Response<Category?> response =  await handler.GetByIdAsync(request);

        if (response.IsSuccess) return TypedResults.Ok(response);
        
        return response.StausCode switch
        {
            StatusCodes.Status404NotFound => TypedResults.NotFound(response),
            StatusCodes.Status500InternalServerError => TypedResults.StatusCode(500),
            _ => TypedResults.BadRequest(response)
        };
    }
}