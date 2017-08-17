using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class ProductionProcessStocks
    {
        //Properties
        public int production_id { get; set; }
        public int stock_id { get; set; }
        public int production_time { get; set; }
        
    }
}