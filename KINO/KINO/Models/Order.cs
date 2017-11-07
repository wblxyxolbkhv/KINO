﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KINO.Models
{
    public class Order
    {
        [Key]
        public int LINK { get; set; }
        // Стоимость заказа
        public int Cost { get; set; }
        //Ключ подтверждения
        public string ValidationKey { get; set; }
        // Ссылка на клиента 
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}