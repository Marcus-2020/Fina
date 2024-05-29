using Fina.Api.Accounts.Endpoints;
using Fina.Api.Categories.Endpoints;
using Fina.Api.Transactions.Endpoints;
using Fina.Api.Transactions.Endpoints.Transactions;

namespace Fina.Api.Common.Extensions;

public static class ApiEndpointsExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("/accounts")
            .RequireAuthorization()
            .MapAccountEndpoints();
        
        app.MapGroup("/categories")
            .RequireAuthorization()
            .MapCategoryEndpoints();
        
        app.MapGroup("/transactions")
            .RequireAuthorization()
            .MapTransactionEndpoints();
    }
}