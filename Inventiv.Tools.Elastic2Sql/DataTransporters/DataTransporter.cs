using System;
using System.Reflection;
using Inventiv.Tools.Elastic2Sql.Mappers;
using Inventiv.Tools.Elastic2Sql.Repository;
using log4net;

namespace Inventiv.Tools.Elastic2Sql.DataTransporters
{
	public class DataTransporter : IDataTransporter
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IRepository sourceRepository;
		private readonly IRepository targetRepository;
		private readonly IMapper mapper;

		public DataTransporter(IRepository sourceRepository, IRepository targetRepository, IMapper mapper)
		{
			this.sourceRepository = sourceRepository;
			this.targetRepository = targetRepository;
			this.mapper = mapper;
		}

		public void Transport(DateTime startDate, DateTime endDate, int dataCount)
		{
			try
			{
				var sourceValues = sourceRepository.Get(dataCount, startDate, endDate);
				
				var targetValuesDt = mapper.MapToBulkInsert(sourceValues);

				targetRepository.BulkInsert(targetValuesDt);

			}
			catch (Exception e)
			{
				logger.Error("When transporting data thrown exception", e);
				throw;
			}
		}
	}
}
