using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public delegate bool seatFindDelegate(Seat s);
    public class Session
    {
        [Key]
        public int LINK { get; set; }
        //Фильм 
        public Film Film { get; set; }
        [Display(Name = "Фильм")]
        public int? FilmLINK { get; set; }
        //Время
        [Display(Name = "Дата и время")]
        [DataType(DataType.Date)]
        public DateTime SessionTime { get; set; }
        //Зал
        public Hall Hall { get; set; }
        [Display(Name = "Зал")]
        public int? HallLINK { get; set; }

        public IEnumerable<Seat> Seats { get; set; }
        public int Cost { get; set; }
    }
    public class SessionComparer : Comparer<KINO.Models.Session>
    {
        public override int Compare(Session x, Session y)
        {
            return x.SessionTime.CompareTo(y);
        }
    }
}