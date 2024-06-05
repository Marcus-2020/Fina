using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Core;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints;

public class GetAllTransactionByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/transactions", GetAllTransactionByPeriodEndpoint.HandleAsync)
            .WithName("Transactions: GetAllByPeriod")
            .WithSummary("Get all transaction by a period")
            .WithDescription("Get all transaction by a period")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>>?>();

    public static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionByPeriodRequest
        {
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = ApiConfiguration.UserId
        };

        var response = await handler.GetAllAsync(request);
        
        if (response.IsSuccess) return TypedResults.Ok(response);
        
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => TypedResults.NotFound(response),
            StatusCodes.Status500InternalServerError => TypedResults.StatusCode(500),
            _ => TypedResults.BadRequest(response)
        };
    }
}