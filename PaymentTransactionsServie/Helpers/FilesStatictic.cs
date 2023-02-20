using System.Collections.Concurrent;
using System.Threading;

namespace PaymentTransactionsServie.Helpers
{
	internal static class FileStatistic
	{
		private static int _countFiles = 0;

		private static int _countParsedLines = 0;

		private static int _countErrors = 0;

		private static ConcurrentBag<string> _invalidFiles = new ConcurrentBag<string>();

		public static int CountFiles
		{
			get { return _countFiles; }
		}

		public static int IncrementFiles()
		{
			return Interlocked.Increment(ref _countFiles);
		}

		public static void AddParsedLines(int lines)
		{
			Interlocked.Exchange(ref _countParsedLines, _countParsedLines + lines);
		}

		public static void AddInvaidFilePath(string filePath)
		{
			_invalidFiles.Add(filePath);
		}

		public static void AddErrors(int errors)
		{
			Interlocked.Exchange(ref _countErrors, _countErrors + errors);
		}

		public static void Reset()
		{
			Interlocked.Exchange(ref _countFiles, 0);
			Interlocked.Exchange(ref _countParsedLines, 0);
			Interlocked.Exchange(ref _countErrors, 0);
			_invalidFiles = new ConcurrentBag<string>();
		}

		public static new string ToString()
		{
			var invalidFilesText = string.Join("\n", _invalidFiles);

			return
				$"parsed_files: {_countFiles}\n" +
				$"parsed_lines: {_countParsedLines}\n" +
				$"found_errors: {_countErrors}\n" +
				$"invalid_files: [{invalidFilesText}]";
		}
	}
}
