using Fina.Core.Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Categories.Data;

public class CategoryContext : DbContext
{
    public CategoryContext(DbContextOptions<CategoryContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
}