using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Transactions.Requests;

public class DeleteTransactionRequest : Request
{
    [Required(ErrorMessage = "ID é obrigatório.")]
    public long Id { get; set; }
}