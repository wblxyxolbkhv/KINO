using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class UserOrdersHistory
    {
        //Дата заказа
        [Display(Name = "Дата оформления заказа")]
        public DateTime? OrderDate { get; set; }
        //Дата сеанса
        [Display(Name = "Дата сеанса")]
        public DateTime? SessionDate { get; set; }
        //Название фильма
        [Display(Name = "Название фильма")]
        public string FilmName { get; set; }
        //Название зала
        [Display(Name = "Название зала")]
        public string HallName { get; set; }
        //Стоимость
        [Display(Name = "Стоимость заказа")]
        public int Cost { get; set; }
        //Кол-во мест 
        [Display(Name = "Количество билетов")]
        public int SeatAmount { get; set; }
    }
}