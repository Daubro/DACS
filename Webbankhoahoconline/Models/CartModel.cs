﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Webbankhoahoconline.Models
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();

        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    }
}
