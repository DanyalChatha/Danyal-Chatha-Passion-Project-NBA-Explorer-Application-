namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Teams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            AddColumn("dbo.Players", "PlayerPosition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "PlayerPosition");
            DropTable("dbo.Teams");
        }
    }
}
