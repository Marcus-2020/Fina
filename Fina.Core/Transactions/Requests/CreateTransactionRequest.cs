using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Enums;
using Fina.Core.Common.Requests;

namespace Fina.Core.Transactions.Requests;

public class CreateTransactionRequest : Request
{
    [Required(ErrorMessage = "Titulo é obrigatório")]
    [MaxLength(80, ErrorMessage = "O título deve ter no máximo 80 caracteres")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O ID da categoria é obrigatório")]
    public long CategoryId { get; set; }
    
    [Required(ErrorMessage = "O valor é obrigatório")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "O tipo é obrigatório")]
    public TransactionTypeEnum Type { get; set; }
}