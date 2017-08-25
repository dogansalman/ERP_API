using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class StockTypes
    {
        //Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Tanım en fazla 255 karakter olmalıdır.")]
        public string name { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }
}