using System;
using System.Globalization;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;
using Inventiv.Tools.Elastic2Sql.DataTransporters;
using Inventiv.Tools.Elastic2Sql.GenericRepository;
using Inventiv.Tools.Elastic2Sql.GenericRepository.SqlServer;
using Inventiv.Tools.Elastic2Sql.Helper;
using Inventiv.Tools.Elastic2Sql.Mappers;
using Inventiv.Tools.Elastic2Sql.Models;
using Inventiv.Tools.Elastic2Sql.Repository;
using Inventiv.Tools.Elastic2Sql.Repository.Elastic;
using Inventiv.Tools.Elastic2Sql.Repository.SqlServer;
using Microsoft.Extensions.CommandLineUtils;

namespace Inventiv.Tools.Elastic2Sql
{
	public class Program
	{
		private readonly CommandLineApplication application;

		public static void Main(string[] args)
		{
			var configRepository = new SqlServerRepository<TransportHistory>();

			var xmlPath = $".{Constants.MAPPING_XML_PATH}";

			var sourceDatabaseInformationByXml = new DatabaseInformationByXml(xmlPath, RootNodeName.Elastic);
			var targetDatabaseInformationByXml = new DatabaseInformationByXml(xmlPath, RootNodeName.Sql);

			var sourceRepository = new ElasticRepository(sourceDatabaseInformationByXml);
			var targetRepository = new SqlServerRepository(targetDatabaseInformationByXml);
			var mapper = new Elastic2SqlMapper(sourceDatabaseInformationByXml, targetDatabaseInformationByXml);

			new Program(
				configRepository
				, sourceRepository
				, targetRepository
				, mapper
				).Execute(args);
		}

		public Program(IGenericRepository<TransportHistory> configRepository, IRepository sourceRepository, IRepository targetRepository, IMapper mapper)
		{
			application = new CommandLineApplication
			{
				Name = "elastic2sql",
				Description = "It gets from values in Elasticsearch and pushes to sql"
			};
			application.HelpOption("-?|-h|--help");

			var startDate = application.Argument("startDate", $"The start date of values to be retrieved from elasticsearch. ({Constants.DATETIME_FORMAT})");
			var endDate = application.Argument("endDate", $"The end date of values to be retrieved from elasticsearch. ({Constants.DATETIME_FORMAT})");
			var dataCount = application.Argument("dataCount", "The number of rows to move in one go");

			application.OnExecute(() =>
					{
						var parsedStartDate = DateTime.ParseExact(startDate.Value, Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);
						var parsedEndDate = DateTime.ParseExact(endDate.Value, Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);
						var parsedDataCount = Convert.ToInt32(dataCount.Value);

						var dataTransporter = new DataTransporter(configRepository, sourceRepository, targetRepository, mapper);
						dataTransporter.Transport(parsedStartDate, parsedEndDate, parsedDataCount);
						return 0;
					});
		}

		public int Execute(params string[] args)
		{
			if (args.Length != 3
				|| !DateTime.TryParseExact(args[0], Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dummyDate)
				|| !DateTime.TryParseExact(args[1], Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dummyDate)
				|| !int.TryParse(args[2], out int _)
				)
			{
				application.ShowHelp();
				return 1;
			}

			try
			{
				return application.Execute(args);
			}
			catch (Exception)
			{
				application.ShowHelp();
				return 1;
			}

		}
	}

}
