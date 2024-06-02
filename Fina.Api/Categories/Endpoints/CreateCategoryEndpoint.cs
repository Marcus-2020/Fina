using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;

namespace Fina.Api.Categories.Endpoints;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Create a new category for a user")
            .WithDescription("Create a new category for a user")
            .WithOrder(1)
            .Produces<Response<Category?>>();
    }

    private static async Task<IResult> HandleAsync(
        CreateCategoryRequest request, 
        ICategoryHandler handler)
    {
        request.UserId = ApiConfiguration.UserId;
        Response<Category?> response =  await handler.CreateAsync(request);
        return response.IsSuccess 
            ? TypedResults.Created($"v1/categories/{response.Data?.Id}", response) 
            : TypedResults.BadRequest(response);
    }
}