namespace Fina.Core.Transactions.Models;

public class Movement
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public long TransactionId { get; set; }
    public long AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaidOrReceivedAt { get; set; }
}