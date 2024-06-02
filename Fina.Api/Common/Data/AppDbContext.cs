using System.Reflection;
using Fina.Api.Common.Data.Mappings;
using Fina.Core.Categories.Models;
using Fina.Core.Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}