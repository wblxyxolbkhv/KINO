using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class AgeLimit
    {
        [Key]
        public int LINK { get; set; }
        //Значение
        public int Amout { get; set; }
    }
}