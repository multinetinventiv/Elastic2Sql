using System.Collections.Generic;
using System.Data;

namespace Inventiv.Tools.Elastic2Sql.Mappers
{
	public interface IMapper
	{
		DataTable MapToBulkInsert(List<object> source);
	}
}
