using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Movements.Requests;

public class GetMovementByIdRequest : Request
{
    [Required(ErrorMessage = "ID do movimento é obrigatório")]
    public long Id { get; set; }
}