using NLog;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace PaymentTransactionsServie
{
	internal class ConfigManager
	{
		private const string CONFIG_FILE_PATH = "C:\\PaymentTransactionsServieConfig.xml";
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public static Configuration Configuration { get; private set; }

		public static Configuration ReadConfig()
		{
			if (!File.Exists(CONFIG_FILE_PATH))
			{
				var message = $"Configuration file not found at {CONFIG_FILE_PATH}";
				var exception = new FileNotFoundException(message);

				EventLog.WriteEntry("PaymentTransactionService", message, EventLogEntryType.Error);
				_logger.Error(message);

				throw exception;
			}

			using (var config = new FileStream(CONFIG_FILE_PATH, FileMode.Open))
			{
				if (config.Length == 0)
				{
					var message = $"Empty config file not allowed: {CONFIG_FILE_PATH}";
					var exception = new ConfigurationErrorsException(message);

					EventLog.WriteEntry("PaymentTransactionService", message, EventLogEntryType.Error);
					_logger.Error(exception);

					throw exception;
				}

				var xmlSerializer = new XmlSerializer(typeof(Configuration));
				Configuration = xmlSerializer.Deserialize(config) as Configuration;

				return Configuration;
			}

		}
	}
}