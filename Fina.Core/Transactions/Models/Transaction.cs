using Fina.Core.Categories.Models;
using Fina.Core.Common.Enums;

namespace Fina.Core.Transactions.Models;

public class Transaction
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaidOrReceivedAt { get; set; }
    
    public TransactionTypeEnum Type { get; set; } = TransactionTypeEnum.Withdrawal;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public string UserId { get; set; } = string.Empty;
}