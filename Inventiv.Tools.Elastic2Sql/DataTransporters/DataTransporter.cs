using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Inventiv.Tools.Elastic2Sql.GenericRepository;
using Inventiv.Tools.Elastic2Sql.Mappers;
using Inventiv.Tools.Elastic2Sql.Models;
using Inventiv.Tools.Elastic2Sql.Repository;
using log4net;

namespace Inventiv.Tools.Elastic2Sql.DataTransporters
{
	public class DataTransporter : IDataTransporter
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IGenericRepository<TransportHistory> configRepository;
		private readonly IRepository sourceRepository;
		private readonly IRepository targetRepository;
		private readonly IMapper mapper;

		public DataTransporter(IGenericRepository<TransportHistory> configRepository, IRepository sourceRepository, IRepository targetRepository, IMapper mapper)
		{
			this.configRepository = configRepository;
			this.sourceRepository = sourceRepository;
			this.targetRepository = targetRepository;
			this.mapper = mapper;
		}

		public void Transport(DateTime startDate, DateTime endDate, int dataCount, TransportHistory transportHistory = null)
		{
			try
			{
				if (endDate > startDate) { throw new Exception($"End Date({endDate}) cannot greater than start date({startDate})"); }

				transportHistory = SetTransportInfo(startDate, endDate, transportHistory);

				#region Start to get values from elastic

				logger.Debug("Starting to get values from elastic...");

				#endregion
				var sourceValues = sourceRepository.Get(dataCount, startDate, endDate);
				#region Finished

				logger.Debug($"Finished to get values from elastic. Data count:{sourceValues.Count}");

				#endregion

				if (CheckElasticValuesCount(transportHistory, sourceValues)) { return; }

				#region Start to convert elastic values for sql

				logger.Debug("Starting to convert elastic values for sql...");

				#endregion
				var targetValuesDt = mapper.MapToBulkInsert(sourceValues);
				#region Finished

				logger.Debug("Finished to convert elastic values for sql.");

				#endregion

				#region Start to insert values to sql

				logger.Debug("Starting to insert values to sql...");

				#endregion
				targetRepository.BulkInsert(targetValuesDt);
				#region Finished

				logger.Debug("Finished to insert values to sql");

				#endregion

				var executionDateOfLastRecord = GetExecutionDateOfLastRecord(targetValuesDt);
				if (executionDateOfLastRecord == null) { throw new Exception("Cannot get execution date of last row!"); }

				Transport(executionDateOfLastRecord.Value, endDate, dataCount, transportHistory);
			}
			catch (Exception e)
			{
				var message = "When transporting data thrown exception";
				logger.Error(message, e);

				SetFailTransportationInfo(transportHistory, message, e);

				throw;
			}
		}

		private void SetFailTransportationInfo(TransportHistory transportHistory, string message, Exception e)
		{
			transportHistory.Result = Result.Fail;
			transportHistory.Description = $"{message}\n{e.Message}";

			configRepository.Update(transportHistory);
		}

		private bool CheckElasticValuesCount(TransportHistory transportHistory, List<object> sourceValues)
		{
			if (sourceValues.Count != 0) { return false; }

			transportHistory.Result = Result.Success;
			configRepository.Update(transportHistory);

			return true;
		}

		private TransportHistory SetTransportInfo(DateTime startDate, DateTime endDate, TransportHistory transportHistory)
		{
			if (transportHistory == null)
			{
				transportHistory =
					configRepository.Add(new TransportHistory { StartDate = startDate, EndDate = endDate, CreateDate = DateTime.Now });
			}
			else
			{
				transportHistory.StartDate = startDate;
				configRepository.Update(transportHistory);
			}
			return transportHistory;
		}

		/// <summary>
		/// It gets first column's value that's type is datetime
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		private DateTime? GetExecutionDateOfLastRecord(DataTable dataTable)
		{
			var lastRow = dataTable.Rows[dataTable.Rows.Count - 1];

			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.DataType != typeof(DateTime)) { continue; }

				return (DateTime)lastRow[column];
			}

			return null;

		}
	}
}
