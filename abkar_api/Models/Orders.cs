using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class OrderStock
    {
        public int stockcard_id { get; set; }
        public int order_unit { get; set; }
    }

    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int customer_id { get; set; }
        [Required]
        public DateTime over_date { get; set; }
        [StringLength(1000)]
        public string order_note { get; set; }
        [NotMapped]
        public  ICollection<OrderStock> order_stocks { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }
}