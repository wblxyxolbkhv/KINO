using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Session
    {
        [Key]
        public int LINK { get; set; }
        //Фильм 
        public Film Film { get; set; }
        public int? FilmLINK { get; set; }
        //Время
        public DateTime SessionTime { get; set; }
        //Зал
        public Hall Hall { get; set; }
        public int? HallLINK { get; set; }

    }
}