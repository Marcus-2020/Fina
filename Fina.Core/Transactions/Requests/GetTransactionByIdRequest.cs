using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Transactions.Requests;

public class GetTransactionByIdRequest : Request
{
    [Required(ErrorMessage = "ID é obrigatório.")]
    public long Id { get; set; }
}