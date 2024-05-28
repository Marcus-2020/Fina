using System.ComponentModel.DataAnnotations;
using Fina.Core.Common.Requests;

namespace Fina.Core.Categories.Requests;

public class UpdateCategoryRequest : Request
{
    [Required(ErrorMessage = "ID é obrigatório.")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Titulo é obrigatório")]
    [MaxLength(80, ErrorMessage = "O título deve ter no máximo 80 caracteres")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Description { get; set; } = string.Empty;
}