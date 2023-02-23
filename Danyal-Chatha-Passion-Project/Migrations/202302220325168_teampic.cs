namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teampic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "TeamHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Teams", "TeamPicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "TeamPicExtension");
            DropColumn("dbo.Teams", "TeamHasPic");
        }
    }
}
