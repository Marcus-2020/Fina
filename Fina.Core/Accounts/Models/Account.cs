
namespace Fina.Core.Accounts.Models;

public class Account
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string UserId { get; set; } = string.Empty;
}