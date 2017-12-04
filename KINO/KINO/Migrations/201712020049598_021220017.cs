namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _021220017 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sessions", "FilmLINK", "dbo.Films");
            DropForeignKey("dbo.Sessions", "HallLINK", "dbo.Halls");
            DropIndex("dbo.Sessions", new[] { "FilmLINK" });
            DropIndex("dbo.Sessions", new[] { "HallLINK" });
            AlterColumn("dbo.Films", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Films", "Poster", c => c.String(nullable: false));
            AlterColumn("dbo.Films", "Duration", c => c.String(nullable: false));
            AlterColumn("dbo.Sessions", "FilmLINK", c => c.Int(nullable: false));
            AlterColumn("dbo.Sessions", "HallLINK", c => c.Int(nullable: false));
            CreateIndex("dbo.Sessions", "FilmLINK");
            CreateIndex("dbo.Sessions", "HallLINK");
            AddForeignKey("dbo.Sessions", "FilmLINK", "dbo.Films", "LINK", cascadeDelete: true);
            AddForeignKey("dbo.Sessions", "HallLINK", "dbo.Halls", "LINK", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "HallLINK", "dbo.Halls");
            DropForeignKey("dbo.Sessions", "FilmLINK", "dbo.Films");
            DropIndex("dbo.Sessions", new[] { "HallLINK" });
            DropIndex("dbo.Sessions", new[] { "FilmLINK" });
            AlterColumn("dbo.Sessions", "HallLINK", c => c.Int());
            AlterColumn("dbo.Sessions", "FilmLINK", c => c.Int());
            AlterColumn("dbo.Films", "Duration", c => c.String());
            AlterColumn("dbo.Films", "Poster", c => c.String());
            AlterColumn("dbo.Films", "Name", c => c.String());
            CreateIndex("dbo.Sessions", "HallLINK");
            CreateIndex("dbo.Sessions", "FilmLINK");
            AddForeignKey("dbo.Sessions", "HallLINK", "dbo.Halls", "LINK");
            AddForeignKey("dbo.Sessions", "FilmLINK", "dbo.Films", "LINK");
        }
    }
}
