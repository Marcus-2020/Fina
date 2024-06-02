using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;

namespace Fina.Api.Transactions.Endpoints;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Create a new transaction for a user")
            .WithDescription("Create a new transaction for a user")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(CreateTransactionRequest request, ITransactionHandler handler)
    {
        request.UserId = ApiConfiguration.UserId;
        var response = await handler.CreateAsync(request);
        return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
    }
}