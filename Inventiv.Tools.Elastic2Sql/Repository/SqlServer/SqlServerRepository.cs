using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Inventiv.Tools.Elastic2Sql.Repository.SqlServer
{
	public class SqlServerRepository : IRepository
	{
		private readonly SqlConnection sqlConnection;
		private readonly string tableName;

		public SqlServerRepository(string tableName)
		{
			this.tableName = tableName;
			sqlConnection = Configuration.GetSqlConnection();
		}

		public List<dynamic> GetAll(int takeCount)
		{
			throw new System.NotImplementedException();
		}

		public List<object> Get(List<QueryInfo> queryInformation, int takeCount)
		{
			var queryString = SqlQueryBuilder.BuildSelectQuery(queryInformation, tableName);
			var parameters = SqlQueryBuilder.BuildSqlParameters(queryInformation);

			var command = new SqlCommand(queryString, sqlConnection);
			command.Parameters.AddRange(parameters.ToArray());

			try
			{
				sqlConnection.Open();

				var reader = command.ExecuteReader();
				var values = new List<object>();

				while (reader.Read())
				{
					var row = new object[reader.FieldCount];

					reader.GetValues(row);

					values.Add(row);
				}

				reader.Close();
				sqlConnection.Close();

				return values;
			}
			catch (Exception)
			{
				sqlConnection.Close();
				throw;
			}

		}

		public void Insert(List<string> columnNames, params object[] values)
		{
			var insertQuery = SqlQueryBuilder.BuildInsertQuery(columnNames, tableName);
			var parameters = SqlQueryBuilder.BuildInsertParameters(columnNames, values);

			var command = new SqlCommand(insertQuery, sqlConnection);
			//TODO
		}

		public void Update(List<string> columnNames, params object[] values)
		{
			throw new System.NotImplementedException();
		}
	}
}
