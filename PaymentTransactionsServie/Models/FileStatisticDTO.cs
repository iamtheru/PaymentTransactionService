
namespace PaymentTransactionsServie.Models
{
	internal class FileStatisticDTO
	{
		public string FileName { get; set; }

		public int CorrectLines { get; set; }

		public int IncorrectLines { get; set; }
	}
}
