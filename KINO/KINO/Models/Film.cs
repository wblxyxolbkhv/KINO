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
        [Display(Name = "Название")]
        public string Name { get; set; }
        //Постер
        [Display(Name = "Название файла-постера")]
        public string Poster { get; set; }
        //Год выпуска
        [Display(Name = "Год выхода на экран")]
        public int ReleaseYear { get; set; }
        //Страна
        public Country Country { get; set; }
        [Display(Name = "Страна производитель")]
        public int? CountryLINK { get; set; }
        //Жанр
        public Genre Genre { get; set; }
        [Display(Name = "Жанр")]
        public int? GenreLINK { get; set; }
        //Режиссер
        public Director Director { get; set; }
        [Display(Name = "Режиссер")]
        public int? DirectorLINK { get; set; }
        //Продолжительность
        [Display(Name = "Длительность (в минутах)")]
        public string Duration { get; set; }
        //Возрастное ограничение 
        public AgeLimit AgeLimit { get; set; }
        [Display(Name = "Возрастное ограничение")]
        public int? AgeLimitLINK { get; set; }
        //Флаг актуальности
        public bool? Archived { get; set; }

    }
}