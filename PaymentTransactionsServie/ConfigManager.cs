using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentTransactionsServie
{
	internal class ConfigManager
	{
		private const string CONFIG_FILE_PATH = "C:\\PaymentTransactionsServieConfig.xml";

		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private static Configuration _configuration;

		public Configuration GetConfig => _configuration;

		public static void ReadAndInitConfig()
		{
			

			if (File.Exists(CONFIG_FILE_PATH))
			{
				try
				{
					using (var fileStream = new FileStream(CONFIG_FILE_PATH, FileMode.Open))
					{
						var xmlSerializer = new XmlSerializer(typeof(Configuration));
						_configuration = xmlSerializer.Deserialize(fileStream) as Configuration;
						_logger.Info($"{_configuration.SourcePath}, {_configuration.DestinationPath}, {_configuration.AllowedFilesFormats}");
					}
				}
				catch (Exception ex)
				{
					_logger.Error(ex.Message);
				}
			}
			else
			{
				var message = $"Configuration file not found at {CONFIG_FILE_PATH}";
				var exception = new FileNotFoundException(message);
				EventLog.WriteEntry("PaymentTransactionService", message ,EventLogEntryType.Error);
				_logger.Error(message);
				throw exception;
			}
		}
	}
}
