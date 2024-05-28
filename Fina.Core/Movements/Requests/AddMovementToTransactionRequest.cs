using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Movements.Requests;

public class AddMovementToTransactionRequest : Request
{
    [Required(ErrorMessage = "ID da transação é obrigatório.")]
    public long TransactionId { get; set; }
    
    [Required(ErrorMessage = "ID da conta é obrigatório.")]
    public long AccountId { get; set; }
    
    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "A data de pagamento ou recebimento é obrigatória.")]
    public DateTime PaidOrReceivedAt { get; set; }
}