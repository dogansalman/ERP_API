using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class StockTypes
    {
        //Properties
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? added_date { get; set; }
        public DateTime? created_date { get; set; }

    }
}