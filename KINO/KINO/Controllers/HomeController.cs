using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KINO.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace KINO.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            IEnumerable<Hall> halls = db.Halls;
            ViewBag.Halls = halls;

            IEnumerable<Film> films = db.Films.Where(x => x.Archived !=true);
            ViewBag.Films = films;
            return View();
        }

        public ActionResult Affiche(int page = 1)
        {

            IEnumerable<Film> films = db.Films/*.Include(x => x.Genre).Include(x => x.AgeLimit)*/.Where(x => x.Archived != true);

            /*foreach (Film film in films.ToArray())
            {
                film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
                film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            }*/

            int pageSize = 1;

            if (page > (int)Math.Ceiling((decimal)films.Count() / pageSize))
            {
                page = 1;
            }
            
            PageInfo pi = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = films.Count() };
            var filmList = films.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.Films = filmList;
            ViewBag.PageInfo = pi;

            return View();
            
        }

        public ActionResult Film(int? id)
        {
            if(id == null)
            {
                return View("Error");
            }

            Film film = db.Films.FirstOrDefault(f => f.LINK == id);
            film.Genre = db.Genres.FirstOrDefault(genre => genre.LINK == film.GenreLINK);
            film.AgeLimit = db.AgeLimits.FirstOrDefault(ageLimit => ageLimit.LINK == film.AgeLimitLINK);
            film.Director = db.Directors.FirstOrDefault(director => director.LINK == film.DirectorLINK);
            film.Country = db.Countries.FirstOrDefault(country => country.LINK == film.CountryLINK);
            IEnumerable<Session> sessions = db.Sessions.Where(s => s.FilmLINK == film.LINK && s.Archived != true);
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
        [Authorize]
        public ActionResult Sessions(int id = -1)
        {
            try
            {
                Session session = db.Sessions.FirstOrDefault(s => s.LINK == id && s.Archived != true);
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
        [HttpPost]
        public ActionResult CreateOrder()
        {
            Order order = new Order();
            string validationKey = GetRandomKey();
            while (db.Orders.Where(o => o.ValidationKey == validationKey).Count() > 0)
            {
                validationKey = GetRandomKey();
            }
            NameValueCollection form = Request.Form;
            int cost = Convert.ToInt32(form.Get("session-cost"));
            cost *= form.Count - 2;
            string applicationUserId = User.Identity.GetUserId();

            order.ValidationKey = validationKey;
            order.ApplicationUserId = applicationUserId;
            order.Cost = cost;

            db.Orders.Add(order);
            db.SaveChanges();


            int sessionLink = Convert.ToInt32(form.Get("session-link"));

            for (int i = 0;i<form.Count; i++)
            {
                if (form.GetKey(i) != "session-cost" && form.GetKey(i) != "session-link")
                {
                    Seat newSeat = new Seat();
                    int value = Convert.ToInt32(form.Get(i));
                    int row = value / 1000;
                    int number = value % 1000;

                    newSeat.Row = row;
                    newSeat.Number = number;
                    newSeat.IsBooked = true;
                    newSeat.OrderLINK = order.LINK;
                    newSeat.SessionLINK = sessionLink;

                    db.Seats.Add(newSeat);
                    db.SaveChanges();
                }
            }

            ViewBag.ValidationKey = validationKey;
            return View();
        }



        Random rnd = new Random();
        private string GetRandomKey()
        {
            string key = "";
            string chars = "0123456789-QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 6; i++)
            {
                int j = rnd.Next(0, chars.Length - 1);
                key += chars[j];
            }
            return key;
        }
    }
}