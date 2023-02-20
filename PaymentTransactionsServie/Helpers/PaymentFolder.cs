using System;
using System.IO;


namespace PaymentTransactionsServie.Helpers
{
	internal static class PaymentFolder
	{
		private static string _resultFolderName;
		private static readonly string _partResultName;
		private static readonly string _resultExtension;
		private static readonly string _metaLogFile;
		private static readonly string _sourcePath;
		private static readonly string _destinationPath;
		public static string ResultPath;

		static PaymentFolder()
		{
			_partResultName = "output";
			_resultExtension = ".json";
			_metaLogFile = "meta.log";
			_sourcePath = ConfigManager.Configuration.SourcePath;
			_destinationPath = ConfigManager.Configuration.DestinationPath;
			_resultFolderName = DateTime.Now.ToString("dd-MM-yyyy");
			ResultPath = Path.Combine(_destinationPath, _resultFolderName);
		}

		public static string MetaLogPath => Path.Combine(ResultPath, _metaLogFile);

		public static string GetCurrentFilePath(int fileNumber)
		{
			return $"{ResultPath}\\{_partResultName + fileNumber + _resultExtension}";
		}

		public static void CreateCurrentResultFolder()
		{
			if (!Directory.Exists(ResultPath))
			{
				Directory.CreateDirectory(ResultPath);
			}
		}

		public static void SetResultFolder()
		{
			_resultFolderName = DateTime.Now.ToString("dd-MM-yyyyy");
		}

		public static void CreateFolders()
		{
			if (!Directory.Exists(_sourcePath))
			{
				Directory.CreateDirectory(_sourcePath);
			}

			if (!Directory.Exists(_destinationPath))
			{
				Directory.CreateDirectory(_destinationPath);
			}
			CreateCurrentResultFolder();
		}
	}
}
