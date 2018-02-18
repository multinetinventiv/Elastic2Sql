using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public class ElasticRepository : IRepository
	{
		private readonly ElasticClient client;
		private readonly IDatabaseInformation databaseInformation;
		private string indexName;
		private readonly string typeName;

		public ElasticRepository(IDatabaseInformation databaseInformation)
		{
			client = Connection.GetElasticSearchClient();

			this.databaseInformation = databaseInformation;

			indexName = GetIndexName(databaseInformation);
			typeName = databaseInformation.TableName;
		}
		
		public List<dynamic> GetAll(int takeCount = 100)
		{
			var searchResponse = client.Search<dynamic>(s => s
				.Index(indexName)
				.Type(typeName)
				.From(0)
				.Size(takeCount)
				.Query(q => q.MatchAll())
			);

			if (!searchResponse.IsValid)
			{
				throw new Exception($"It is gotton exception when executing get all query. (indexName = {indexName}, typeName = {typeName})", searchResponse.OriginalException);
			}

			return new List<dynamic>(searchResponse.Documents);
		}

		public List<object> Get(List<QueryInfo> queryInformation, int takeCount = 100, params object[] values)
		{
			var searchResponse = client.Search<dynamic>(s => s
				.Index(indexName)
				.Type(typeName)
				.From(0)
				.Size(takeCount)
				.Query(q =>
					q.BuildQuery(queryInformation, values)
				)
			);

			if (!searchResponse.IsValid)
			{
				throw new Exception(
					$"It is gotton exception when executing get all query. (indexName = {indexName}, typeName = {typeName})\nDetail:\n{searchResponse.DebugInformation}"
					, searchResponse.OriginalException
					);
			}

			return new List<dynamic>(searchResponse.Documents);
		}

		public List<object> Get(int takeCount = 100, params object[] values)
		{
			return Get(databaseInformation.Queries, takeCount, values);
		}

		public void Insert(List<string> columnNames, params object[] values)
		{
			var value = new ExpandoObject();
			var valueDictionary = (IDictionary<string, object>)value;
			for (var i = 0; i < columnNames.Count; i++)
			{
				valueDictionary.Add(columnNames[i], values[i]);
			}

			var searchResponse = client.Index(value, i => i
				.Index(indexName)
				.Type(typeName)
			);

			if (!searchResponse.IsValid)
			{
				throw new Exception($"It is gotton exception when inserting a record. (indexName = {indexName}, typeName = {typeName}, columnNames= {string.Join(", ", columnNames)}, values= {string.Join(", ", values)})", searchResponse.OriginalException);
			}
		}

		public void BulkInsert(DataTable values)
		{
			throw new NotImplementedException();
		}

		#region private

		private string GetIndexName(IDatabaseInformation databaseInformation)
		{
			if (!databaseInformation.DatabaseName.Contains("{"))
			{
				indexName = databaseInformation.DatabaseName;
			}
			else
			{
				var dbInformationList = databaseInformation.DatabaseName.Split('{');
				var indexNumber = Convert.ToInt32(dbInformationList[1].Replace("{", string.Empty).Replace("}", string.Empty).Trim());

				var indexRecord = client.CatIndices().Records
					.Where(r => r.Index.Contains(dbInformationList[0]))
					.OrderByDescending(r => Convert.ToInt32(r.Index.Split('_').LastOrDefault()))
					.Skip(indexNumber)
					.FirstOrDefault();

				if (indexRecord == null)
				{
					throw new Exception($"Cannot gotton index name!({databaseInformation.DatabaseName})");
				}

				indexName = indexRecord.Index;
			}

			return indexName;
		}

		#endregion
	}

}
