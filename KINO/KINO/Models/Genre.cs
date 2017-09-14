using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Genre
    {
        [Key]
        public int LINK { get; set; }
        //Название жанра
        public string Name { get; set; }
    }
}