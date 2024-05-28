using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Movements.Requests;

public class DeleteMovementRequest : Request
{
    [Required(ErrorMessage = "ID do movimento é obrigatório.")]
    public long Id { get; set; }
}