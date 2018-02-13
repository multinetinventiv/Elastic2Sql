using System.Collections.Generic;

namespace Inventiv.Tools.Elastic2Sql.Repository
{
	public class QueryInfo
	{
		public string FieldName { get; set; }
		public List<object> Values { get; set; }
		public QueryContainerType Type { get; set; }
	}
}
