using PaymentTransactionsServie.Helpers;
using PaymentTransactionsServie.Interfaces;
using PaymentTransactionsServie.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace PaymentTransactionsServie.Readers
{
	internal class CSVReader : IFileReader
	{
		private readonly Lazy<Validator> _validator = new Lazy<Validator>();

		public async Task<PaymentsDTO> ReadFile(string filePath)
		{
			int lineCount = 0;
			var currentAddress = string.Empty;

			var output = await Task.Run(() => File.ReadAllLines(filePath)
			.Skip(1)
			.Where(str => _validator.Value.Validate(str, ref lineCount))
			.Select(str => str.CuttAddressAndFormat(out currentAddress))
			.Select(parts => new PaymentModel
			{
				FirstName = parts[0],
				LastName = parts[1],
				Address = currentAddress,
				Payment = decimal.Parse(parts[2].Replace('.', ',')),
				Date = DateTime.ParseExact(parts[3],"yyyy-dd-MM", CultureInfo.InvariantCulture),
				AccountNumber = long.Parse(parts[4]),
				Service = parts[5]
			})
			.ToList());

			return new PaymentsDTO
			{
				FileLinesCount = lineCount ,
				Payments = output 
			};
		}
	}
}
