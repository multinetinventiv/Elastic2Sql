using System;

namespace Inventiv.Tools.Elastic2Sql.Helper
{
	public static class Constants
	{
		public const string DATA_SOURCE ="SqlServer.DataSource";
		public const string DATABASE_NAME = "SqlServer.InitialCatalog";
		public const string INTEGRATED_SECURITY = "SqlServer.IntegratedSecurity";
		public const string USERNAME = "SqlServer.Username";
		public const string PASSWORD = "SqlServer.Password";

		public const string ELASTIC_SERVER_ADDRESS = "Elastic.ServerAddress";

		public const string DATETIME_FORMAT = "yyyyMMddHHmmss";

		public const string MAPPING_XML_PATH = "/Mappings/Mapping.xml";
		public const string SQL_SERVER_CONTEXT_NAME = "SqlServerContext";
	}
}
