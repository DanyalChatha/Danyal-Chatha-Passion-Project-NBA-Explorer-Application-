namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accoladeplayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accolades",
                c => new
                    {
                        AccoladeId = c.Int(nullable: false, identity: true),
                        AccoladeName = c.String(),
                        AccoladeYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccoladeId);
            
            CreateTable(
                "dbo.AccoladePlayers",
                c => new
                    {
                        Accolade_AccoladeId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Accolade_AccoladeId, t.Player_PlayerId })
                .ForeignKey("dbo.Accolades", t => t.Accolade_AccoladeId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .Index(t => t.Accolade_AccoladeId)
                .Index(t => t.Player_PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccoladePlayers", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.AccoladePlayers", "Accolade_AccoladeId", "dbo.Accolades");
            DropIndex("dbo.AccoladePlayers", new[] { "Player_PlayerId" });
            DropIndex("dbo.AccoladePlayers", new[] { "Accolade_AccoladeId" });
            DropTable("dbo.AccoladePlayers");
            DropTable("dbo.Accolades");
        }
    }
}
