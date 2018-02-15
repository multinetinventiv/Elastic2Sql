using System;
using Inventiv.Tools.Elastic2Sql.Query;
using Inventiv.Tools.Elastic2Sql.Repository.Elastic.QueryContainers;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public class QueryContainerFactory
	{
		public IQueryContainer CreateQueryContainer(QueryType type)
		{
			switch (type)
			{
				case QueryType.Match:
					return new MatchQueryContainer();

				case QueryType.DateRange:
					return new DateRangeQueryContainer();

				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
