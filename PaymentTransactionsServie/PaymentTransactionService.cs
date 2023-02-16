using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionsServie
{
	public partial class PaymentTransactionService : ServiceBase
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public PaymentTransactionService()
		{
			InitializeComponent();
			_logger.Info("Constructor from PaymentTransactionService");
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				ConfigManager.ReadAndInitConfig();
				_logger.Info("Config initialized");
			}
			catch (FileNotFoundException ex)
			{
				_logger.Error(ex.Message);
				Stop();
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				Stop();
			}
		}

		protected override void OnStop()
		{
		}
	}
}
