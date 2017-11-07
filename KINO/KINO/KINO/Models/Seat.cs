using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Seat
    {
        [Key]
        public int LINK { get; set; }
        //Ряд
        public int Row { get; set; }
        //Номер
        public int Number { get; set; }
        //Статус брони
        public bool IsBooked { get; set; }
        // Ссылка на клиента ??
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        // Сеанс
        public Session Session { get; set; }
        public int? SessionLINK { get; set; }
    }
}