using System;
using Inventiv.Tools.Elastic2Sql.Repository.Elastic.QueryContainers;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public class QueryContainerFactory
	{
		public IQueryContainer CreateQueryContainer(QueryContainerType type)
		{
			switch (type)
			{
				case QueryContainerType.Match:
					return new MatchQueryContainer();

				case QueryContainerType.DateRange:
					return new DateRangeQueryContainer();

				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
