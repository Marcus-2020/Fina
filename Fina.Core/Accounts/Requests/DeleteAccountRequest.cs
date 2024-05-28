using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Accounts.Requests;

public class DeleteAccountRequest : Request
{
    [Required(ErrorMessage = "ID da conta é obrigatório.")]
    public long Id { get; set; }
}