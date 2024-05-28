using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Movements.Requests;

public class GetMovementsByTransactionIdRequest : PagedRequest
{
    [Required(ErrorMessage = "ID da transação é obrigatório")]
    public long TransactionId { get; set; }
}