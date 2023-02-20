using PaymentTransactionsServie.Models;
using System.Threading.Tasks;

namespace PaymentTransactionsServie.Interfaces
{
	internal interface IFileReader
	{
		Task<PaymentsDTO> ReadFile(string filePath);
	}
}