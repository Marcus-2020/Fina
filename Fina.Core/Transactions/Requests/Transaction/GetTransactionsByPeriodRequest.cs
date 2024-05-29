using Fina.Core.Common.Requests;

namespace Fina.Core.Transactions.Requests.Transaction;

public class GetTransactionsByPeriodRequest : PagedRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}