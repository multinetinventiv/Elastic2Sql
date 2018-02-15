using System;
using System.Collections.Generic;
using Inventiv.Tools.Elastic2Sql.Query;

namespace Inventiv.Tools.Elastic2Sql.DatabaseInformation
{
	public class DatabaseInformationByXml : IDatabaseInformation
	{
		private readonly dynamic xmlReader;

		public DatabaseInformationByXml(string xmlPath, RootNodeName rootNodeName)
		{
			xmlReader = new XmlReader(xmlPath);
			xmlReader = rootNodeName == RootNodeName.Elastic ? xmlReader.Elastic : xmlReader.Sql;
		}

		public string DatabaseName => xmlReader.Database.Name.Value;

		public string TableName => xmlReader.Table.Name.Value;

		public List<string> Columns
		{
			get
			{
				var list = new List<string>();
				foreach (var column in xmlReader.Table.Columns.Column)
				{
					list.Add(column.Name.Value);
				}

				return list;
			}
		}

		public List<QueryInfo> Queries
		{
			get
			{
				var list = new List<QueryInfo>();
				foreach (var query in xmlReader.Table.Queries.Query)
				{
					list.Add(new QueryInfo
					{
						 FieldName = query.ColumnName.Value,
						 QueryType = GetQueryTypeByName(query.QueryType.Value),
						 Values = GetValues(query.ColumnValues)
					});
				}

				return list;
			}
		}

		#region private

		private QueryType GetQueryTypeByName(string name)
		{
			return (QueryType)Enum.Parse(typeof(QueryType), name);
		}

		private List<object> GetValues(dynamic values)
		{
			var list = new List<object>();

			foreach (var value in values.ColumnValue)
			{
				list.Add(value.Value);
			}

			return list;
		}

		#endregion
	}
}
