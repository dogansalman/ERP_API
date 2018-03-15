using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace abkar_api.ModelViews
{
    public class _ProductionPersonnelsWithOperation
    {
        public ICollection<_ProductionPersonnelOperation> operations { get; set; }
        public _ProductionPersonnel personnel { get; set; }
    }
    public class _ProductionPersonnel
    {
        
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string lastname { get; set; }
        [Required]
        [Range(1, 4)]
        public int department_id { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Durum bilgisi hatalı")]
        public bool state { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
        public bool deleted { get; set; } = false;



    }
}