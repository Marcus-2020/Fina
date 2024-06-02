using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;

namespace Fina.Api.Transactions.Endpoints;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Delete the transaction")
            .WithDescription("Delete the transaction")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(long id, ITransactionHandler handler)
    {
        var request = new DeleteTransactionRequest { Id = id, UserId = ApiConfiguration.UserId };
        var response = await handler.DeleteAsync(request);
        return response.IsSuccess ? Results.Ok(response.Data) : Results.NotFound();
    }
}