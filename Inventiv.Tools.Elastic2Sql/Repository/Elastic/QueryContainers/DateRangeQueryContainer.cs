using System;
using System.Globalization;
using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic.QueryContainers
{
	public class DateRangeQueryContainer : IQueryContainer
	{
		private const string dateTimePattern = "yyyyMMddHHmmss";

		public QueryContainer BuildQuery(QueryContainerDescriptor<dynamic> descriptor, QueryInfo queryInfo)
		{
			if (string.IsNullOrWhiteSpace(queryInfo.FieldName)) { throw new ArgumentNullException(nameof(queryInfo), "Field name in query info is empty or null!"); }
			if (queryInfo.Values.Count != 2 || (DateTime)queryInfo.Values[0] > (DateTime)queryInfo.Values[1]) { throw new Exception("There is a consistency between the datetimes"); }

			var startDate = (DateTime)queryInfo.Values[0];
			var endDate = (DateTime)queryInfo.Values[1];

			return descriptor.DateRange(r => r
				.Field(queryInfo.FieldName)
				.GreaterThan(startDate.ToString(dateTimePattern))
				.LessThan(endDate.ToString(dateTimePattern))
				.Format(dateTimePattern)
			);
		}
	}
}
