using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;

namespace Fina.Api.Transactions.Endpoints;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: GetById")
            .WithSummary("Get a transaction by ID")
            .WithDescription("Get a transaction by ID")
            .WithOrder(4)
            .Produces<Response<Transaction>?>();

    private static async Task<IResult> HandleAsync(long id, ITransactionHandler handler)
    {
        var request = new GetTransactionByIdRequest { Id = id, UserId = ApiConfiguration.UserId };
        var response = await handler.GetByIdAsync(request);
        
        if (response.IsSuccess) return TypedResults.Ok(response);
        
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => TypedResults.NotFound(response),
            StatusCodes.Status500InternalServerError => TypedResults.StatusCode(500),
            _ => TypedResults.BadRequest(response)
        };
    }
}