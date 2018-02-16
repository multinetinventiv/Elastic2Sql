using System;
using System.Collections.Generic;
using System.Linq;

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

		public List<Column> Columns
		{
			get
			{
				var list = new List<Column>();
				foreach (var column in xmlReader.Table.Columns.Column)
				{
					var type = GetType(column.Type.Value);
					list.Add(new Column(column.Name.Value, type));
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

		private Type GetType(string typeName)
		{
			var type = typeof(string).Assembly.GetTypes().FirstOrDefault(t => t.Namespace == "System" && t.Name == typeName);
			if (type == null) { throw new Exception($"Not found type name in system types ({typeName})"); }

			return Type.GetType($"System.{typeName}");
		}

		#endregion
	}
}
