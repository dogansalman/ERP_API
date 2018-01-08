using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class StockCardProcessNo
    {
        [Key]
        public int id { get; set; }
        public int stockcard_id { get; set; }
        [StringLength(500)]
        public string process_no { get; set; }
        [StringLength(500)]
        public string name { get; set; }
    }
}