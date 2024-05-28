using Fina.Core.Common.Requests;

namespace Fina.Core.Transactions.Requests;

public class GetTransactionByPeriodRequest : PagedRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}