using PaymentTransactionsServie.Interfaces;
using PaymentTransactionsServie.Readers;
using System;

namespace PaymentTransactionsServie
{
	internal class FileReaderFactory : IFileReaderFactory
	{
		public IFileReader Create(string extension)
		{
			switch (extension.ToLower())
			{
				case ".txt":
					return new TXTReader();
				case ".csv":
					return new CSVReader();
				default:
					throw new NotSupportedException($"Unsupported file extension:{extension}");
			}
		}
	}
}