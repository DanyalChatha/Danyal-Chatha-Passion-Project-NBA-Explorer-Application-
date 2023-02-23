namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "PlayerHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "PicExtension");
            DropColumn("dbo.Players", "PlayerHasPic");
        }
    }
}
