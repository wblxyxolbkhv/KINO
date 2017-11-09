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
        public ActionResult Index()
        {
            return View();
        }

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
        public ActionResult Film(int id)
        {
            Film film = db.Films.FirstOrDefault(f => f.LINK == id);
            film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
            film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            film.Director = db.Directors.FirstOrDefault(director => director.LINK == film.DirectorLINK);
            film.Country = db.Countries.FirstOrDefault(country => country.LINK == film.CountryLINK);
            IEnumerable<Session> sessions = db.Sessions.Where(s => s.FilmLINK == film.LINK);
            foreach (Session session in sessions)
            {
                session.Hall = db.Halls.FirstOrDefault(h => h.LINK == session.HallLINK);
            }
            ViewBag.Sessions = sessions;
            ViewBag.Film = film;

            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Halls()
        {
            IEnumerable<Hall> halls = db.Halls;
            ViewBag.Halls = halls;

            return View();
        }
        public ActionResult Sessions(int id = -1)
        {
            try
            {
                Session session = db.Sessions.FirstOrDefault(s => s.LINK == id);
                if (session == null) { throw new KeyNotFoundException(); }
                IEnumerable<Seat> seats = db.Seats.Where(s => s.SessionLINK == session.LINK);
                session.Seats = seats.ToList();
                session.Film = db.Films.FirstOrDefault(f => f.LINK == session.FilmLINK);
                session.Hall = db.Halls.FirstOrDefault(h => h.LINK == session.HallLINK);
                ViewBag.Session = session;
            }
            catch
            {
                return View("Error");
            }
            return View();
        }
    }
}