namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playerteams : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "TeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.Players", "TeamId");
            AddForeignKey("dbo.Players", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "TeamId" });
            DropColumn("dbo.Players", "TeamId");
        }
    }
}
