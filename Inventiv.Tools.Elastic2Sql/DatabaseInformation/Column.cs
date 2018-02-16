using System;

namespace Inventiv.Tools.Elastic2Sql.DatabaseInformation
{
	public class Column
	{
		public Column(string name,Type type)
		{
			Name = name;
			Type = type;
		}

		public string Name { get; }
		public Type Type { get; }
	}
}
