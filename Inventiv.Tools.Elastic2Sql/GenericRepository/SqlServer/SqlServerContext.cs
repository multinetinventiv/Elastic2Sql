using System.Data.Entity;
using Inventiv.Tools.Elastic2Sql.Helper;
using Inventiv.Tools.Elastic2Sql.Models;

namespace Inventiv.Tools.Elastic2Sql.GenericRepository.SqlServer
{
	public class SqlServerContext : DbContext
	{
		public SqlServerContext() : base($"name={Constants.SQL_SERVER_CONTEXT_NAME}") { }

		public virtual DbSet<TransportHistory> TransportHistories { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) { }
	}
}
