using System;
using System.ComponentModel.DataAnnotations;


namespace abkar_api.Models
{
    public class Productions
    {
        //Properties
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        public string description { get; set; }
        public int production_personel { get; set; }
        [StringLength(255)]
        public string creator_fullname { get; set; }
        public int creator_id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime? updated_date { get; set; }
        [StringLength(255)]
        public string state { get; set; }
        [StringLength(255)]
        public string product { get; set; }
        public int count { get; set; }
        public int order_id { get; set; }
    }
}