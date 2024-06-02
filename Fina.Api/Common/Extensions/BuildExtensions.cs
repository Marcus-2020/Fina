using Fina.Api.Categories.Handlers;
using Fina.Api.Common.Data;
using Fina.Api.Transactions.Handlers;
using Fina.Core;
using Fina.Core.Categories.Handlers;
using Fina.Core.Transactions.Handlers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Fina.Api.Common.Extensions;

public static class BuildExtensions
{
    public static void AddConfigurations(this WebApplicationBuilder builder)
    {
        ApiConfiguration.ConnectionString =
            builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("Services:BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("Services:FrontendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
    }

    public static void AddLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .WriteTo.Console();
        });
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(x =>
        {
            x.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!);
        });
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy =>
                {
                    policy
                        .WithOrigins([Configuration.BackendUrl, Configuration.FrontendUrl])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}