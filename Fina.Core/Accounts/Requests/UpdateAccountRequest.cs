using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Accounts.Requests;

public class UpdateAccountRequest : Request
{
    [Required(ErrorMessage = "ID da conta é obrigatório.")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "O nome da conta é obrigatório.")]
    [MaxLength(30, ErrorMessage = "O nome da conta deve ter no máximo 30 caracteres.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O saldo inicial é obrigatório.")]
    public decimal Balance { get; set; }
}