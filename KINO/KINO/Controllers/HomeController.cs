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
        public ActionResult Film(int link)
        {
            Film film = db.Films.FirstOrDefault(f => f.LINK == link);
            film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
            film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            film.Director = db.Directors.FirstOrDefault(director => director.LINK == film.DirectorLINK);
            film.Country = db.Countries.FirstOrDefault(country => country.LINK == film.CountryLINK);
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
    }
}