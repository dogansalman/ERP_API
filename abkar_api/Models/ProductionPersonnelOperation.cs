using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class ProductionPersonnelOperation
    {
        //Properties
        [Key]
        public int id { get; set; }
        public int production_personel_id { get; set; }
        [StringLength(255)]
        public string machine { get; set; }
        [StringLength(255)]
        public string operation { get; set; }
        public int operation_time { get; set; }

    }
}