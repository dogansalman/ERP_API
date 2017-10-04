using System;
using System.ComponentModel.DataAnnotations;
namespace abkar_api.Models
{
    public class Machines
    {
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Required]
        public string name { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
    }
}