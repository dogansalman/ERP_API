using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class ProductionPersonnel
    {
        //Properties
        [Key]
        public int id { get; set; }
        public int production_id { get; set; }
        public int personel_id { get; set; }
        [NotMapped]
        public ICollection<ProductionPersonnelOperation> personnel_operation { get; set; }

    }
}