﻿using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models
{
    public class MomoInfoModel
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public decimal Amount { get; set; }
        public string FullName { get; set; }
        public DateTime DatePaid { get; set; }
    }
}
