using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using Elasticsearch.Net;
using Inventiv.Tools.Elastic2Sql.Helper;
using log4net;
using Nest;

namespace Inventiv.Tools.Elastic2Sql
{
	public static class Configuration
	{
		#region Fields

		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Elasticsearch

		public static ElasticClient GetElasticSearchClient()
		{
			var serverAddress = ConfigurationManager.AppSettings[Constants.ELASTIC_SERVER_ADDRESS];

			return GetElasticSearchClient(serverAddress);
		}
		public static ElasticClient GetElasticSearchClient(string serverAddress)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(serverAddress)) { throw new ArgumentNullException(nameof(serverAddress), "Elasticsearch server address is null or empty"); }

				var nodes = new[] { new Uri(serverAddress) };

				var connectionPool = new StaticConnectionPool(nodes);
				var connectionSettings = new ConnectionSettings(connectionPool)
					.DisableDirectStreaming();

				return new ElasticClient(connectionSettings);
			}
			catch (Exception e)
			{
				var message = "It is not gotten a client to connect to elasticsearch instance";
				logger.Fatal(message, e);
				throw new Exception(message, e);
			}
		}

		#endregion
		
		#region Sql Server Ado.Net

		public static SqlConnection GetSqlConnection()
		{
			var dataSource = ConfigurationManager.AppSettings[Constants.DATA_SOURCE];
			var databaseName = ConfigurationManager.AppSettings[Constants.DATABASE_NAME];
			var integratedSecutiry = ConfigurationManager.AppSettings[Constants.INTEGRATED_SECURITY];
			var username = ConfigurationManager.AppSettings[Constants.USERNAME];
			var password = ConfigurationManager.AppSettings[Constants.PASSWORD];

			if (!string.IsNullOrWhiteSpace(integratedSecutiry) && integratedSecutiry == "true")
			{
				return GetSqlConnection(dataSource, databaseName);
			}

			return GetSqlConnection(dataSource, databaseName, username, password);
		}

		public static SqlConnection GetSqlConnection(string dataSource, string databaseName)
		{
			return GetSqlConnection(dataSource, databaseName, true);
		}

		public static SqlConnection GetSqlConnection(string dataSource, string databaseName, string username, string password)
		{
			return GetSqlConnection(dataSource, databaseName, false, username, password);
		}

		private static SqlConnection GetSqlConnection(string dataSource, string databaseName, bool integratedSecurity, string username = "", string password = "")
		{
			try
			{
				if (string.IsNullOrWhiteSpace(dataSource) || string.IsNullOrWhiteSpace(databaseName)) { throw new Exception("Data source or database name are null or empty"); }
				if (!integratedSecurity && (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))) { throw new Exception("Username or password are null or empty!"); }

				var connectionString = $"Data Source={dataSource};Initial Catalog={databaseName};";

				if (integratedSecurity)
				{
					connectionString += "Integrated Security=true";
				}
				else
				{
					connectionString += $"User Id={username}; Password={password}";
				}

				return new SqlConnection(connectionString);
			}
			catch (Exception e)
			{
				var message = "It is not created connection string by given information";
				logger.Fatal(message, e);
				throw new Exception(message, e);
			}
		}

		#endregion
	}
}
