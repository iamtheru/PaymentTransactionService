using NLog;
using PaymentTransactionsServie.Helpers;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace PaymentTransactionsServie
{
	public partial class PaymentTransactionService : ServiceBase
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private readonly Configuration _configuration;
		private FileSystemWatcher _watcher;
		private Timer _timer;

		public PaymentTransactionService()
		{
			InitializeComponent();
			_configuration = ConfigManager.ReadConfig();
			_logger.Info("Constructor from PaymentTransactionService");
		}

		protected override void OnStart(string[] args)
		{
			if(_configuration == null)
			{
				Stop();
				return;
			}

			PaymentFolder.CreateFolders();
			SetWatcher(_configuration.SourcePath);
			SetTimer();
			_logger.Info(_watcher.Filter);
		}

		protected override void OnStop()
		{
			_timer.Stop();
			_timer.Dispose();
		}

		private async void ProcessPayment(object sender, FileSystemEventArgs e)
		{
			var sourcePath = e.FullPath;
			var extension = Path.GetExtension(sourcePath);

			if (_configuration.AllowedFilesFormats.IndexOf(extension) == -1)
			{
				return;
			}

			try
			{
				await new PaymentHaldler().ProcessFile(sourcePath, extension);
			}
			catch (Exception ex)
			{
				_logger.Error($"Exception occurs when processing file. Exception source: {ex.Source}" +
					$"Eception message:{ex.Message}" +
					$"Stack: {ex.StackTrace}");
			}

		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			File.WriteAllText(PaymentFolder.MetaLogPath, FileStatistic.ToString());

			PaymentFolder.SetResultFolder();
			PaymentFolder.CreateCurrentResultFolder();

			var now = DateTime.Now;
			var nextRunTime = now.AddDays(1).Date;
			var timeToNextRun = nextRunTime - now;
			_timer.Interval = timeToNextRun.TotalMilliseconds;
		}

		private void SetTimer()
		{
			var now = DateTime.Now;
			var nextRunTime = now.AddDays(1).Date;
			var timeToNextRun = nextRunTime - now;
	
			_timer = new Timer(timeToNextRun.TotalMilliseconds);
			_timer.Elapsed += OnTimerElapsed;
			_timer.AutoReset = true;
			_timer.Start();
		}

		private void SetWatcher(string sourcePath)
		{
			_watcher = new FileSystemWatcher();
			_watcher.Path = sourcePath;
			_watcher.IncludeSubdirectories = false;
			_watcher.EnableRaisingEvents = true;
			_watcher.Created += new FileSystemEventHandler(ProcessPayment);
		}
	}
}