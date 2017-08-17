using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class Suppliers
    {
        //Properties
        [Key]
        public int id { get; set; }
        public string company { get; set; }
        public string adress { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }

    }
}