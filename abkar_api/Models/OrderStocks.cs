using System;
using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class OrderStocks
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int order_id { get; set; }
        [Required]
        public int stockcard_id { get; set; }
        [Required]
        public int order_unit { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;

    }
}