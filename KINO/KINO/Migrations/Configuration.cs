namespace KINO.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<KINO.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "KINO.Models.ApplicationDbContext";
        }

        protected override void Seed(KINO.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            Hall smallHall = new Hall() { LINK = 1, Name = "Малый зал", SeatsNumber = 50 };
            Hall mediumHall = new Hall() { LINK = 2, Name = "Средный зал", SeatsNumber = 80 };
            Hall hugeHall = new Hall() { LINK = 3, Name = "Большой зал", SeatsNumber = 150 };
            Hall hugestHallInTheWorld = new Hall() { LINK = 4, Name = "Самый огромный зал в мире", SeatsNumber = 10000 };

            context.Halls.Add(smallHall);
            context.Halls.Add(mediumHall);
            context.Halls.Add(hugeHall);
            context.Halls.Add(hugestHallInTheWorld);

            AgeLimit zero = new AgeLimit() { LINK = 1, Amout = 0, Value = "0+" };
            AgeLimit six = new AgeLimit() { LINK = 2, Amout = 6, Value = "6+" };
            AgeLimit twelve = new AgeLimit() { LINK = 3, Amout = 12, Value = "12+" };
            AgeLimit sixteen = new AgeLimit() { LINK = 4, Amout = 16, Value = "16+" };
            AgeLimit eighteen = new AgeLimit() { LINK = 5, Amout = 18, Value = "18+" };
            AgeLimit twentyone = new AgeLimit() { LINK = 6, Amout = 21, Value = "21+" };

            context.AgeLimits.Add(zero);
            context.AgeLimits.Add(six);
            context.AgeLimits.Add(twelve);
            context.AgeLimits.Add(sixteen);
            context.AgeLimits.Add(eighteen);
            context.AgeLimits.Add(twentyone);

            Country MOTHERRUSSIA = new Country() { LINK = 1, Name = "Россия" };
            context.Countries.Add(MOTHERRUSSIA);

            Director dir = new Director() { LINK = 1, Name = "АЛЕХА", Surname = "ВЫЙДЕТ" };
            context.Directors.Add(dir);

            Genre horror = new Genre() { LINK = 1, Name = "Ужасы" };
            context.Genres.Add(horror);

            Film coolFilm = new Film() { LINK = 1, Name = "Большой КУШ", GenreLINK = 1, ReleaseYear = DateTime.Now.Year, AgeLimitLINK = 2, Duration = "Вечность", DirectorLINK = 1, CountryLINK = 1 };
            context.Films.Add(coolFilm);

            Session sess = new Session() { LINK = 1, FilmLINK = 1, HallLINK = 4, SessionTime = DateTime.Now };
            context.Sessions.Add(sess);

            Seat seat = new Seat() { LINK = 1, IsBooked = false, Number = 10, Row = 1, SessionLINK = 1 };
            context.Seats.Add(seat);

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
