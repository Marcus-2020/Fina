using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;

namespace Fina.Api.Categories.Endpoints;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Update the category")
            .WithDescription("Update the category")
            .WithOrder(2)
            .Produces<Response<Category?>>();
    }

    private static async Task<IResult> HandleAsync(UpdateCategoryRequest request, ICategoryHandler handler)
    {
        request.UserId = ApiConfiguration.UserId;
        Response<Category?> response =  await handler.UpdateAsync(request);
        return response.IsSuccess 
            ? TypedResults.Ok(response) 
            : TypedResults.BadRequest(response);
    }
}