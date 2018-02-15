using System.Collections.Generic;

namespace Inventiv.Tools.Elastic2Sql.Query
{
	public class QueryInfo
	{
		public string FieldName { get; set; }
		public List<object> Values { get; set; }
		public QueryType QueryType { get; set; }
	}
}
