using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public class ElasticRepository : IRepository
	{
		private readonly ElasticClient client;
		private readonly string indexName;
		private readonly string typeName;

		public ElasticRepository(string indexName, string typeName)
		{
			if (string.IsNullOrWhiteSpace(indexName) || string.IsNullOrWhiteSpace(typeName))
			{
				throw new ArgumentNullException($"Index name or type name are empty! (indexName = {indexName}, typeName = {typeName})");
			}

			this.indexName = indexName;
			this.typeName = typeName;

			client = Configuration.GetElasticSearchClient();
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

		public List<object> Get(List<QueryInfo> queryInformation, int takeCount = 100)
		{
			var searchResponse = client.Search<dynamic>(s => s
				.Index(indexName)
				.Type(typeName)
				.From(0)
				.Size(takeCount)
				.Query(q =>
					q.BuildQuery(queryInformation)
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

		public void Update(List<string> columnNames, params object[] values)
		{
			throw new NotImplementedException();
		}

	}
}
