using System.Security.Claims;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Requests.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Transactions.Endpoints.Transactions;

public static class GetTransactionsByPeriodEndpoint
{
    public static Task<PagedResponse<List<Core.Transactions.Models.Transaction>>> HandleAsync(
        [FromServices] ClaimsPrincipal principal, 
        [FromServices] ITransactionHandler handler, 
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        if (startDate is null || endDate is null)
        {
            var today = DateTime.Now;
            startDate ??= new DateTime(today.Year, today.Month, 1);
            endDate ??= startDate.Value.AddMonths(1).AddDays(-1);
        }
        
        var request = new GetTransactionsByPeriodRequest
        {
            StartDate = startDate.Value,
            EndDate = endDate.Value
        };
        
        return handler.GetByPeriodAsync(request);
    }
}