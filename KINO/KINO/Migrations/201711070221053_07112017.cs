namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07112017 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Seats", new[] { "ApplicationUserId" });
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Cost = c.Int(nullable: false),
                        ValidationKey = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LINK)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            AddColumn("dbo.Seats", "OrderLINK", c => c.Int());
            CreateIndex("dbo.Seats", "OrderLINK");
            AddForeignKey("dbo.Seats", "OrderLINK", "dbo.Orders", "LINK");
            DropColumn("dbo.Seats", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "ApplicationUserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Seats", "OrderLINK", "dbo.Orders");
            DropForeignKey("dbo.Orders", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ApplicationUserId" });
            DropIndex("dbo.Seats", new[] { "OrderLINK" });
            DropColumn("dbo.Seats", "OrderLINK");
            DropTable("dbo.Orders");
            CreateIndex("dbo.Seats", "ApplicationUserId");
            AddForeignKey("dbo.Seats", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
