using Fina.Core.Transactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fina.Api.Common.Data.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transaction");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(80);
        
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        
        builder.Property(t => t.PaidOrReceivedAt)
            .IsRequired(false);
        
        builder.Property(t => t.Type)
            .IsRequired()
            .HasColumnType("smallint");
        
        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(15,2)");
        
        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(160);

        builder.Property(t => t.CategoryId)
            .IsRequired();
        
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId);
    }
}