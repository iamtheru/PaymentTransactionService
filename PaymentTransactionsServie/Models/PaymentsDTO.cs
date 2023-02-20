using System.Collections.Generic;

namespace PaymentTransactionsServie.Models
{
	internal class PaymentsDTO
	{
		public int FileLinesCount { get; set; }

		public List<PaymentModel> Payments { get; set; }
	}
}