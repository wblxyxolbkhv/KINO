using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Director
    {
        [Key]
        public int LINK { get; set; }
        //Имя 
        public string Name { get; set; }
        //Фамилия
        public string Surname { get; set; }
    }
}