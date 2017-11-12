namespace KINO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgeLimits",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Amout = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.LINK);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LINK);
            
            CreateTable(
                "dbo.Directors",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.LINK);
            
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Poster = c.String(),
                        ReleaseYear = c.Int(nullable: false),
                        CountryLINK = c.Int(),
                        GenreLINK = c.Int(),
                        DirectorLINK = c.Int(),
                        Duration = c.String(),
                        AgeLimitLINK = c.Int(),
                    })
                .PrimaryKey(t => t.LINK)
                .ForeignKey("dbo.AgeLimits", t => t.AgeLimitLINK)
                .ForeignKey("dbo.Countries", t => t.CountryLINK)
                .ForeignKey("dbo.Directors", t => t.DirectorLINK)
                .ForeignKey("dbo.Genres", t => t.GenreLINK)
                .Index(t => t.CountryLINK)
                .Index(t => t.GenreLINK)
                .Index(t => t.DirectorLINK)
                .Index(t => t.AgeLimitLINK);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LINK);
            
            CreateTable(
                "dbo.Halls",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        SeatsNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LINK);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        Row = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        SessionLINK = c.Int(),
                    })
                .PrimaryKey(t => t.LINK)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Sessions", t => t.SessionLINK)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.SessionLINK);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        LINK = c.Int(nullable: false, identity: true),
                        FilmLINK = c.Int(),
                        SessionTime = c.DateTime(nullable: false),
                        HallLINK = c.Int(),
                        Cost = c.Int(),
                    })
                .PrimaryKey(t => t.LINK)
                .ForeignKey("dbo.Films", t => t.FilmLINK)
                .ForeignKey("dbo.Halls", t => t.HallLINK)
                .Index(t => t.FilmLINK)
                .Index(t => t.HallLINK);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Seats", "SessionLINK", "dbo.Sessions");
            DropForeignKey("dbo.Sessions", "HallLINK", "dbo.Halls");
            DropForeignKey("dbo.Sessions", "FilmLINK", "dbo.Films");
            DropForeignKey("dbo.Seats", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Films", "GenreLINK", "dbo.Genres");
            DropForeignKey("dbo.Films", "DirectorLINK", "dbo.Directors");
            DropForeignKey("dbo.Films", "CountryLINK", "dbo.Countries");
            DropForeignKey("dbo.Films", "AgeLimitLINK", "dbo.AgeLimits");
            DropIndex("dbo.Sessions", new[] { "HallLINK" });
            DropIndex("dbo.Sessions", new[] { "FilmLINK" });
            DropIndex("dbo.Seats", new[] { "SessionLINK" });
            DropIndex("dbo.Seats", new[] { "ApplicationUserId" });
            DropIndex("dbo.Films", new[] { "AgeLimitLINK" });
            DropIndex("dbo.Films", new[] { "DirectorLINK" });
            DropIndex("dbo.Films", new[] { "GenreLINK" });
            DropIndex("dbo.Films", new[] { "CountryLINK" });
            DropTable("dbo.Sessions");
            DropTable("dbo.Seats");
            DropTable("dbo.Halls");
            DropTable("dbo.Genres");
            DropTable("dbo.Films");
            DropTable("dbo.Directors");
            DropTable("dbo.Countries");
            DropTable("dbo.AgeLimits");
        }
    }
}
