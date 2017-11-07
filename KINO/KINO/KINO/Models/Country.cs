using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Country
    {
        [Key]
        public int LINK { get; set; }
        //Называние страны
        public string Name { get; set; }
    }
}