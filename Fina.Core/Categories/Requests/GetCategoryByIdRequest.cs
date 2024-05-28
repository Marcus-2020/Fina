using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Categories.Requests;

public class GetCategoryByIdRequest  : Request
{
    [Required(ErrorMessage = "ID é obrigatório.")]
    public long Id { get; set; }
}