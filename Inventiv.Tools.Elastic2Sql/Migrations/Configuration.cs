using System.Data.Entity.Migrations;
using Inventiv.Tools.Elastic2Sql.GenericRepository.SqlServer;

namespace Inventiv.Tools.Elastic2Sql.Migrations
{

	internal sealed class Configuration : DbMigrationsConfiguration<SqlServerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SqlServerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
