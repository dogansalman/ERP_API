using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class OrderStock
    {
      
        public StockCards order_stock { get; set; }
        public int order_unit { get; set; }
        public int produced_orderstock  { get; set; }
        public string process_no { get; set; }
        
    }
    public class OrderStockDetail
    {
        public StockCards order_stock { get; set; }
        public int over_date { get; set; }
        public int customer_id { get; set; }

    }

    public class Orders
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int customer_id { get; set; }
        [Required]
        public DateTime over_date { get; set; }
        [StringLength(1000)]
        public string order_note { get; set; }
        public bool is_production { get; set; } = false;
        public bool is_complated { get; set; } = false;
        [NotMapped]
        public OrderStock order_stocks { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }
}