using System.Collections.Generic;
using Inventiv.Tools.Elastic2Sql.Query;

namespace Inventiv.Tools.Elastic2Sql.DatabaseInformation
{
	public interface IDatabaseInformation
	{
		string DatabaseName { get; }
		string TableName { get; }
		List<string> Columns { get; }
		List<QueryInfo> Queries { get; }
	}
}