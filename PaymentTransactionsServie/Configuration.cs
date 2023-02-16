using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionsServie
{
	public class Configuration
	{
		public string SourcePath { get; set; }

		public string DestinationPath { get; set; }

		public string AllowedFilesFormats { get; set; }
	}
}
