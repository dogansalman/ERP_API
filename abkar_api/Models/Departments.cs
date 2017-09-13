using System;
using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class Departments
    {
        
        //Properties
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
        public string role { get; set; }

    }
}