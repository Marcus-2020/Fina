using Fina.Api.Common.Data;
using Fina.Api.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Extensions;

public static class DatabaseExtensions
{
    public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
        });
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
        });

        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        
        services.AddAuthorizationBuilder();

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddApiEndpoints();
    }
}