namespace Inventiv.Tools.Elastic2Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultAndDescriptionAddedToTransportHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Result = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransportHistories");
        }
    }
}
