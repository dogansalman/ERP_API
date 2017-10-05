using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class ProductionPersonnel
    {
        //Properties
        [Key]
        public int production_id { get; set; }
        public int personel_id { get; set; }
        [StringLength(255)]
        public string personel_fullname { get; set; }

    }
}