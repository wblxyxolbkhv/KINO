namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _091120171349 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Cost", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Cost");
        }
    }
}
