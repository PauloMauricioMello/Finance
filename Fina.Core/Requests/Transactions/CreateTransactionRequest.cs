using Fina.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Transactions;

public class CreateTransactionRequest :Request
{
      [Required(ErrorMessage = "O título é requerido.")]
      public string Title { get; set; } = string.Empty;

      [Required(ErrorMessage = "O type é requerido.")]
      public ETransactionType Type { get; set; }

      [Required(ErrorMessage = "O Amaount é requerido.")]
      public decimal Amount { get; set; }

      [Required(ErrorMessage = "A categoria é requerida.")]
      public long CategoryId { get; set; }

      [Required(ErrorMessage = "A data é requerida.")]
      public DateTime? PaidReceivedAt { get; set; }
}
