namespace Danyal_Chatha_Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teambio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "TeamBio", c => c.String());
            AddColumn("dbo.Teams", "TeamBio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "TeamBio");
            DropColumn("dbo.Players", "TeamBio");
        }
    }
}
