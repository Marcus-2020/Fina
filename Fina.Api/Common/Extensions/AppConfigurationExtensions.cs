using Fina.Api.Accounts.Handlers;
using Fina.Api.Categories.Handlers;
using Fina.Api.Transactions.Handlers;
using Fina.Core.Accounts.Handlers;
using Fina.Core.Categories.Handlers;
using Fina.Core.Transactions.Handlers;

namespace Fina.Api.Common.Extensions;

public static class AppConfigurationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountHandler, AccountHandler>();
        services.AddScoped<ICategoryHandler, CategoryHandler>();
        services.AddScoped<ITransactionHandler, TransactionHandler>();
        services.AddScoped<IMovementHandler, MovementHandler>();
    }
}