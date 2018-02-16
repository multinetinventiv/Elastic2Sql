using System.Collections.Generic;
using System.Data.SqlClient;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;

namespace Inventiv.Tools.Elastic2Sql.Repository.SqlServer
{
	public class SqlQueryBuilder
	{
		public static string BuildSelectQuery(List<QueryInfo> queryInformation, string tableName)
		{
			var queryString = $"Select * from {tableName} Where ";

			foreach (var queryInfo in queryInformation)
			{
				if (queryInformation.IndexOf(queryInfo) != 0)
				{
					queryString += " AND ";
				}

				queryString += $"{queryInfo.FieldName} = @{queryInfo.FieldName} ";
			}

			return queryString;
		}

		public static List<SqlParameter> BuildSqlParameters(List<QueryInfo> queryInformantion)
		{
			var list = new List<SqlParameter>();

			foreach (var queryInfo in queryInformantion)
			{
				list.Add(new SqlParameter($"@{queryInfo.FieldName}", queryInfo.Values[0]));
			}

			return list;
		}

		public static string BuildInsertQuery(List<string> columns, string tableName)
		{
			var dbColumns = string.Empty;
			var parameters = string.Empty;

			for (var index = 0; index < columns.Count; index++)
			{
				if (index != 0)
				{
					dbColumns += ", ";
					parameters += ", ";
				}

				dbColumns += columns[index];
				parameters += $"@{columns[index]}";
			}

			return $"INSERT INTO {tableName} ({dbColumns}) VALUES ({parameters})";
		}

		public static List<SqlParameter> BuildInsertParameters(List<string> columnNames, object[] values)
		{
			//var parameters = new List<SqlParameter>();

			//for (var index = 0; index < columnNames.Count; index++)
			//{
			//	parameters.Add(new SqlParameter
			//	{
			//		 ParameterName = $"@{columnNames[index]}",
			//		  DbType = DbType.
			//	});
			//}

			throw new System.NotImplementedException();
		}
	}
}
