using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
