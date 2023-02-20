using Newtonsoft.Json;
using PaymentTransactionsServie.Helpers;
using PaymentTransactionsServie.Interfaces;
using PaymentTransactionsServie.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTransactionsServie
{
	internal class PaymentHaldler
	{
		private readonly IFileReaderFactory _fileReaderFactory = new FileReaderFactory();

		public async Task ProcessFile(string sourcePath, string extension)
		{
			var reader = _fileReaderFactory.Create(extension);
			var payments = await reader.ReadFile(sourcePath);

			if (payments.FileLinesCount == 0)
			{
				return;
			}

			var fileNumber = FileStatistic.IncrementFiles();
			var filePath = PaymentFolder.GetCurrentFilePath(fileNumber);
			var correctLines = payments.Payments.Count();
			var incorrectLinesCount = payments.FileLinesCount - correctLines;

			FileStatistic.AddParsedLines(correctLines);

			if (incorrectLinesCount > 0)
			{
				FileStatistic.AddErrors(incorrectLinesCount);
				FileStatistic.AddInvaidFilePath(filePath);
			}

			var result = await TransformPayments(payments.Payments);

			await Task.Run(() => File.WriteAllText(filePath, JsonConvert.SerializeObject(result, Formatting.Indented)));
		}

		private async Task<IEnumerable<object>> TransformPayments(List<PaymentModel> payments)
		{
			return await Task.Run(() => payments
			.GroupBy(p => p.Address.Split(',')[0].Trim())
			.Select(g => new {
				city = g.Key,
				services = g.GroupBy(p => p.Service)
					.Select(s => new {
						name = s.Key,
						payers = s.Select(p => new {
							name = $"{p.FirstName} {p.LastName}",
							payment = p.Payment,
							date = p.Date.ToShortDateString(),
							account_number = p.AccountNumber
						}),
						total = s.Sum(p => p.Payment)
					}),
				total = g.Sum(p => p.Payment)
			}));
		}
	}
}