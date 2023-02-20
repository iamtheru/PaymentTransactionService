
namespace PaymentTransactionsServie.Interfaces
{
	internal interface IFileReaderFactory
	{
		IFileReader Create(string extension);
	}
}