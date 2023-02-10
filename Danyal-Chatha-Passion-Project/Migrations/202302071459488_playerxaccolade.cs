namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playerxaccolade : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccoladePlayers", newName: "PlayerAccolades");
            DropPrimaryKey("dbo.PlayerAccolades");
            AddPrimaryKey("dbo.PlayerAccolades", new[] { "Player_PlayerId", "Accolade_AccoladeId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PlayerAccolades");
            AddPrimaryKey("dbo.PlayerAccolades", new[] { "Accolade_AccoladeId", "Player_PlayerId" });
            RenameTable(name: "dbo.PlayerAccolades", newName: "AccoladePlayers");
        }
    }
}
