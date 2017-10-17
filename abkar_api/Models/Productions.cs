using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class Productions
    {
        //Properties
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        public string description { get; set; }
        public bool is_complate { get; set; } = false;
        public bool is_cancel { get; set; } = false;
        public int product_id { get; set; } = 0;
        public int unit { get; set; }
        public int order_id { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
        [NotMapped]
        public ICollection<ProductionPersonnel> production_personnel { get; set; }
        
    }
}