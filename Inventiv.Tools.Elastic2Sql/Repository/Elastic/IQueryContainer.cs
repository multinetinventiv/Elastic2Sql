using Nest;

namespace Inventiv.Tools.Elastic2Sql.Repository.Elastic
{
	public interface IQueryContainer
	{
		QueryContainer BuildQuery(QueryContainerDescriptor<dynamic> descriptor, QueryInfo queryInfo);
	}
}
