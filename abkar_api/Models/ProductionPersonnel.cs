using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class ProductionPersonnel
    {
        //Properties
        [Key]
        public int id { get; set; }
        [Required]
        public int production_id { get; set; }
        [Required]
        public int personel_id { get; set; }

    }
}