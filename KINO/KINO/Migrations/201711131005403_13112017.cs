namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13112017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Date");
        }
    }
}
