using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class ProductionStocks
    {
        //Properties
        [Key]
        public int id { get; set; }
        public int production_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string waybillNo { get; set; }
        [StringLength(255)]
        public string supplier { get; set; }
        [StringLength(255)]
        public string stock_type { get; set; }
    }
}