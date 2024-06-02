using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;

namespace Fina.Api.Transactions.Endpoints;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Update the transaction")
            .WithDescription("Update the transaction")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(UpdateTransactionRequest request, ITransactionHandler handler)
    {
        request.UserId = ApiConfiguration.UserId;
        var response = await handler.UpdateAsync(request);
        return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
    }
}