using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KINO.Models;

namespace KINO.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Affiche")]
        public ActionResult Affiche()
        {

            IEnumerable<Film> films = db.Films;
            foreach (Film film in films)
            {
                film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
                film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            }
            ViewBag.Films = films;
            return View();
        }
        [Route("Film")]
        public ActionResult Film(int link)
        {
            Film film = db.Films.FirstOrDefault(f => f.LINK == link);
            film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
            film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            film.Director = db.Directors.FirstOrDefault(director => director.LINK == film.DirectorLINK);
            film.Country = db.Countries.FirstOrDefault(country => country.LINK == film.CountryLINK);
            ViewBag.Film = film;

            IEnumerable<Session> sessions = db.Sessions.Where(s => s.FilmLINK == film.LINK);
            foreach (Session s in sessions)
            {
                s.Hall = db.Halls.FirstOrDefault(h => h.LINK == s.HallLINK);
            }
            ViewBag.Sessions = sessions;

            return View();
        }
        [Route("Contact")]
        public ActionResult Contact()
        {

            return View();
        }
        [Route("Halls")]
        public ActionResult Halls()
        {
            IEnumerable<Hall> halls = db.Halls;
            ViewBag.Halls = halls;

            return View();
        }
        [Route("Session")]
        public ActionResult Sessions(int link)
        {
            Session session = db.Sessions.FirstOrDefault(s => s.LINK == link);

            session.Film = db.Films.FirstOrDefault(f => f.LINK == session.FilmLINK);
            session.Hall = db.Halls.FirstOrDefault(h => h.LINK == session.HallLINK);
            List<Seat> seats = db.Seats.Where(s => s.SessionLINK == session.LINK).ToList();
            session.Seats = seats;
            ViewBag.Session = session;
            return View();
        }
        [HttpPost]
        public ActionResult BookSeat(int row, int number)
        {
            return View();
        }
    }
}