using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using abkar_api.ModelViews;

namespace abkar_api.Models
{
    public class Productions
    {
        //Properties
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Required]
        public string name { get; set; }
        [StringLength(1500)]
        [Required]
        public string description { get; set; }
        public bool is_complate { get; set; } = false;
        public bool is_cancel { get; set; } = false;
        public int product_id { get; set; } = 0;
        [Required]
        public int unit { get; set; }
        [Required]
        public int order_id { get; set; }
        [Required]
        public TimeSpan start_time { get; set; }
        [Required]
        public TimeSpan end_time { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
        [NotMapped]
        [Required]
        public ICollection<_ProductionPersonnelsWithOperation> production_personnels { get; set; }
        
    }
}