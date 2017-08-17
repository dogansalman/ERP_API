using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class Personnel
    {
        //Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string lastname { get; set; }
        [Required]
        [StringLength(255)]
        public string username { get; set; }
        [Required]
        [StringLength(255)]
        public string password { get; set; }
        [Required]
        [Range(1,4)]
        public int department_id { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }



    }
}