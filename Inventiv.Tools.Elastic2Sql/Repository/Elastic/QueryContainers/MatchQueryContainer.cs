using System;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic.QueryContainers
{
	public class MatchQueryContainer : IQueryContainer
	{
		public QueryContainer BuildQuery(QueryContainerDescriptor<dynamic> descriptor, QueryInfo queryInfo)
		{
			if (string.IsNullOrWhiteSpace(queryInfo.FieldName)) { throw new ArgumentNullException(nameof(queryInfo), "Field name in query info is empty or null!"); }

			var queryValue = queryInfo.Values == null || queryInfo.Values.Count == 0 ? string.Empty : queryInfo.Values[0].ToString();
			queryValue = queryValue == true.ToString() || queryValue == false.ToString() ? queryValue.ToLower() : queryValue;

			return descriptor.Match(m => m
				.Field(queryInfo.FieldName)
				.Query(queryValue)
			);
		}
	}
}
