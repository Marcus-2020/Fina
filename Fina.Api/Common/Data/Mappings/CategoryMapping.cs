using Fina.Core.Categories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fina.Api.Common.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("category");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Title)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(80);
        
        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasColumnType("varchar")
            .HasMaxLength(255); // 255 é o limite para indice do banco
        
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(160);
    }
}