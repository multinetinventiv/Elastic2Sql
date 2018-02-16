using System.Collections.Generic;

namespace Inventiv.Tools.Elastic2Sql.DatabaseInformation
{
	public interface IDatabaseInformation
	{
		string DatabaseName { get; }
		string TableName { get; }
		List<Column> Columns { get; }
		List<QueryInfo> Queries { get; }
	}
}