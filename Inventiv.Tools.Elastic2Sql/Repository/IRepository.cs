using System.Collections.Generic;
using Inventiv.Tools.Elastic2Sql.Query;

namespace Inventiv.Tools.Elastic2Sql.Repository
{
	public interface IRepository
	{
		List<dynamic> GetAll(int takeCount);
		List<object> Get(List<QueryInfo> queryInformation, int takeCount = 100, params object[] values);
		List<object> Get(int takeCount = 100, params object[] values);
		void Insert(List<string> columnNames, params object[] values);
		void Update(List<string> columnNames, params object[] values);
	}
}
