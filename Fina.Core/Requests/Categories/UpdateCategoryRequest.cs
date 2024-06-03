using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories;

public class UpdateCategoryRequest : Request
{
      public long Id { get; set; }

      [Required(ErrorMessage = "O título é requerido.")]
      [MaxLength(80, ErrorMessage = "O título deve conter até {0} caracteres")]
      public string Title { get; set; } = string.Empty;

      [Required(ErrorMessage = "A descrição é requerida.")]
      public string Description { get; set; } = string.Empty;
}