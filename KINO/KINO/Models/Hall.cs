using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Hall
    {
        [Key]
        public int LINK { get; set; }
        //Название зала
        public string Name { get; set; }
        //Изображение
        public Image Image { get; set; }
        //Количество мест
        public int SeatsNumber { get; set; }
    }
}