using System.Collections.Generic;

namespace Inventiv.Tools.Elastic2Sql.Repository
{
	public interface IRepository
	{
		List<dynamic> GetAll(int takeCount);
		List<object> Get(List<QueryInfo> queryInformation, int takeCount);
		void Insert(List<string> columnNames, params object[] values);
		void Update(List<string> columnNames, params object[] values);
	}
}
