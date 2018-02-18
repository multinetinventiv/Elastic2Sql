using System;
using Inventiv.Tools.Elastic2Sql.Models;

namespace Inventiv.Tools.Elastic2Sql.DataTransporters
{
	public interface IDataTransporter
	{
		void Transport(DateTime startDate, DateTime endDate, int dataCount, TransportHistory transportHistory = null);
	}
}
