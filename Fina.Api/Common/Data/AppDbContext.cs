using Fina.Core.Accounts.Models;
using Fina.Core.Categories.Models;
using Fina.Core.Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Movement> Movements { get; set; }
}