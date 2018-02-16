using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;

namespace Inventiv.Tools.Elastic2Sql.Repository.SqlServer
{
	public class SqlServerRepository : IRepository
	{
		private readonly SqlConnection sqlConnection;
		private readonly IDatabaseInformation databaseInformation;

		public SqlServerRepository(IDatabaseInformation databaseInformation)
		{
			this.databaseInformation = databaseInformation;
			sqlConnection = Connection.GetSqlConnection();
		}

		public List<dynamic> GetAll(int takeCount)
		{
			throw new System.NotImplementedException();
		}

		public List<object> Get(List<QueryInfo> queryInformation, int takeCount, params object[] values)
		{
			var queryString = SqlQueryBuilder.BuildSelectQuery(queryInformation, databaseInformation.TableName);
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
			throw new NotImplementedException();
		}

		public void BulkInsert(DataTable values)
		{
			sqlConnection.Open();
			using (var transaction = sqlConnection.BeginTransaction())
			{
				try
				{
					using (var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, transaction))
					{
						bulkCopy.DestinationTableName = databaseInformation.TableName;
						
						foreach (DataColumn dcPrepped in values.Columns)
						{
							bulkCopy.ColumnMappings.Add(dcPrepped.ColumnName, dcPrepped.ColumnName);
						}

						bulkCopy.WriteToServer(values);
					}

					transaction.Commit();
					sqlConnection.Close();
				}
				catch(Exception ex)
				{
					transaction.Rollback();
					sqlConnection.Close();

					throw ex;
				}
			}
		}
	}
}
