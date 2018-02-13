using System;
using System.Globalization;
using System.Reflection;
using Inventiv.Tools.Elastic2Sql.DataTransporters;
using Inventiv.Tools.Elastic2Sql.Helper;
using log4net;
using Microsoft.Extensions.CommandLineUtils;

namespace Inventiv.Tools.Elastic2Sql
{
	public class Program
	{

		#region Fields

		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly CommandLineApplication application;

		#endregion

		public static void Main(string[] args)
		{
			var dataTransporter = new DataTransporter();

			new Program(
				dataTransporter
				).Execute(args);
		}

		public Program(IDataTransporter dataTransporter)
		{
			application = new CommandLineApplication
			{
				Name = "elastic2sql",
				Description = "It gets from values in Elasticsearch and pushes to sql"
			};
			application.HelpOption("-?|-h|--help");

			var startDate = application.Argument("startDate", $"The start date of values to be retrieved from elasticsearch. ({Constants.DATETIME_FORMAT})");
			var endDate = application.Argument("endDate", $"The end date of values to be retrieved from elasticsearch. ({Constants.DATETIME_FORMAT})");

			application.OnExecute(() =>
			{
				var parsedStartDate = DateTime.ParseExact(startDate.Value, Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);
				var parsedEndDate = DateTime.ParseExact(endDate.Value, Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);

				dataTransporter.Transport(parsedStartDate, parsedEndDate);
				return 0;
			});
		}

		public int Execute(params string[] args)
		{
			if (args.Length != 2
				|| !DateTime.TryParseExact(args[0], Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dummyDate)
				|| !DateTime.TryParseExact(args[1], Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dummyDate)
				)
			{
				application.ShowHelp();
				return 1;
			}

			try
			{
				return application.Execute(args);
			}
			catch (Exception ex)
			{
				application.ShowHelp();
				return 1;
			}

		}
	}
}
