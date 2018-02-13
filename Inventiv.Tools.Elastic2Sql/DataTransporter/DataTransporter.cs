using System;
using System.Collections.Generic;
using System.Reflection;
using Inventiv.Tools.Elastic2Sql.Repository;
using log4net;

namespace Inventiv.Tools.Elastic2Sql.DataTransporters
{
	public class DataTransporter : IDataTransporter
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IRepository sourceRepository;

		public DataTransporter(IRepository sourceRepository)
		{
			this.sourceRepository = sourceRepository;
		}

		public void Transport(DateTime startDate, DateTime endDate, int dataCount)
		{
			try
			{
				

			}
			catch (Exception e)
			{
				logger.Error("When transporting data throws exception", e);
				throw;
			}
		}
	}
}
