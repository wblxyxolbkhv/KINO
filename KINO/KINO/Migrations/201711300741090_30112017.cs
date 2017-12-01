namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30112017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "Archived", c => c.Boolean());
            AddColumn("dbo.Sessions", "Archived", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Archived");
            DropColumn("dbo.Films", "Archived");
        }
    }
}
