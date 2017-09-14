using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Film
    {   
        [Key]
        public int LINK { get; set; }
        //Название 
        public string Name { get; set; }
        //Постер
        public Image Poster { get; set; }
        //Год выпуска
        public DateTime ReleaseYear { get; set; }
        //Страна
        public Country Country { get; set; }
        public int? CountryLINK { get; set; }
        //Жанр
        public Genre Genre { get; set; }
        public int? GenreLINK { get; set; }
        //Режиссер
        public Director Director { get; set; }
        public int? DirectorLINK { get; set; }
        //Продолжительность
        public string Duration { get; set; }
        //Возрастное ограничение 
        public AgeLimit AgeLimit { get; set; }
        public int? AgeLimitLINK { get; set; }

    }
}