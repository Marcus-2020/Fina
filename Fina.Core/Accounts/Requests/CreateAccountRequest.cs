using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Accounts.Requests;

public class CreateAccountRequest : Request
{
    [Required(ErrorMessage = "O nome da conta é obrigatório.")]
    [MaxLength(30, ErrorMessage = "O nome da conta deve ter no máximo 30 caracteres.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O saldo inicial é obrigatório.")]
    public decimal Balance { get; set; }
}