using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class OrderStocks
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Index("IX_OrderStock", 1, IsUnique = true)]
        public int order_id { get; set; }
        [Required]
        [Index("IX_OrderStock", 2, IsUnique = true)]
        public int stockcard_id { get; set; }
        [Required]
        public int order_unit { get; set; }
        public int produced_orderstock { get; set; } = 0;
        public DateTime created_date { get; set; } = DateTime.Now;

    }
}