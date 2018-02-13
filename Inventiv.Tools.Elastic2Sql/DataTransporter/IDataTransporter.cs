using System;

namespace Inventiv.Tools.Elastic2Sql.DataTransporters
{
	public interface IDataTransporter
	{
		void Transport(DateTime startDate, DateTime endDate);
	}
}
