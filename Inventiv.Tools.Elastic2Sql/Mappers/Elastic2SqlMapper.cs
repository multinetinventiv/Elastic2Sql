using System;
using System.Collections.Generic;
using System.Data;
using Inventiv.Tools.Elastic2Sql.DatabaseInformation;

namespace Inventiv.Tools.Elastic2Sql.Mappers
{
	public class Elastic2SqlMapper : IMapper
	{
		private readonly IDatabaseInformation sourceDbInfo;
		private readonly IDatabaseInformation targetDbInfo;

		public Elastic2SqlMapper(IDatabaseInformation sourceDbInfo, IDatabaseInformation targetDbInfo)
		{
			this.sourceDbInfo = sourceDbInfo;
			this.targetDbInfo = targetDbInfo;
		}

		public DataTable MapToBulkInsert(List<dynamic> source)
		{
			var dataTable = new DataTable();
			var dataColumns = GetDataColumns().ToArray();
			dataTable.Columns.AddRange(dataColumns);

			foreach (var item in source)
			{
				var dr = dataTable.NewRow();

				for (var index = 0; index < dataColumns.Length; index++)
				{
					var dataColumn = dataColumns[index];
					var sourceColumn = sourceDbInfo.Columns[index];

					var valueBySourceType = DynamicObjectReader.GetPropertyValue(item, sourceColumn.Name, sourceColumn.Type);

					if (dataColumn.DataType == sourceColumn.Type)
					{
						dr[dataColumn] = valueBySourceType;
						continue;
					}

					dr[dataColumn] = ConvertBetweenDifferentTypes(valueBySourceType, dataColumn.DataType);
				}

				dataTable.Rows.Add(dr);
			}

			return dataTable;
		}

		#region private

		private List<DataColumn> GetDataColumns()
		{
			var list = new List<DataColumn>();

			foreach (var column in targetDbInfo.Columns)
			{
				list.Add(new DataColumn
				{
					ColumnName = column.Name,
					DataType = column.Type
				});
			}

			return list;
		}

		private object ConvertBetweenDifferentTypes(object value, Type targetType)
		{
			if (targetType == typeof(Guid)) { return new Guid(value.ToString()); }
			if (targetType == typeof(byte))
			{
				return byte.Parse(value.ToString() == "SUCCESS" ? "1" : "0");
			}

			return value;
		}

		#endregion
	}
}
