using System;
using System.Collections.Generic;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public static class ElasticQueryBuilderExtension
	{
		public static QueryContainer BuildQuery(this QueryContainerDescriptor<dynamic> descriptor, List<QueryInfo> queryInformation)
		{
			if (queryInformation == null || queryInformation.Count == 0) { return descriptor.MatchAll(); }

			var factory = new QueryContainerFactory();
			var queryContainer = new QueryContainer();

			foreach (var queryInfo in queryInformation)
			{
				queryContainer = queryContainer && factory.CreateQueryContainer(queryInfo.Type).BuildQuery(descriptor, queryInfo);
			}

			return queryContainer;
		}
	}
}
