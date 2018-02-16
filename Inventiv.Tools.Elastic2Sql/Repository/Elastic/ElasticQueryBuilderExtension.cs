using System.Collections.Generic;
using System.Text.RegularExpressions;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public static class ElasticQueryBuilderExtension
	{
		public static QueryContainer BuildQuery(this QueryContainerDescriptor<dynamic> descriptor, List<QueryInfo> queryInformation, params object[] values)
		{
			if (queryInformation == null || queryInformation.Count == 0) { return descriptor.MatchAll(); }

			var factory = new QueryContainerFactory();
			var queryContainer = new QueryContainer();

			var pattern = @"{\d{1,}}";

			foreach (var queryInfo in queryInformation)
			{
				ReplaceParameterByValue(queryInfo, values, pattern);

				queryContainer = queryContainer && factory.CreateQueryContainer(queryInfo.QueryType).BuildQuery(descriptor, queryInfo);
			}

			return queryContainer;
		}

		private static void ReplaceParameterByValue(QueryInfo queryInfo, IReadOnlyList<object> values, string pattern)
		{
			for (var valueIndex = 0; valueIndex < queryInfo.Values.Count; valueIndex++)
			{
				var value = queryInfo.Values[valueIndex];
				if (!Regex.IsMatch(value.ToString(), pattern))
				{
					continue;
				}

				var indexString = value.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Trim();
				if (!int.TryParse(indexString, out int index))
				{
					continue;
				}

				queryInfo.Values[valueIndex] = values[index];
			}
		}
	}
}
