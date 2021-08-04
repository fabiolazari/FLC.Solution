using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlcIO.App.ViewModels
{
	public class FlcMessageViewModel
	{
		[Key]
		public Guid Id { get; }

		[DisplayName("Requisição")]
		[Required(ErrorMessage = "O campo {0} é obrigatório!")]
		public Int32 IdRequest { get; }

		[DisplayName("Data")]
		[Required(ErrorMessage = "O campo {0} é obrigatório!")]
		public DateTime Timestamp { get; }

		[DisplayName("Mensagem")]
		[Required(ErrorMessage = "O campo {0} é obrigatório!")]
		[StringLength(250, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres!", MinimumLength = 2)]
		public string MessageDescription { get; }
	}
}
