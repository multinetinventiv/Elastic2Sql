using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Inventiv.Tools.Elastic2Sql.Query;

namespace Inventiv.Tools.Elastic2Sql.Repository.SqlServer
{
	public class SqlServerRepository : IRepository
	{
		private readonly SqlConnection sqlConnection;
		private readonly string tableName;

		public SqlServerRepository(string tableName)
		{
			this.tableName = tableName;
			sqlConnection = Connection.GetSqlConnection();
		}

		public List<dynamic> GetAll(int takeCount)
		{
			throw new System.NotImplementedException();
		}

		public List<object> Get(List<QueryInfo> queryInformation, int takeCount, params object[] values)
		{
			var queryString = SqlQueryBuilder.BuildSelectQuery(queryInformation, tableName);
			var parameters = SqlQueryBuilder.BuildSqlParameters(queryInformation);

			var command = new SqlCommand(queryString, sqlConnection);
			command.Parameters.AddRange(parameters.ToArray());

			try
			{
				sqlConnection.Open();

				var reader = command.ExecuteReader();
				var rows = new List<object>();

				while (reader.Read())
				{
					var row = new object[reader.FieldCount];

					reader.GetValues(row);

					rows.Add(row);
				}

				reader.Close();
				sqlConnection.Close();

				return rows;
			}
			catch (Exception)
			{
				sqlConnection.Close();
				throw;
			}

		}

		public List<object> Get(int takeCount = 100, params object[] values)
		{
			throw new NotImplementedException();
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
